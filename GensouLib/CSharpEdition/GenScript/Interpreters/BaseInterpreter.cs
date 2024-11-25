using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#if GODOT
using Godot;
#endif


namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 脚本解释器基类<br/>
    /// Script interpreter base class 
    /// </summary>
    /// <remarks> 
    /// 负责解析脚本并执行命令。<br/>
    /// Responsible for parsing scripts and executing commands.<br/>
    /// 提供控制台命令处理和字符串解析。<br/>
    /// Provides console command processing and string parsing.<br/>
    /// 可供其他类继承和扩展以实现更多功能。<br/>
    /// This class can be inherited and extended by other classes for additional functionality. 
    /// </remarks>
    public class BaseInterpreter
    {

        // 多行日志计数器
        private static int multiLineLogLineConut = 0; // 用于跟踪当前多行日志的剩余行数


#if GODOT
        /// <summary> 
        /// 解析脚本 <br/> 
        /// Parses the script 
        /// </summary>
        /// <param name="scriptContent"> 
        /// 读取到的脚本内容<br/> 
        /// The content of the script that has been read 
        /// </param>
        /// <param name="node">
        /// 挂载到自动加载的脚本初始化器节点。<br/>
        /// Mount to the autoloaded script initializer node.
        /// </param>
        public static void ParseScript(string scriptContent, Node node)
#elif UNITY_5_3_OR_NEWER
        /// <summary> 
        /// 解析脚本 <br/> 
        /// Parses the script 
        /// </summary>
        /// <param name="scriptContent"> 
        /// 读取到的脚本内容<br/> 
        /// The content of the script that has been read 
        /// </param>
        public static void ParseScript(string scriptContent)
#endif
        {
            // 按行以换行符和回车分割
            string[] lines = scriptContent.Split(new[] {'\n', '\r'},StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

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
                    string codePart = trimmedLine[..commentIndex].Trim(); // 获取注释前的代码部分
#if GODOT
                    ScriptExecutor.ExecuteCommand(codePart, node); // 执行代码部分
#elif UNITY_5_3_OR_NEWER
                    ScriptExecutor.ExecuteCommand(codePart); // 执行代码部分
#endif
                }
                else
                {
#if GODOT
                    ScriptExecutor.ExecuteCommand(trimmedLine, node); // 执行当前行内容
#elif UNITY_5_3_OR_NEWER
                    ScriptExecutor.ExecuteCommand(trimmedLine); // 执行当前行内容
#endif
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
            if (VariableInterpreter.CheckExpression(consolePrint)) // 检查是否包含运算符
            {
                // 处理包含运算符的表达式
                var postfixExpression = VariableInterpreter.InfixToPostfix(consolePrint); // 转换中缀表达式为后缀表达式
                var result = VariableInterpreter.EvaluatePostfix(postfixExpression); // 计算后缀表达式的结果

                if (long.TryParse(result, out long longResult))
                {
                    ScriptConsole.PrintLog(longResult); // 输出计算结果
                }
                else if (double.TryParse(result, out double doubleResult))
                {
                    if (!Regex.IsMatch(consolePrint, @"-?\d+\.\d+") && (doubleResult >= long.MaxValue || doubleResult <= long.MinValue))
                    {
                        ScriptConsole.PrintErr("Integer calculation result out of range; you are seeing a floating-point result.(整数计算结果超出范围,你看到的是浮点数结果)");
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
        /// 替换占位符 <br/>
        /// Replaces placeholders.
        /// </summary>
        /// <param name="input"> 
        /// 文本 <br/>
        /// The input text containing placeholders to be replaced. 
        /// </param>
        /// <returns> 
        /// 返回替换占位符后的字符串。如果有错误（如变量名不合法或未定义），返回相应的错误信息。 <br/>
        /// Returns the string with placeholders replaced. If there is an error (e.g., invalid or undefined variable names), an error message is returned. 
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
                { @"\""", "\"" }, // 转义引号 "
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
                if (!VariableInterpreter.CheckVariableName(variableName))
                {
                    ScriptConsole.PrintErr($"Variable name: {variableName} invalid (变量名: {variableName} 不合法)");
                    return "InvalidVariableName";
                }

                // 检查变量是否存在于 VariableList 中
                if (VariableInterpreter.VariableList.TryGetValue(variableName, out var value))
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
        /// <summary>
        /// 尝试将字符串解析为数值 <br/>
        /// Tries to parse a string to a numeric value. <br/>
        /// </summary>
        /// <param name="input"> : <br/>
        /// 输入的字符串，尝试转换为数值 <br/>
        /// The input string, which is attempted to be parsed into a numeric value. <br/>
        /// </param>
        /// <returns>
        /// 如果字符串是有效的整数或浮点数格式，则返回true和对应的数值；否则，返回false和null。 <br/>
        /// If the string is in valid integer or floating-point format, then true and the corresponding number are returned. Otherwise, false and null are returned.<br/>
        /// </returns>
        public static bool TryParseNumeric(string input, out object result)
        {
            result = null;

            // 尝试解析为 long 类型
            if (long.TryParse(input, out long longValue))
            {
                result = longValue;
                return true;
            }

            // 尝试解析为 double 类型
            if (double.TryParse(input, out double doubleValue))
            {
                result = doubleValue;
                return true;
            }

            // 如果无法解析为数字，返回 false
            return false;
        }
    }
}