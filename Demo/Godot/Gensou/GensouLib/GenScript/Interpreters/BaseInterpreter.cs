using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#if GODOT
using GensouLib.Godot.Core;
using Godot;
#elif UNITY_5_3_OR_NEWER
using GensouLib.Unity.Core;
#endif


namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 脚本解释器基类
    /// </summary>
    public class BaseInterpreter
    {
        /// <summary>
        /// 当前行索引
        /// </summary>
        public static int CurrentLine {get; set; } = 0; // 当前行索引
        
        /// <summary>
        /// 最大行号
        /// </summary>
        public static int CurrentMaxLine {get; set; } = 0; // 最大行号
        
        /// <summary>
        /// 当前脚本内容
        /// </summary>
        public static string[] CurrentScriptContent {get; set; } = null; // 当前脚本内容


        /// <summary> 
        /// 初始化解释器
        /// </summary>
        /// <param name="scriptContent"> 
        /// 读取到的脚本内容
        /// </param>
        public static void Init(string scriptContent, int initialLineIndex)
        {
            CurrentScriptContent = scriptContent.Split(new[] {'\n', '\r'},StringSplitOptions.RemoveEmptyEntries);
            CurrentLine = initialLineIndex; // 初始化当前行号
            CurrentMaxLine = CurrentScriptContent.Length; // 初始化最大行号
            ExecuteNextLine(); // 执行第一行
        }

        /// <summary>
        /// 执行下一行
        /// </summary>
        public static void ExecuteNextLine()
        {
            // 如果当前正在打字，则显示完整对话并返回
            if (VisualNoveCore.Typewriter.IsTyping && ParseScript.WaitClick)
            {
                VisualNoveCore.Typewriter.DisplayCompleteLine();
                return;
            }
            if (CurrentLine < CurrentMaxLine)
            {
                string content = Regex.Split(CurrentScriptContent[CurrentLine], @"(?<!\\);")[0]; // 去注释
                string replacedLine = content.Replace("\\;", ";"); // 替换转义
                string trimmedLine = replacedLine.Trim();

                // 跳过空行
                if (string.IsNullOrEmpty(trimmedLine))
                {
                    CurrentLine++;
                    ExecuteNextLine();
                    return;
                }
                CurrentLine++; // 增加行号(此时从索引变为执行行行号)

                ParseScript.ParseCommand(trimmedLine); // 执行代码部分
            }
        }

        /// <summary> 
        /// 替换占位符
        /// </summary>
        /// <param name="input"> 
        /// 文本
        /// </param>
        /// <returns> 
        /// 返回替换占位符后的字符串。如果有错误（如变量名不合法或未定义），返回相应的错误信息。
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
                { @"\\", @"\" }, // 转义转义符 \
                { @"\""", "\"" }, // 转义引号 "
                { @"\;", ";" },  // 转义分号 ;
                { @"\|", "|" },  // 转义竖线 |
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
        /// 尝试将字符串解析为数值
        /// </summary>
        /// <param name="input">
        /// 输入的字符串，尝试转换为数值
        /// </param>
        /// <returns>
        /// 如果字符串是有效的整数或浮点数格式，则返回true和对应的数值；否则，返回false和null。
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

        /// <summary>
        /// 尝试将字符串解析为指定类型
        /// </summary>
        /// <typeparam name="T">
        /// 目标类型
        /// </typeparam>
        /// <param name="input">
        /// 输入的字符串，尝试转换为指定类型
        /// </param>
        /// <param name="result">
        /// 输出结果
        /// </param>
        /// <returns>
        /// 如果字符串是有效的指定类型格式，则返回true和对应的数值；否则，返回false和默认值。
        /// </returns>
        public static bool TryParseNumeric<T>(string input, out T result) where T : struct
        {
            result = default;

            try
            {
                // 获取类型转换器
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                if (converter != null && converter.IsValid(input))
                {
                    result = (T)converter.ConvertFromString(input);
                    return true;
                }
            }
            catch
            {
                // 忽略异常，返回 false
            }

            return false;
        }

    }
}
