using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 变量命令解释器 <br/>
    /// Variable command interpreter
    /// </summary>
    /// <remarks>
    /// 提供变量声明赋值，数学表达式处理以及变量释放功能。<br/>
    /// Provides variable declaration and assignment, mathematical expression processing and variable release functions
    /// </remarks>
    public class VariableInterpreter : BaseInterpreter
    {
        /// <summary>
        /// 不可用作变量名的保留关键字列表<br/>
        /// List of reserved keywords that cannot be used as variable names. 
        /// </summary>
        public static List<string> KeyWords = new() {
            "when",
            "release"
        };

        /// <summary> 
        /// 变量字典，键为变量名，值为变量值<br/>
        /// Variable dictionary, where the key is the variable name and the value is the variable value.  
        /// </summary>
        public static Dictionary<string, object> VariableList = new(); // 存储变量名及其对应的值

        // 有表达式的变量赋值
        private static void ProcessExpressionAssignment(string varName, string valueExpression)
        {
            var postfixExpression = InfixToPostfix(valueExpression); // 转换中缀表达式为后缀表达式
            var result = EvaluatePostfix(postfixExpression); // 计算后缀表达式的结果

            object valueToStore;

            if (long.TryParse(result, out long longResult))
            {
                valueToStore = longResult;
            }
            else if (double.TryParse(result, out double doubleResult))
            {
                if (!Regex.IsMatch(valueExpression, @"-?\d+\.\d+") && (doubleResult >= long.MaxValue || doubleResult <= long.MinValue))
                {
                    ScriptConsole.PrintErr($"Long integer variable: {varName} data size overflow (attempted to assign: {result}), stored as floating-point. (长整型变量: {varName} 数据大小溢出 (尝试赋值: {result}),已将其储存为浮点数型)");
                }
                valueToStore = doubleResult; // 保留小数部分
            }
            else
            {           
                ScriptConsole.PrintErr($"Variable: {varName} cannot be assigned, unexpected target value: {result} (变量: {varName} 无法赋值,意外的目标值: {result})");
                return;
            }

            if (valueToStore != null)
            {
                if (VariableList.ContainsKey(varName))
                {
                    VariableList[varName] = valueToStore; // 更新变量值
                }
                else
                {
                    VariableList.Add(varName, valueToStore); // 添加新变量
                }
            }
            else
            {               
                ScriptConsole.PrintErr($"Variable: {varName} cannot be assigned, unexpected target value: {result} (变量: {varName} 无法赋值,意外的目标值: {result})");
            }
        }

        /// <summary> 
        /// 处理变量赋值<br/>
        /// Handles variable assignment.  
        /// </summary>
        /// <param name="code"> 
        /// 命令标识符后的代码<br/>
        /// The code following the command identifier.  
        /// </param>
        public static void HandleVariableAssignment(string code)
        {
            // 获取赋值表达式
            string varName = code[..code.IndexOf('=')].Trim(); // 变量名
            string valueExpression = code[(code.IndexOf('=') + 1)..].Trim();// 赋值表达式

            // 检查变量名是否合法
            if (!CheckVariableName(varName))
            {           
                ScriptConsole.PrintErr("Variable name invalid (变量名不合法)");
                return;
            }
            // 字符串赋值处理
            if (valueExpression.StartsWith("\"") && valueExpression.EndsWith("\""))
            {
                valueExpression = valueExpression[1..^1].Trim(); // 去掉首尾的引号

                if (VariableList.ContainsKey(varName))
                {
                    VariableList[varName] = valueExpression;
                    return;
                }
                else
                {
                    VariableList.Add(varName, valueExpression);
                    return;
                }
            }
            // 赋值处理（如果赋值表达式包含引号则视作是字符串赋值）
            if (!CheckExpression(valueExpression))
            {
                AssignValue(varName, valueExpression);
                return;
            }

            // 处理合法数学表达式赋值
            ProcessExpressionAssignment(varName, valueExpression);
        }

        /// <summary> 
        /// 处理变量声明 <br/>
        /// Handles variable declaration.  
        /// </summary>
        /// <param name="code"> 
        /// 命令标识符后的代码<br/>
        /// The code following the command identifier.  
        /// </param>
        public static void HandleVariableDeclaration(string code)
        {
            // 检查变量名是否合法
            if (!CheckVariableName(code))
            {       
                ScriptConsole.PrintErr($"Variable name: {code} invalid (变量名: {code} 不合法)");
                return;
            }

            // 检查变量是否已存在
            if (VariableList.ContainsKey(code))
            {                
                ScriptConsole.PrintErr($"Variable: {code} already exists (变量: {code} 已存在)");
            }
            else
            {
                VariableList.Add(code, ""); // 添加变量并初始化为空
            }
        }
        
        // 变量赋值 
        private static void AssignValue(string varName, string value)
        {
            bool hasDecimalPoint = Regex.IsMatch(value, @"-?\d+\.\d+");

            // 处理变量赋值
            object valueToAssign = value; // 默认赋值为字符串

            // 检查是否是另一个变量的值
            if (VariableList.ContainsKey(value))
            {
                valueToAssign = VariableList[value]; // 赋值为另一个变量的值
            }
            else if (long.TryParse(value, out long intValue))
            {
                valueToAssign = intValue; // 整型变量赋值
            }
            else if (double.TryParse(value, out double doubleValue))
            {
                if (!hasDecimalPoint)
                {
                    ScriptConsole.PrintErr($"Long integer variable: {varName} data size overflow (attempted to assign: {value}), stored as floating-point. (长整型变量: {varName} 数据大小溢出 (尝试赋值: {value}),已将其储存为浮点数型)");
                }
                valueToAssign = doubleValue; // 浮点变量赋值
            }
            else if (bool.TryParse(value, out bool boolValue))
            {
                valueToAssign = boolValue; // 布尔变量赋值
            }

            // 赋值操作
            if (VariableList.ContainsKey(varName))
            {
                VariableList[varName] = valueToAssign; // 赋值为另一个变量的值
            }
            else
            {
                VariableList.Add(varName, valueToAssign);
            }
        }

        /// <summary> 
        /// 检查数学表达式是否合法<br/>
        /// Checks if a math expression is valid.
        /// </summary>
        /// <param name="expression"> 
        /// 要检查的表达式<br/>
        /// The expression to check. 
        /// </param>
        /// <returns> 
        /// 如果是有效数学表达式则返回true，否则返回false  <br/>
        /// Returns true if it is a valid mathematical expression, otherwise returns false.
        /// </returns>
        public static bool CheckExpression(string expression)
        {
            // 不以负号以外的符号开头
            if(expression.StartsWith("+") || expression.StartsWith("*") || expression.StartsWith("/") || expression.StartsWith("%"))
            {
                return false;
            }
            // 分割表达式检查操作数是否合法
            string[] parts = expression.Split(new char[] { '+', '-', '*', '/', '%' }, StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
            foreach (string part in parts)
            {
                // 如果操作数不是数字且不是合法变量名，则返回false
                if (!double.TryParse(part, out _) && !CheckVariableName(part))
                {
                    return false;
                }
            }
            // 定义正则表达式，匹配未被转义的运算符
            string pattern = @"(?<!\\)([+\-*/%])"; 
            // 使用正则表达式检查是否是合法表达式
            return Regex.IsMatch(expression, pattern);
        }
        /// <summary> 
        /// 从变量字典释放一个变量。<br/>
        /// Releases a variable from the variable dictionary.
        /// </summary>
        /// <param name="variable"> 
        /// 要释放的变量名 <br/>
        /// The name of the variable to release
        /// </param>        
        public static void ReleaseVariable(string variable)
        {
            if (VariableList.ContainsKey(variable))
            {
                VariableList.Remove(variable);
            }
            return;
        }

        /// <summary> 
        /// 检测提供的名称是否为合法变量名<br/>
        /// Checks if the provided name is a valid variable name. 
        /// </summary>
        /// <param name="name"> 
        /// 要检查的变量名 <br/>
        /// The name of the variable to check. 
        /// </param>
        /// <returns> 
        /// 如果变量名合法则返回true，否则返回false <br/>
        /// Returns true if the variable name is valid; otherwise, returns false. 
        /// </returns>
        public static bool CheckVariableName(string name)
        {
            name = name.Trim();
            // 正则表达式检查字符串是否只包含Unicode字母和下划线并且不包含关键字
            return Regex.IsMatch(name, @"^[\p{L}_][\p{L}\p{N}_]*$") 
                    && !bool.TryParse(name, out _) 
                    && !KeyWords.Contains(name);
        }

        /// <summary>
        /// 尝试获取指定变量的值。<br/> 
        /// Try to get the value of the specified variable.
        /// </summary>
        /// <param name="variable">
        /// 要获取值的变量名。<br/>
        /// The name of the variable whose value is to be retrieved.
        /// </param>
        /// <param name="value">
        /// 输出参数，接收获取到的变量值。返回时，如果变量存在且类型匹配，则返回对应的值；否则返回null。<br/> 
        /// Output parameter that receives the retrieved value of the variable. If the variable exists and the type matches, the corresponding value is returned; otherwise, null is returned.
        /// </param>
        /// <returns>
        /// 如果变量已定义且类型匹配，则返回true，并通过<c>value</c>输出对应的值；否则返回false，并且<c>value</c>为null。<br/>
        /// Returns true if the variable is defined and its type matches, and outputs the corresponding value through <c>value</c>; otherwise, returns false, and <c>value</c> is null.
        /// </returns>
        public static bool TryGetVariableValue(string variable, out object value)
        {
            if (VariableList.TryGetValue(variable, out var varValue))
            {
                if (varValue is double) // 变量是浮点数
                {
                    value = Convert.ToDouble(varValue);
                    return true;
                }
                else if (varValue is long) // 变量是整数
                {
                    value = Convert.ToInt64(varValue);
                    return true;
                }
                else if (varValue is bool) // 变量是布尔值
                {
                    value = Convert.ToBoolean(varValue);
                    return true;
                }
                else if (varValue is string) // 变量是字符串
                {
                    value = Convert.ToString(varValue);
                    return true;
                }
            }

            value = null;
            return false; // 未定义变量，返回false
        }
        
        /// <summary> 
        /// Shunting Yard 算法，转换中缀表达式为后缀表达式 <br/>
        /// Shunting Yard algorithm, converts infix expressions to postfix expressions. 
        /// </summary>
        /// <param name="input">
        /// 中缀表达式 <br/>
        /// Infix expression
        /// </param>
        /// <remarks>
        /// 直接将返回的值传入<see cref="EvaluatePostfix(List{string})"/>方法 <br/>
        /// Directly pass the returned value to the <see cref="EvaluatePostfix(List{string})"/> method.
        /// </remarks>
        /// <returns>
        /// 转换后的后缀表达式，或错误标识：<br/>
        /// The converted postfix expression, or error indicators: 
        /// <list type="bullet">
        /// <item>"InvalidExpression" -表示输入的表达式无效。/ Indicates that the input expression is invalid.</item>
        /// <item>"UnknownOperator" - 表示表达式中包含未知运算符。/ Indicates that the expression contains an unknown operator.</item>
        /// <item>"UndefinedVariable" - 表示表达式中使用了未定义的变量。/ Indicates that the expression uses an undefined variable. </item>
        /// <item>"InvalidVariableType" - 表示表达式中使用了不合法的变量类型。/ Indicates that an invalid variable type is used in the expression. </item>
        /// </list>
        ///  
        /// </returns>
        public static List<string> InfixToPostfix(string input)
        {
            // 去除空格
            input = Regex.Replace(input, @"\s+", "");
            // 输出结果列表
            var output = new List<string>();
            // 操作符栈
            var operators = new Stack<string>();
            // 操作符优先级字典
            var precedence = new Dictionary<string, int>
            {
                { "+", 1 },
                { "-", 1 },
                { "%", 2 },
                { "*", 2 },
                { "/", 2 },
            };
            // 使用正则表达式匹配输入的中缀表达式中的所有成分
            foreach (Match match in Regex.Matches(input, @"(?<=^|[(]|[+\-*/%])-\d+(\.\d+)?|(?<=^|[(]|[+\-*/%])-\p{L}[\p{L}\p{N}_]*|\d+(\.\d+)?|[+\-*/%()]|[\p{L}][\p{L}\p{N}_]*").Cast<Match>())
            {               

                string token = match.Value; // 提取匹配的字符串
                if (double.TryParse(token, out _)) // 如果是数字
                {
                    output.Add(token); // 添加到输出列表
                }
                else if (token == "-") // 处理负号
                {
                    // 检查前一个 token
                    if (output.Count == 0 || output.Last() == "(" || precedence.ContainsKey(output.Last()))
                    {
                        // 负号后填充0
                        output.Add("0"); // 添加0作为左操作数
                        operators.Push(token); // 将负号入栈
                    }
                    else
                    {
                        // 检查操作数的数量
                        if (output.Count < 1)
                        {
                            // 如果没有有效操作数，则返回错误
                            ScriptConsole.PrintErr("Invalid expression: unexpected operator. (意外的操作符)");
                            return new List<string> { "InvalidExpression" }; // 返回错误标识
                        }

                        // 将栈中优先级不低于当前运算符的运算符弹出并添加到输出
                        while (operators.Count > 0 && precedence.ContainsKey(operators.Peek()) && precedence[operators.Peek()] >= precedence[token])
                        {
                            output.Add(operators.Pop());
                        }
                        operators.Push(token); // 当前运算符入栈
                    }
                }
                else if (token.StartsWith("-") && VariableList.ContainsKey(token[1..].Trim())) // 处理负变量
                {
                    // 将变量值取出并转为负数
                    if (VariableList[token[1..].Trim()] is double variableValue)
                    {
                        output.Add((-variableValue).ToString()); // 将负变量值加入输出
                    }
                    else if (VariableList[token[1..].Trim()] is long intValue)
                    {
                        output.Add((-intValue).ToString()); // 将负变量值加入输出
                    }
                    else
                    {
                        ScriptConsole.PrintErr($"Invalid variable type for: {token[1..]} (变量类型无效: {token[1..]})");
                        return new List<string> { "InvalidVariableType" }; // 返回错误标识
                    }
                }
                else if (precedence.ContainsKey(token)) // 如果是运算符
                {
                    // 检查操作数的数量
                    if (output.Count < 1)
                    {
                        // 如果没有有效操作数，则返回错误
                        ScriptConsole.PrintErr("Invalid expression: unexpected operator. (意外的操作符)");
                        return new List<string> { "InvalidExpression" }; // 返回错误标识
                    }
                    // 将栈中优先级不低于当前运算符的运算符弹出并添加到输出
                    while (operators.Count > 0 && precedence.ContainsKey(operators.Peek()) && precedence[operators.Peek()] >= precedence[token])
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token); // 当前运算符入栈
                }
                else if (token == "(") // 左括号
                {
                    operators.Push(token); // 入栈
                }
                else if (token == ")") // 右括号
                {
                    // 弹出所有运算符直到遇到左括号
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    if (operators.Count == 0) // 如果没有找到左括号
                    {
                        ScriptConsole.PrintErr("Invalid expression: mismatched parentheses. (括号不匹配)");
                        return new List<string> { "InvalidExpression" }; // 返回错误标识
                    }
                    operators.Pop(); // 弹出左括号
                }
                else // 处理变量
                {
                    if (VariableList.ContainsKey(token))
                    {
                        var value = VariableList[token];
                        if (value is double || value is long) // 检查是否为数值类型
                        {
                            output.Add(value.ToString()); // 将变量值加入输出
                        }
                        else
                        {
                            ScriptConsole.PrintErr($"Invalid variable type for: {token} (变量类型无效: {token})");
                            return new List<string> { "InvalidVariableType" }; // 返回错误标识
                        }
                    }
                    else
                    {
                        ScriptConsole.PrintErr($"Undefined variable: {token} (未定义的变量: {token})");
                        return new List<string> { "UndefinedVariable" }; // 返回错误标识
                    }
                }

            }
            // 弹出剩余的运算符
            while (operators.Count > 0)
            {
                string op = operators.Pop();
                if (precedence.ContainsKey(op))
                {
                    output.Add(op); // 只添加有效的运算符
                }
                else
                {
                    ScriptConsole.PrintErr($"Unknown operator: {op} (未知运算符: {op} )");    
                    return new List<string> {"UnknownOperator"};// 返回错误标识
                }
            }

            // 验证表达式的有效性
            if (output.Count == 0 || operators.Count > 0)
            {
                ScriptConsole.PrintErr("Invalid expression: incomplete or malformed. (不完整的表达式)");
                return new List<string> {"InvalidExpression"};// 返回错误标识
            }

            return output; // 返回后缀表达式
        }

        /// <summary> 
        /// 计算后缀表达式的值 <br/>
        /// Calculate the value of the postfix expression.
        /// </summary>
        /// <param name="postfix">
        /// 将中缀表达式由 <see cref="InfixToPostfix(string)"/> 方法转换后得到的后缀表达式。<br/> 
        /// The postfix expression obtained by converting the infix expression using the <see cref="InfixToPostfix(string)"/> method.
        /// 该表达式应包含有效的运算符和操作数。<br/> 
        /// This expression should contain valid operators and operands.
        /// </param>
        /// <returns>
        /// 计算得到的结果字符串，或者返回错误标识：<br/>
        /// The calculated result string, or an error indicator:
        /// <list type="bullet">
        /// <item>"InvalidExpression" - 表示表达式无效。 / Indicates that the expression is invalid.</item>
        /// <item>"UnknownOperator" - 表示表达式中包含未知运算符。 / Indicates that the expression contains an unknown operator.</item>
        /// <item>"UndefinedVariable" - 表示表达式中使用了未定义的变量。 / Indicates that the expression uses an undefined variable.</item>
        /// <item>"InvalidVariableType" - 表示表达式中使用了不合法的变量类型。 / Indicates that an invalid variable type is used in the expression.</item>
        /// <item>"DivisionInvalid" - 表示尝试进行非法的除法运算（如除以零）。 / Indicates an attempt to perform an illegal division operation (e.g., division by zero).</item>
        /// </list>
        /// </returns>
       public static string EvaluatePostfix(List<string> postfix)
        {
            if (postfix.Count == 1 && postfix[0] == "InvalidExpression")
            {
                return "InvalidExpression";// 返回错误标识
            }
            if (postfix.Count == 1 && postfix[0] == "UnknownOperator")
            {
                return "UnknownOperator";// 返回错误标识
            }            
            if (postfix.Count == 1 && postfix[0] == "UndefinedVariable")
            {
                return "UndefinedVariable";// 返回错误标识
            }            
            if (postfix.Count == 1 && postfix[0] == "InvalidVariableType")
            {
                return "InvalidVariableType";// 返回错误标识
            }

            // 操作数栈
            var stack = new Stack<double>();

            // 遍历后缀表达式的每个元素
            foreach (string token in postfix)
            {
                if (double.TryParse(token, out double number)) // 如果是数字
                {
                    stack.Push(number); // 压入栈中
                }
                else // 否则，必须是运算符
                {
                    // 确保栈中至少有两个操作数
                    if (stack.Count < 2)
                    {
                        ScriptConsole.PrintErr("Invalid expression: not enough operands. (操作数不足)");
                        return "InvalidExpression"; // 返回错误标识
                    }

                    double rightOperand = stack.Pop(); // 右操作数
                    double leftOperand = stack.Pop();  // 左操作数

                    // 根据运算符进行相应的计算
                    switch (token)
                    {
                        case "+":
                            stack.Push(leftOperand + rightOperand); // 加法
                            break;
                        case "-":
                            stack.Push(leftOperand - rightOperand); // 减法
                            break;
                        case "*":
                            stack.Push(leftOperand * rightOperand); // 乘法
                            break;
                        case "/":
                            if (rightOperand == 0) // 检查除数是否为零
                            {
                                ScriptConsole.PrintErr("Division by zero is not allowed. (除数不能为0)");
                                return "DivisionInvalid"; // 返回错误标识
                            }
                            stack.Push(leftOperand / rightOperand); // 除法
                            break;
                        case "%":
                            stack.Push(leftOperand % rightOperand); // 取余
                            break;
                        default:
                            ScriptConsole.PrintErr($"Unknown operator: {token} (未知运算符: {token})"); // 未知运算符
                            return "UnknownOperator"; // 返回错误标识
                    }
                }
            }
            // 返回栈中最后的结果
            return stack.Pop().ToString();
        }        
    }
}