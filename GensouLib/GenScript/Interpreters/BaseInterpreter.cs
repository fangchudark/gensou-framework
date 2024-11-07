using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 脚本解释器基类<br/>
    /// Script interpreter base class 
    /// </summary>
    /// <remarks> 
    /// 负责解析脚本并执行命令。<br/>
    /// Responsible for parsing scripts and executing commands.<br/>
    /// 提供基础的命令处理：变量命令和控制台命令。<br/>
    /// Provides basic command handling for variable commands and console commands.<br/>
    /// 可供其他类继承和扩展以实现更多功能。<br/>
    /// This class can be inherited and extended by other classes for additional functionality. 
    /// </remarks>
    public class BaseInterpreter
    {
        /// <summary>
        /// 不可用作变量名的保留关键字列表<br/>
        /// List of reserved keywords that cannot be used as variable names. 
        /// </summary>
        public static List<string> KeyWords = new() {"when"};

        /// <summary> 
        /// 变量字典，键为变量名，值为变量值<br/>
        /// Variable dictionary, where the key is the variable name and the value is the variable value.  
        /// </summary>
        public static Dictionary<string, object> VariableList { get; set; } = new(); // 存储变量名及其对应的值

        // 多行日志计数器
        private static int multiLineLogLineConut = 0; // 用于跟踪当前多行日志的剩余行数

        /// <summary> 
        /// 解析脚本 <br/> 
        /// Parses the script 
        /// </summary>
        /// <param name="scriptContent"> 
        /// 读取到的脚本内容<br/> 
        /// The content of the script that has been read 
        /// </param>
        public static void ParseScript(string scriptContent)
        {
            // 按行以换行符和回车分割
            string[] lines = scriptContent.Split(new[] {'\n', '\r'},StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // 去掉首尾空格以规范化行内容
                string trimmedLine = line.Trim();
                
                // 跳过空行，避免处理无效输入
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;
                // 如果还有未输出的多行日志，则直接输出当前行
                if (multiLineLogLineConut > 0)
                {
                    ScriptConsole.PrintLog(ReplacePlaceholders(line)); // 输出日志
                    multiLineLogLineConut--; // 减少剩余行数计数
                    continue; // 继续处理下一行
                }

                int commentIndex = trimmedLine.IndexOf("|:");

                // 如果当前行存在注释，则截断到注释前的内容并执行
                if(commentIndex != -1)
                {
                    string codePart = trimmedLine[..commentIndex]; // 获取注释前的代码部分
                    ScriptExecutor.ExecuteCommand(codePart); // 执行代码部分
                }
                else
                {
                    ScriptExecutor.ExecuteCommand(trimmedLine); // 执行当前行内容
                }
            }
        }

        /// <summary> 
        /// 处理控制台命令<br/>
        /// Processes console commands.  
        /// </summary>
        /// <param name="code"> 
        /// 命令标识符后的代码<br/>
        /// The code following the command identifier.  
        /// </param>
        public static void HandleDebugOutput(string code)
        {
            var consolePrint = code[1..].Trim();

            if (consolePrint.StartsWith("<") && consolePrint.EndsWith(">"))
            {
                // 去掉 "<" 和 ">"，并去除首尾空格
                string innerText = consolePrint[1..^1].Trim();
                
                // 尝试将内层文本转换为整数
                if (int.TryParse(innerText, out int lineCount) && lineCount > 0) 
                {
                    // 检查是否以 "<" 开头，表示多行日志
                    multiLineLogLineConut = lineCount; // 设置多行日志的行数
                }
                return;
            }
            // 输出字符串和变量
            if (CheckExpression(consolePrint)) // 检查是否包含运算符
            {
                // 处理包含运算符的表达式
                var postfixExpression = InfixToPostfix(consolePrint); // 转换中缀表达式为后缀表达式
                var result = EvaluatePostfix(postfixExpression); // 计算后缀表达式的结果

                if (long.TryParse(result, out long longResult))
                {
                    ScriptConsole.PrintLog(longResult); // 输出计算结果
                }
                else if (double.TryParse(result, out double doubleResult))
                {
                    if (!Regex.IsMatch(consolePrint, @"-?\d+\.\d+") && (doubleResult >= long.MaxValue || doubleResult <= long.MinValue))
                    {
                        ScriptConsole.PrintErr("Integer calculation result out of range; you are seeing a floating-point result (if you know what you are doing, you can ignore this warning).(整数计算结果超出范围,你看到的是浮点数结果(如果你知道你在做什么请忽略这条警告))");
                    }
                    ScriptConsole.PrintLog(doubleResult); // 输出计算结果
                }
                else
                {
                    ScriptConsole.PrintErr($"Unable to parse the expression: {consolePrint} (无法解析的表达式: {consolePrint})"); // 原样输出
                }
            }
            else
            {
                // 输出字符串和变量
                ScriptConsole.PrintLog(ReplacePlaceholders(consolePrint)); // 输出字符串
            }
        }
        
        /// <summary> 
        /// 有表达式的变量赋值 <br/>
        /// Variable assignment with an expression. 
        /// </summary>
        /// <param name="varName"> 
        /// 变量名<br/>
        /// The name of the variable.  
        /// </param>
        /// <param name="valueExpression"> 
        /// 赋值表达式<br/>
        /// The expression to assign to the variable.  
        /// </param>
        public static void ProcessExpressionAssignment(string varName, string valueExpression)
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
        /// 处理变量字符串赋值和初始化<br/>
        /// Handles variable string assignment and initialization.  
        /// </summary>
        /// <param name="code"> 
        /// 命令标识符后的代码<br/>
        /// The code following the command identifier.  
        /// </param>
        public static void HandleVariableAssignment(string code)
        {
            // 获取赋值表达式
            string varName = code[..code.IndexOf('=')]; // 变量名
            string valueExpression = code[(code.IndexOf('=') + 1)..];// 赋值表达式

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
            // 赋值处理（如果赋值表达式包含等号则视作是字符串赋值）
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
        
        /// <summary> 
        /// 处理变量赋值  <br/>
        /// Handles variable assignment.
        /// </summary>
        /// <param name="varName"> 
        /// 变量名 <br/>
        /// The name of the variable.
        /// </param>
        /// <param name="value"> 
        /// 值<br/>
        /// The value to assign to the variable. 
        /// </param>
        public static void AssignValue(string varName, string value)
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
            string[] parts = expression.Split(new char[] { '+', '-', '*', '/', '%' }, StringSplitOptions.RemoveEmptyEntries);
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
        /// <param name="numberToDouble">
        /// 可选参数，默认为true。指定是否将数值类型变量转换为<c>double</c>。如果需要针对整数（<c>long</c>）和浮点数（<c>double</c>）做特殊处理，可以将其设置为false，避免将<c>long</c>类型变量转换为<c>double</c>。<br/> 
        /// Optional parameter, default is true. Specifies whether to convert numeric variables to <c>double</c>. If special handling for integers (<c>long</c>) and floating-point numbers (<c>double</c>) is required, set it to false to avoid converting <c>long</c> variables to <c>double</c>.
        /// </param>
        /// <returns>
        /// 如果变量已定义且类型匹配，则返回true，并通过<c>value</c>输出对应的值；否则返回false，并且<c>value</c>为null。<br/>
        /// Returns true if the variable is defined and its type matches, and outputs the corresponding value through <c>value</c>; otherwise, returns false, and <c>value</c> is null.
        /// </returns>
        public static bool TryGetVariableValue(string variable, out object value, bool numberToDouble = true)
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
                    if (numberToDouble)
                    {
                        value = Convert.ToDouble(varValue);
                        return true;
                    }
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
                else if (token.StartsWith("-") && VariableList.ContainsKey(token[1..])) // 处理负变量
                {
                    // 将变量值取出并转为负数
                    // 获取变量值并强制转换为 double
                    if (VariableList[token[1..]] is double variableValue)
                    {
                        output.Add((-variableValue).ToString()); // 将负变量值加入输出
                    }
                    else if (VariableList[token[1..]] is long intValue)
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

        /// <summary> 
        ///  在输入文本中替换占位符为相应的值。<br/>
        /// Replaces placeholders in the input text with their corresponding values.
        /// </summary>
        /// <param name="input"> 
        /// 文本 <br/>
        /// The input text containing placeholders to be replaced. 
        /// </param>
        /// <returns> 
        /// 替换后的文本，或错误标识："UndefinedVariable" <br/>
        /// The modified text with placeholders replaced, or error indicator: "UndefinedVariable". 
        /// </returns>
        public static string ReplacePlaceholders(string input)
        {
            // 定义需要转义的字符及其替换值
            var escapeSequences = new Dictionary<string, string>
            {
                { @"\{", "{" }, // 转义花括号 {
                { @"\}", "}" }, // 转义花括号 }
                { @"\/", "/" },  // 转义斜杠 /
                { @"\+", "+" },  // 转义加号 +
                { @"\-", "-" },  // 转义减号 -
                { @"\*", "*" },  // 转义乘号 *
                { @"\%", "%" },  // 转义取模 %
                { @"\\\", "\\" }, // 转义转义符 \
                { @"\[", "[" }, // 转义方括号 \
                { @"\]", "]" }, // 转义方括号 \
                { @"\""", "\"" }, // 转义引号 "
                { @"\=", "=" }, // 转义等号 =
            };

            // 处理转义符
            foreach (var escape in escapeSequences)
            {
                input = input.Replace(escape.Key, escape.Value);
            }

            // 使用正则表达式提取变量名
            var variableNames = Regex.Matches(input, @"(?<!\\){([\p{L}\p{N}_]+)}")
                                    .Cast<Match>()
                                    .Select(m => m.Groups[1].Value)
                                    .ToList();
            // 创建输出字符串
            var output = new StringBuilder(input);

            foreach (var variableName in variableNames)
            {
                if (!CheckVariableName(variableName))
                {
                    ScriptConsole.PrintErr($"Variable name: {variableName} invalid (变量名: {variableName} 不合法)");
                    return "InvalidVariableName";
                }

                // 检查变量是否存在于 VariableList 中
                if (VariableList.TryGetValue(variableName, out var value))
                {
                    // 替换占位符
                    string placeholder = $"{{{variableName}}}";
                    output.Replace(placeholder, value.ToString());
                }
                else
                {
                    ScriptConsole.PrintErr($"Undefined variablet: {variableName} (未定义的变量: {variableName} )");
                    return "UndefinedVariable"; // 返回错误标识
                }
            }
            return output.ToString();
        }
    }
}