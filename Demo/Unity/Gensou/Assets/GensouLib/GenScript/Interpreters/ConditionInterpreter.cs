using System;
using System.Collections.Generic;
using System.Linq;
using static GensouLib.GenScript.Interpreters.VariableInterpreter;

namespace GensouLib.GenScript.Interpreters
{
    /// <summary> 
    /// 条件命令解释器 
    /// </summary>
    public class ConditionInterpreter : BaseInterpreter
    {
        /// <summary> 
        /// 处理条件命令
        /// </summary>
        /// <param name="condition"> 
        /// 条件表达式，表示需要评估的逻辑条件。
        /// </param>
        /// <returns> 
        /// 条件的结果，返回布尔值指示条件是否满足。
        /// </returns>
        public static bool CheckCondition(string condition)
        {
            // 检查条件是否为空
            if (string.IsNullOrWhiteSpace(condition))
            {
                ScriptConsole.PrintErr("The condition expression is empty. Command ignored.(条件表达式为空，命令已忽略)");
                return false;
            }
            // 定义条件运算符
            List<string> conditionOperator = new() {">=", "<=", "==", "!=", ">", "<"};
            // 查找运算符
            string getedOperator = FindOperator(condition, conditionOperator);
            // 切割条件表达式
            var expression = condition.Split(getedOperator, StringSplitOptions.RemoveEmptyEntries);
            // 检查表达式的合法性
            if (!IsValidExpression(expression, getedOperator, conditionOperator))
            {
                ScriptConsole.PrintErr("The condition expression is invalid. Command ignored. (条件表达式不合法，命令已忽略)");
                return false;
            }
            // 如果匹配到条件运算符
            if (expression.Length > 0 && !string.IsNullOrEmpty(getedOperator))
            {
                return ProcessCheckCondition(expression, getedOperator);
            }
            // 处理简单条件
            return EvaluateSimpleCondition(condition);
        }

#region Check condition expression
        // 遍历运算符列表查找运算符
        private static string FindOperator(string condition, List<string> operators)
        {
            foreach (string op in operators)
            {
                if (condition.Contains(op))
                {
                    return op;
                }
            }
            return string.Empty; // 找不到运算符，返回空字符串
        }

        // 验证条件表达式的合法性。
        // 当指定了运算符时，对表达式的格式和内容进行进一步检查，判断其是否符合合法的条件格式。
        private static bool IsValidExpression(string[] expression, string getedOperator, List<string> operators)
        {
            // 若满足以下任一条件则视为无效表达式：
            if (!string.IsNullOrEmpty(getedOperator) && // 运算符不为空的情况下才进行检查
                (expression.Length != 2 || // 表达式长度不等于2（必须包含左右两个操作数）
                expression.Any(e => string.IsNullOrWhiteSpace(e)) ||  // 表达式中存在空白元素（为空或仅包含空格）
                expression[0].Contains('=') || expression[1].Contains('=') || // 左右操作数中不应包含等号
                expression.Any(e => e.StartsWith(getedOperator) || e.EndsWith(getedOperator)) || // 操作数不应以运算符开头或结尾
                expression.Any(e => operators.Any(op => e.Contains(op))))) // 操作数中不应包含其他运算符
            {
                return false;
            }
            // 有效表达式，返回true
            return true;
        }

        // 判断给定的左右操作数是否应视为数值比较。
        // 若两个操作数均为数值或表达式，或一方为数值/变量且另一方为表达式，则视为数值比较。
        private static bool IsNumericComparison(string left, string right)
        {
            return (TryParseNumeric(left, out _) && TryParseNumeric(right, out _)) || // 左右都是数字或
              (CheckExpression(left) && CheckExpression(right)) || // 左右都是表达式或
              (CheckExpression(left) && (CheckVariableName(right) || TryParseNumeric(right, out _))) || // 左是表达式，右是数字或变量或
              ((CheckVariableName(left) || TryParseNumeric(left, out _)) && CheckExpression(right));  // 左是数字或变量，右是表达式              
        }
#endregion

#region Start compare 
        // 处理条件检查，通过比较给定表达式中的两个值（数值或变量）来确定条件是否满足。
        // 如果两个值均为数值，则解析并计算它们的数值，再根据指定的操作符进行比较；
        // 否则，将它们作为变量直接进行比较。
        private static bool ProcessCheckCondition(string[] expression, string op)
        {
            var left = expression[0].Trim(); // 左操作数
            var right = expression[1].Trim(); // 右操作数
            // 如果左右操作数均为数学表达式或数值以及数值类型变量，执行数值比较
            if (IsNumericComparison(left, right))
            {
                // 解析左右操作数（可以为表达式或数值以及数值类型变量）
                if (EvaluateNumbers(left, right, out object leftValueToCompare, out object rightValueToCompare))
                {
                    return CompareValues(leftValueToCompare, rightValueToCompare, op); // 比较数值
                }
                return false; // 无法解析为数值时，返回false
            }
            // 如果左右操作数为变量，执行变量比较
            return CompareVariables(left, right, op);
        }
#endregion

#region Check and compare operand        
        // 比较两个变量的值，根据左右操作数的类型（布尔、字符串、数值）执行不同的比较逻辑。
        // 如果左右操作数都是有效变量：
        //   - 若类型相同，则根据类型（布尔、字符串或数值）执行相应的比较并返回结果；
        //   - 若类型不兼容，调用错误处理方法并返回 false。
        // 如果其中一个或两个操作数不是有效变量，则按字符串或布尔值进行直接比较。
        private static bool CompareVariables(string left, string right, string op)
        {
            // 左右操作数都是有效变量
            if (TryGetVariableValue(left, out object leftValue) && TryGetVariableValue(right, out object rightValue))
            {
                if (leftValue is bool boolLeft && rightValue is bool boolRight) // 布尔值比较
                {
                    return CompareBooleans(boolLeft, boolRight, op);
                }

                if (leftValue is string strLeft && rightValue is string strRight) // 字符串比较
                {
                    return CompareStrings(strLeft, strRight, op);
                }

                if (leftValue is double or long && rightValue is double or long) // 数值比较
                {
                    return CompareValues(leftValue, rightValue, op); 
                }
                // 类型不兼容，调用错误处理并返回 false
                HandleIncompatibleComparison(left, right, leftValue, rightValue);
                return false;
            }

            // 左右操作数包含非变量值处理
            return EvaluateNoVariable(left, right, op);
        }

        // 对非变量的左右操作数进行比较。若左右操作数类型相同（字符串、布尔值或数值），则执行对应的比较；
        // 如果左右操作数不符合这些条件，则将其视为包含变量的情况进一步处理。
        private static bool EvaluateNoVariable(string left, string right, string op)
        {
            // 左右是字符串
            if (left.StartsWith("\"") && left.EndsWith("\"") && right.StartsWith("\"") && right.EndsWith("\""))
            {   
                return CompareStrings(left, right, op);
            }
            // 左右都是布尔值
            if (bool.TryParse(left, out bool boolValueLeft) && bool.TryParse(right, out bool boolValueRight))
            {
                return CompareBooleans(boolValueLeft, boolValueRight, op);
            }
            // 左右都是数字
            if (TryParseNumeric(left, out object numberValueLeft) && TryParseNumeric(right, out object numberValueRight))
            {
                return CompareValues(numberValueLeft, numberValueRight, op);
            }
            // 左或右是变量
            return OnlyOneSideIsVariable(left, right, op);
        }

        // 处理简单的布尔条件，支持布尔值、布尔类型变量以及字符串的布尔解析。
        // 该函数首先检查传入的条件是否为已定义的布尔类型变量，
        // 然后尝试将字符串解析为布尔值，最后处理语法错误和无效的条件。
        private static bool EvaluateSimpleCondition(string condition)
        {
            // 如果条件是已定义的变量，则检查其是否为布尔类型
            if (VariableList.ContainsKey(condition)) 
            {
                if (VariableList[condition] is bool result)
                {
                    return result;
                }
                
                ScriptConsole.PrintErr("Variable type mismatch: expected Boolean. Command ignored.（变量类型错误，命令已忽略）");
            }
            // 尝试将字符串解析为布尔值（true/false）
            else if (bool.TryParse(condition, out bool cond)) 
            {
                return cond;
            }
            // 如果变量名存在语法上有效但未定义
            if (!VariableList.ContainsKey(condition) && CheckVariableName(condition)) 
            {
                ScriptConsole.PrintErr($"Undefined variable: {condition}.（未定义的变量：{condition}）");
            }
            // 防止使用仅包含数学表达式的条件
            if (CheckExpression(condition)) 
            {
                ScriptConsole.PrintErr("A standalone mathematical expression is not a valid condition.（单独的数学表达式不被视为条件）");
            }
            // 检测并警告意外的赋值操作
            if (condition.Contains('=')) 
            {
                ScriptConsole.PrintErr("Unexpected assignment operation, if you expect an equal comparison, use \"==\".(意外的赋值操作，若期望进行等于比较，使用“==”)");
            }
            // 最后，处理任何其他不符合条件的情况
            ScriptConsole.PrintErr($"Invalid argument: {condition}. Expected: Boolean variable, Boolean value, or conditional expression. Command ignored.（无效的参数：{condition}，期望：布尔类型变量，布尔值或条件表达式。命令已忽略）");
            return false;           
        }
#endregion

#region Compare variables and values
        // 检查是否仅有一侧操作数为变量，根据情况处理单侧为变量的比较
        // 如果左右操作数都符合或其一符合变量名规则但未定义则判定为不存在的变量
        // 最后处理类型不兼容的比较
        private static bool OnlyOneSideIsVariable(string left, string right, string getedOperator)
        {
            // 左侧为已定义变量，右侧为非变量
            if (TryGetVariableValue(left, out object varLeft) && !CheckVariableName(right))
            {
                return CompareVariableWithValue(varLeft, right, left, getedOperator, "L");
            }
            // 右侧为已定义变量，左侧为非变量
            if (!CheckVariableName(left) && TryGetVariableValue(right, out object varRight))
            {
                return CompareVariableWithValue(varRight, left, right, getedOperator, "R");
            }
            // 如果左右操作数都符合或其一符合变量命名规则但未定义，报告不存在的变量
            if (CheckVariableName(left) && CheckVariableName(right) && !TryGetVariableValue(left, out _) && !TryGetVariableValue(right, out _))
            {
                ScriptConsole.PrintErr($"Undefined variables: {left} and {right}. Command ignored.（未定义的变量：{left} 和 {right}，命令已忽略）");
                return false;
            } 
            else if(CheckVariableName(left) && !TryGetVariableValue(left, out _))
            {
                ScriptConsole.PrintErr($"Undefined variables: {left}. Command ignored.（未定义的变量：{left} ，命令已忽略）");
                return false;                
            }
            else if (CheckVariableName(right) && !TryGetVariableValue(right, out _))
            {
                ScriptConsole.PrintErr($"Undefined variables: {right}. Command ignored.（未定义的变量：{right} ，命令已忽略）");
                return false;                        
            }
            // 最后，处理类型不兼容的比较
            HandleIncompatibleComparison(left, right);
            return false;
        }

        // 变量与不同类型的值的比较
        // 根据不同的类型进入不同的分支
        // 布尔、字符串和数值类型以外的类型直接返回false
        private static bool CompareVariableWithValue(object variable, string value, string variableName, string op, string side)
        {
            return variable switch
            {
                bool boolValue => CompareBooleanWithValue(boolValue, value, variableName, op, side),
                string stringValue => CompareStringWithValue(stringValue, value, variableName, op, side),
                object numberValue => CompareNumberWithValue(numberValue, value, variableName, op, side),
                _ => false,
            };
        }

        // 比较一个布尔变量与一个非变量操作数的值
        // 如果非变量操作数不能被解析为布尔值，则根据变量操作数的位置（左侧或右侧）报告不兼容的比较
        private static bool CompareBooleanWithValue(bool boolValue, string value, string variableName, string op, string side)
        {
            // 尝试将非变量操作数解析为布尔值
            if (bool.TryParse(value, out bool parsedValue))
            {
                return CompareBooleans(boolValue, parsedValue, op);
            }
            // 如果非变量操作数无法解析为布尔值，根据变量的位置处理类型不兼容的比较
            if (side == "L") // 左侧为变量时类型不兼容
            {
                HandleIncompatibleComparison(variableName, value, leftValue: boolValue);
            }
            else // 右侧为变量时类型不兼容
            {
                HandleIncompatibleComparison(variableName, value, rightValue: boolValue);
            }
            // 类型不兼容，返回false
            return false;
        }

        // 比较一个字符串变量与一个非变量操作数的值
        // 如果非变量操作数不被双引号包裹，则根据变量操作数的位置（左侧或右侧）报告不兼容的比较
        private static bool CompareStringWithValue(string stringValue, string value, string variableName, string op, string side)
        {
            // 判定非变量操作数是否被双引号包裹
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return CompareStrings(stringValue, value.Trim('"'), op); // 去除首尾的双引号
            }
            // 如果非变量操作数不被双引号包裹，根据变量的位置处理类型不兼容的比较
            if (side == "L") // 左侧为变量
            {
                HandleIncompatibleComparison(variableName, value, leftValue: stringValue);
            }
            else // 右侧为变量
            {
                HandleIncompatibleComparison(variableName, value, rightValue: stringValue);
            }
            // 类型不兼容，返回false
            return false;

        }

        // 比较一个数值类型变量与一个非变量操作数的值
        // 如果非变量操作数不能被解析为数值，则根据变量操作数位置（左侧或右侧）报告不兼容的比较
        private static bool CompareNumberWithValue(object numberValue, string value, string variableName, string op, string side)
        {
            // 尝试将非变量操作数解析为数值
            if (TryParseNumeric(value, out object parsedValue))
            {
                return CompareValues(numberValue, parsedValue, op);
            }
            // 如果非变量操作数无法解析为数值，根据变量的位置处理类型不兼容的比较
            if (side == "L") // 左侧为变量
            {
                HandleIncompatibleComparison(variableName, value, leftValue: numberValue);
            }
            else // 右侧为变量
            {
                HandleIncompatibleComparison(variableName, value, rightValue: numberValue);
            }
            // 类型不兼容，返回false
            return false;
        }
#endregion

#region Error Handling
        // 左右操作数进行比较时类型不兼容
        // 根据操作数的类型以及是否为变量输出对应的错误信息
        private static void HandleIncompatibleComparison(string left, string right, object leftValue = null, object rightValue = null)
        {
            // 左右操作数符合变量名规则但未定义
            if (!TryGetVariableValue(left, out _) && !TryGetVariableValue(right, out _) && CheckVariableName(left) && CheckVariableName(right))
            {
                ScriptConsole.PrintErr($"Variables {left} and {right} do not exist. Command ignored.(变量 {left} 和 {right} 不存在，命令已忽略)");
                return;
            }
            // 左右操作数是布尔间数值的比较
            if ((bool.TryParse(left, out _) && TryParseNumeric(right, out _)) || (TryParseNumeric(left, out _) && bool.TryParse(right, out _)))
            {
                ScriptConsole.PrintErr("Cannot compare Boolean value and numeric value. Command ignored.(不能将布尔值和数值比较，命令已忽略)");
                return;
            }
            // 数值与字符串的比较
            if ((TryParseNumeric(left, out _) && right.StartsWith("\"") && right.EndsWith("\"")) || (TryParseNumeric(right, out _) && left.StartsWith("\"") && left.EndsWith("\"")))
            {
                ScriptConsole.PrintErr("Cannot compare numeric value and string. Command ignored.(不能将数值和字符串比较，命令已忽略)");
                return;
            }
            // 布尔值与字符串的比较
            if ((bool.TryParse(left, out _) && right.StartsWith("\"") && right.EndsWith("\"")) || (bool.TryParse(right, out _) && left.StartsWith("\"") && left.EndsWith("\"")))
            {
                ScriptConsole.PrintErr("Cannot compare Boolean value and string. Command ignored.(不能将布尔值和字符串比较，命令已忽略)");
                return;
            }
            // 左侧是布尔变量，右侧是字符串变量
            if (leftValue is bool && rightValue is string)
            {   
                ScriptConsole.PrintErr($"Cannot compare string type variable: {right} with a Boolean type variable: {left}. Command ignored.(不能将字符串类型变量：{right} 用以和布尔类型变量：{left} 比较，命令已忽略)");
                return;
            }
            // 左侧字符串变量，右侧是布尔变量
            if (leftValue is string && rightValue is bool)
            {
                ScriptConsole.PrintErr($"Cannot compare string type variable: {left} with a Boolean type variable: {right}. Command ignored.(不能将字符串类型变量：{left} 用以和布尔类型变量：{right} 比较，命令已忽略)");
                return;
            }
            // 左侧是布尔变量，右侧是其他类型的非变量操作数
            if (leftValue is bool)
            {
                HandleBooleanIncompatibleComparison(left, right);
                return;
            }
            // 左侧是数值类型变量，右侧是其他类型的非变量操作数
            if (leftValue is double or long)
            {
                HandleNumericIncompatibleComparison(left, right);
                return;
            }
            // 左侧是字符串变量，右侧是其他类型的非变量操作数
            if (leftValue is string)
            {
                HandleStringIncompatibleComparison(left, right);
                return;
            }
            // 右侧是布尔变量，左侧是其他类型的非变量操作数
            if (rightValue is bool)
            {
                HandleBooleanIncompatibleComparison(right, left);
                return;
            }
            // 右侧是数值类型变量，左侧是其他类型的非变量操作数
            if (rightValue is double or long)
            {
                HandleNumericIncompatibleComparison(right, left);
                return;
            }
            // 右侧是字符串变量，左侧是其他类型的非变量操作数
            if (rightValue is string)
            {
                HandleStringIncompatibleComparison(right, left);
                return;
            }
            // 其他的意外情况
            ScriptConsole.PrintErr($"Unexpected operand in conditional expression {left} and {right}, expected: Numbers and mathematical expressions, values of consistent types, number variables and mathematical expressions. Command ignored.(条件表达式意外的操作数：{left} 和 {right} ，期望：数值和数学表达式，类型一致的值，数值类型变量和数学表达式。命令已忽略)");
            return;
        }
        
        // 布尔值变量与其他类型的非变量操作数的不兼容比较
        private static void HandleBooleanIncompatibleComparison(string variable, string other)
        {
            if (TryParseNumeric(other, out _)) // 布尔变量与数值比较
            {
                 ScriptConsole.PrintErr($"Cannot compare Boolean type variable: {variable} with a numeric value. Command ignored.(不能将布尔类型变量：{variable} 用以和数值比较，命令已忽略)");           
            }
            else if (!bool.TryParse(other, out _)) // 布尔变量与字符串比较（无法被解析为布尔值则视作字符串）
            {
                if (other.StartsWith("\"") && other.EndsWith("\""))
                {
                    ScriptConsole.PrintErr($"Cannot compare Boolean type variable: {variable} with a string. Command ignored.(不能将布尔类型变量：{variable} 用以和字符串比较，命令已忽略)");        
                }
                else
                {
                    ScriptConsole.PrintErr($"When doing comparison, you need to use \"\" to indicate the string to avoid confusion with the variable. However, you cannot compare Boolean type variable: {variable} with a string. Command ignored.(进行比较时需要使用\"\"指明字符串，以免和变量混淆，但是不能将布尔类型变量：{variable} 用以和字符串比较，命令已忽略)");            
                }
            }
            // 指明意外的操作数
            ScriptConsole.PrintErr($"Unexpected operand: {other}.(意外的操作数：{other} )");
        }
       
        // 数值类型变量与其他类型非变量操作数的不兼容比较
        private static void HandleNumericIncompatibleComparison(string variable, string other)
        {
            if (bool.TryParse(other, out _)) // 数值类型变量与布尔值比较
            {
                ScriptConsole.PrintErr($"Cannot compare numeric value variable: {variable} with a Boolean value. Command ignored.(不能将数值类型变量：{variable} 用以和布尔值比较，命令已忽略)");  
            }
            else if (!TryParseNumeric(other, out _)) // 数值类型变量与字符串比较（无法被解析为数值则视作字符串）
            {
                if (other.StartsWith("\"") && other.EndsWith("\""))
                {
                    ScriptConsole.PrintErr($"Cannot compare numeric value variable: {variable} with a string. Command ignored.(不能将数值类型变量：{variable} 用以和字符串比较，命令已忽略)");     
                }
                else
                {
                    ScriptConsole.PrintErr($"When doing comparison, you need to use \"\" to indicate the string to avoid confusion with the variable. However, you cannot compare numeric value variable: {variable} with a string. Command ignored.(进行比较时需要使用\"\"指明字符串，以免和变量混淆，但是不能将数值类型变量：{variable} 用以和字符串比较，命令已忽略)");            
                }
            }
            // 指明意外的操作数
            ScriptConsole.PrintErr($"Unexpected operand: {other}.(意外的操作数：{other} )");
        }
        
        // 字符串变量与其他类型操作数的不兼容比较
        private static void HandleStringIncompatibleComparison(string variable, string other)
        {
            if (bool.TryParse(other, out _)) // 字符串变量与布尔值比较
            {
                ScriptConsole.PrintErr($"Cannot compare string type variable: {variable} with a Boolean value. Command ignored.(不能将字符串类型变量：{variable} 用以和布尔值比较，命令已忽略)");
            }
            else if (TryParseNumeric(other, out _)) // 字符串变量与数值比较
            {
                ScriptConsole.PrintErr($"Cannot compare string type variable: {variable} with a numeric value. Command ignored.(不能将字符串类型变量：{variable} 用以和数值比较，命令已忽略)");            
            }
            else // 未使用双引号的字符串比较
            {
                ScriptConsole.PrintErr($"When doing comparison, you need to use \"\" to indicate the string to avoid confusion with the variable. Command ignored.(进行比较时需要使用\"\"指明字符串，以免和变量混淆，命令已忽略)");            
            }
            // 指明意外的操作数
            ScriptConsole.PrintErr($"Unexpected operand: {other}.(意外的操作数：{other} )");
        }

#endregion

#region Expression Parsing
        // 处理数值比较
        // 尝试将左右操作数解析为数值。如果解析成功，返回true以及解析后的值；
        // 否则返回false。
        private static bool EvaluateNumbers(string left, string right, out object leftValueToCompare, out object rightValueToCompare)
        {
            // 解析左表达式
            if (TryGetExpressionValue(left, out object leftValue))
            {
                // 解析右表达式
                if (TryGetExpressionValue(right, out object rightValue))
                {
                    leftValueToCompare = leftValue;
                    rightValueToCompare = rightValue;
                    return true;
                }
            }
            
            leftValueToCompare = null;
            rightValueToCompare = null;
            return false;
        }

        // 尝试解析表达式
        // 如果是合法数学表达式则尝试解析它，如果解析成功，返回true以及解析后的值，解析失败则输出错误信息并返回false
        // 否则，如果是已定义变量且是数值则返回true以及变量值
        // 否则，如果是数字则返回true以及该数字
        // 最后根据传入的表达式输出对应错误提示并返回false
        private static bool TryGetExpressionValue(string expression, out object value)
        {
            value = null;

            bool varIsNumber = true; // 标记变量是否是数值
            // 如果是数学表达式
            if (!string.IsNullOrWhiteSpace(expression) && CheckExpression(expression))
            {
                // 解析表达式
                var postfix = InfixToPostfix(expression);
                var result = EvaluatePostfix(postfix);
                if (TryParseNumeric(result, out object parsedValue))
                {
                    value = parsedValue;
                    return true;
                }

                ScriptConsole.PrintErr($"Unable to parse the expression: {expression}. Command ignored.(无法解析的表达式: {expression}，命令已忽略)");
                return false;
            }
            else if (TryGetVariableValue(expression, out object varValue))// 如果是已定义变量
            {
                if (varValue is double or long)
                {
                    value = varValue;
                    return true;
                }
                varIsNumber = false; // 如果不是数值类型变量则标记它
            }
            else if (TryParseNumeric(expression, out object number)) // 如果是数字
            {
                value = number;
                return true;
            }
            // 如果操作数是已定义变量但不是数值类型变量
            if (!varIsNumber)
            {
                ScriptConsole.PrintErr($"Only numeric value variables can be compared with mathematical expressions. Command ignored.(只能使用数值类型变量与数学表达式进行比较，命令已忽略)");
                return false;
            }
            // 否则将操作数视作未定义数值类型变量
            ScriptConsole.PrintErr($"Numeric value variable: {expression} does not exist. Command ignored.(数值类型变量：{expression} 不存在，命令已忽略)");        
            return false;
        }
#endregion

#region Compare values
        // 字符串间的比较
        private static bool CompareStrings(string leftValue, string rightValue, string comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case "==":
                    return leftValue == rightValue;
                case "!=":
                    return leftValue != rightValue;
                default:
                    ScriptConsole.PrintErr($"Operator: \"{comparisonOperator}\" is not supported for string types. Only '==' and '!=' are valid. Command ignored.(运算符：\"{comparisonOperator}\" 不支持字符串类型，只有 '==' 和 '!=' 是有效的，命令已忽略)");
                    return false;            
            }
        }

        // 布尔值间的比较
        private static bool CompareBooleans(bool leftValue, bool rightValue, string comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case "==":
                    return leftValue == rightValue;
                case "!=":
                    return leftValue != rightValue;
                default:
                    ScriptConsole.PrintErr($"Operator: \"{comparisonOperator}\" is not supported for Boolean types. Only '==' and '!=' are valid. Command ignored.(运算符：\"{comparisonOperator}\" 不支持布尔类型，只有 '==' 和 '!=' 是有效的，命令已忽略)");
                    return false;            
            }
        }

        // 数值间的比较
        private static bool CompareValues(object leftValue, object rightValue, string comparisonOperator)
        {
            if (leftValue is long leftLong && rightValue is long rightLong)
            {
                return comparisonOperator switch
                {
                    ">" => leftLong > rightLong,
                    "<" => leftLong < rightLong,
                    ">=" => leftLong >= rightLong,
                    "<=" => leftLong <= rightLong,
                    "==" => leftLong == rightLong,
                    "!=" => leftLong != rightLong,
                    _ => false
                };
            }
            else if (leftValue is double leftDouble && rightValue is double rightDouble)
            {
                return comparisonOperator switch
                {
                    ">" => leftDouble > rightDouble,
                    "<" => leftDouble < rightDouble,
                    ">=" => leftDouble >= rightDouble,
                    "<=" => leftDouble <= rightDouble,
                    "==" => leftDouble == rightDouble,
                    "!=" => leftDouble != rightDouble,
                    _ => false
                };
            }
            else if (leftValue is long longValue && rightValue is double doubleValue)
            {
                return comparisonOperator switch
                {
                    ">" => longValue > doubleValue,
                    "<" => longValue < doubleValue,
                    ">=" => longValue >= doubleValue,
                    "<=" => longValue <= doubleValue,
                    "==" => longValue == doubleValue,
                    "!=" => longValue != doubleValue,
                    _ => false
                };
            }
            else if (leftValue is double doubleValue2 && rightValue is long longValue2)
            {
                return comparisonOperator switch
                {
                    ">" => doubleValue2 > longValue2,
                    "<" => doubleValue2 < longValue2,
                    ">=" => doubleValue2 >= longValue2,
                    "<=" => doubleValue2 <= longValue2,
                    "==" => doubleValue2 == longValue2,
                    "!=" => doubleValue2 != longValue2,
                    _ => false
                };
            }
            else
            {
                return false;
            }
        }
#endregion
    }
}