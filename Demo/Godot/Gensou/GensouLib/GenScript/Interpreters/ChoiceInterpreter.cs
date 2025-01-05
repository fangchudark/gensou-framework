#if UNITY_5_3_OR_NEWER
using GensouLib.Unity.Core;
#else
using GensouLib.Godot.Core;
#endif
using System.Collections.Generic;

namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 选择指令解释器
    /// </summary>
    public class ChoiceInterpreter : BaseInterpreter
    {
        /// <summary>
        /// 是否处于选择状态
        /// </summary>
        public static bool OnChoosing {get; set; } = false;
        
        private static readonly List<string> choiceTexts = new();
        
        /// <summary>
        /// 选项对应的目标脚本名
        /// </summary>
        public static List<string> ChoiceTargets { get; set; } = new();
        
        /// <summary>
        /// 选项对应的目标脚本名所对应的执行行索引
        /// </summary>
        public static List<int> ChoiceLines { get; set; } = new();
        
        /// <summary>
        /// 解析选择指令
        /// </summary>
        /// <param name="param">参数</param>
        public static void ParseChoiceCommand(string param)
        {
            OnChoosing = true;
            string[] choices = param.Split('|');

            foreach (string choice in choices)
            {
                // 分割文字和分支目标
                string[] textAndTarget = choice.Split("->", System.StringSplitOptions.RemoveEmptyEntries);
                // 跳过语法错误
                if (textAndTarget.Length > 3) continue;
                string text = textAndTarget[0].Trim();
                string storyOrLine = textAndTarget[1].Trim();
                int lineIndex = -1;
                bool parseLineSuccess = false;
                if (textAndTarget.Length == 3)
                    if (TryParseNumeric(textAndTarget[2], out lineIndex))
                        parseLineSuccess = true;
                    else
                        continue;
                // 跳过空文本
                if (string.IsNullOrEmpty(text)) continue;
                // 跳过空目标
                if (string.IsNullOrEmpty(storyOrLine)) continue;
                string[] storyAndLine = storyOrLine.Split(':', System.StringSplitOptions.RemoveEmptyEntries);
                // 添加到列表
                choiceTexts.Add(text);
                if (storyAndLine[0] != ParamKeywords.Line)
                {
                    ChoiceTargets.Add(storyOrLine);
                    if (lineIndex < 0 && parseLineSuccess)
                        ChoiceLines.Add(0);
                    else if (parseLineSuccess)
                        ChoiceLines.Add(lineIndex);
                    else
                        ChoiceLines.Add(-1);
                    continue;
                }
                else if (storyAndLine.Length == 2)
                {
                    if (TryParseNumeric(storyAndLine[1], out int line))
                    {
                        ChoiceTargets.Add(ScriptReader.CurrentScriptName);
                         if (line < 0)
                                ChoiceLines.Add(0);
                        ChoiceLines.Add(line);
                    }
                    else
                    {
                        ChoiceTargets.Add(ScriptReader.CurrentScriptName);
                        ChoiceLines.Add(-1);
                    }
                }
            }
            // 创建按钮
            VisualNoveCore.CreateChoiceButtons(choiceTexts.ToArray(), 15);
            choiceTexts.Clear();
        }

        /// <summary>
        /// 选择选项
        /// </summary>
        /// <param name="index">选项索引</param>
        public static void SelectChoice(int index)
        {
            string target = ChoiceTargets[index];
            int line = ChoiceLines[index];
            VisualNoveCore.ClearChoiceButtons();
            if (line == -1) return;
            ScriptReader.ReadAndExecute(target, line);
            OnChoosing = false;
            ChoiceTargets.Clear();
            ChoiceLines.Clear();
        }
    }
}