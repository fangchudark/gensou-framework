#if GODOT
using GensouLib.Godot.Core;
#elif UNITY_5_3_OR_NEWER
using GensouLib.Unity.Core;
using UnityEngine.SceneManagement;
#endif
using System;
using System.Linq;

namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 解析脚本命令
    /// </summary>
    public class ParseScript
    {
        /// <summary>
        /// 只读，是否等待点击
        /// </summary>
        public static bool WaitClick {get; private set; } = true;

        /// <summary>
        /// 只读，是否是脚本隐藏了文本框
        /// </summary>
        public static bool ScriptHidedTextbox {get; private set; } = false;

        /// <summary>
        /// 解析命令 
        /// </summary>
        /// <param name="raw">
        /// 原始命令字符串。
        /// </param>
        public static void ParseCommand(string raw)
        {
            if (raw == CommandKeywords.End)
            {
#if UNITY_5_3_OR_NEWER
                SceneManager.LoadScene(VisualNoveCore.TitleScene);
#else
                VisualNoveCore.GameManagerNode.GetTree().ChangeSceneToFile(VisualNoveCore.TitleScenePath);
#endif
                return;
            }
            WaitClick = true; // 重置等待点击状态
            // 分割参数和命令部分
            string[] command = raw.Split(" -", StringSplitOptions.None);
            string commandAndParam = command[0].Trim(); // 命令与必选参数
            string optionalParam = command.Length > 1 
                ? raw[raw.IndexOf(" -")..] 
                : string.Empty; // 可选参数，不能去空格
            
            // 查找关键字
            string keyword = string.Empty; // 关键字
            string commandParam = string.Empty; // 命令参数
            int index = commandAndParam.IndexOf(':'); // 冒号索引
            // 有冒号没有关键字，keyword为空字符串；无冒号，commandParam为空字符串
            if (index == -1)
            {
                keyword = commandAndParam; // 无冒号
            }
            else
            {
                keyword = commandAndParam[..index].Trim(); // 获取关键字
                commandParam = commandAndParam[(index + 1)..].Trim(); // 获取命令参数
            }

            // 临时调试用
            // ScriptConsole.PrintLog("keyword: " + keyword + " commandParam: " + commandParam + " optionalParam: " + optionalParam);

            if (string.IsNullOrEmpty(optionalParam)) // 无可选参数
            {
                // 处理无可选参数命令
                switch (keyword)
                {
                    case CommandKeywords.Var:
                        VariableCommand(commandParam); // 变量声明、赋值
                        BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                        return;
                    case CommandKeywords.Release:
                        VariableInterpreter.ReleaseVariable(commandParam); // 释放变量
                        BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                        return;
                    case CommandKeywords.ChangeFigure:
                        DialogueInterpreter.ParseFigureCommand(commandParam); // 切换立绘
                        break;
                    case CommandKeywords.ChangePortrait:
                        DialogueInterpreter.ParsePortraitCommand(commandParam); // 切换肖像
                        break;
                    case CommandKeywords.ChangeBackground:
                        DialogueInterpreter.ParseBackgroundCommand(commandParam); // 切换背景
                        break;
                    case CommandKeywords.Bgm:
                        AudioInterpreter.ParseBgmCommand(commandParam); // BGM
                        BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                        return;
                    case CommandKeywords.Bgs:
                        AudioInterpreter.ParseBgsCommand(commandParam); // BGS
                        BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                        return;
                    case CommandKeywords.Se:
                        AudioInterpreter.ParseSeCommand(commandParam); // SE
                        BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                        return;
                    case CommandKeywords.Choose:
                        ChoiceInterpreter.ParseChoiceCommand(commandParam); // 选择选项
                        return;
                    case CommandKeywords.Call:
                        ScriptReader.ReadAndExecute(commandParam); // 调用脚本
                        return;
                    case CommandKeywords.SetTextbox:
                        SetTextBox(commandParam); // 显示/隐藏文本框
                        return;
                    default:
                        VisualNoveCore.History.Add(DialogueInterpreter.ParseDialogue(commandAndParam).ToString()); // 显示对话
                        VisualNoveCore.DisplayCurrentLine();
                        return;
                }
                if (VisualNoveCore.OnAutoPlay || VisualNoveCore.OnSkiping) 
                    BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                return;
            }
            // 可选参数列表
            string[] paramKeywords = optionalParam.Split(" -", StringSplitOptions.RemoveEmptyEntries);

            // 临时调试用
            // ScriptConsole.PrintLog("paramKeywords: " + string.Join(",", paramKeywords));

            // 如果可选参数列表不为空，但是没有能够解析的关键字，则视作对话命令
            if (
                paramKeywords.Length != 0 && 
                !paramKeywords.Any(x => KeywordsHelper.EqualsAnyParamKeyWord(x)) &&
                !paramKeywords.Any(x => KeywordsHelper.IsParamWithValueKeyWord(x)) &&
                !KeywordsHelper.EqualsAnyCommandKeyWord(keyword)
            )
            {
                // 临时调试用
                // ScriptConsole.PrintLog("cannot parse command: " + commandAndParam);
                // ScriptConsole.PrintLog("EqualsAnyCommandKeyWord: " + paramKeywords.Any(x => KeywordsHelper.EqualsAnyCommandKeyWord(x)));
                // ScriptConsole.PrintLog("EqualsAnyParamKeyWord: " + paramKeywords.Any(x => KeywordsHelper.EqualsAnyParamKeyWord(x)));
                // ScriptConsole.PrintLog("IsParamWithValueKeyWord: " + paramKeywords.Any(x => KeywordsHelper.IsParamWithValueKeyWord(x)));

                DialogueInterpreter.ParseDialogue(commandAndParam);
                VisualNoveCore.DisplayCurrentLine();
                return;
            }

            // 处理条件参数
            if (paramKeywords.Any(keyWord => keyWord.StartsWith(ParamKeywords.When) && !ProcessConditionCommand(keyWord)))
            {
                // 临时调试用
                // ScriptConsole.PrintLog("condition not met: " + commandAndParam);

                // 条件命令未通过，不再继续执行后续命令，直接执行下一行
                BaseInterpreter.ExecuteNextLine();
                return;
            }

            // 处理透明度参数
            float defaultAlpha = 1.0f;
            if (
                paramKeywords.Any(keyWord => keyWord.StartsWith(ParamKeywords.Alpha)) && 
                BaseInterpreter.TryParseNumeric(
                    KeywordsHelper.GetParamValue(paramKeywords, ParamKeywords.Alpha), 
                    out float alpha
                )
            )
            {
                defaultAlpha = alpha;
            }

            // 处理音量参数
            float defaultVolume = 1.0f;
            if (
                paramKeywords.Any(keyWord => keyWord.StartsWith(ParamKeywords.Volume)) && 
                BaseInterpreter.TryParseNumeric(
                    KeywordsHelper.GetParamValue(paramKeywords, ParamKeywords.Volume), 
                    out float volume
                )
            )
            {
                defaultVolume = volume;
            }

            // 处理淡入淡出时间参数
            float defaultFadeTime = 0.0f;
            if (
                paramKeywords.Any(keyWord => keyWord.StartsWith(ParamKeywords.Fade)) && 
                BaseInterpreter.TryParseNumeric(
                    KeywordsHelper.GetParamValue(paramKeywords, ParamKeywords.Fade), 
                    out float fadeTime
                )
            )
            {
                defaultFadeTime = fadeTime;
            }

            // 处理语音参数
            if (paramKeywords.Any(keyWord => keyWord.StartsWith(ParamKeywords.Voice)))
            {
                string voiceName = KeywordsHelper.GetParamValue(paramKeywords, ParamKeywords.Voice);
                AudioInterpreter.ParseVoiceCommand(voiceName, defaultVolume);
            }

            // 处理指定执行行参数
            int executeLineIndex = 0;
            if (
                paramKeywords.Any(keyword => keyword.StartsWith(ParamKeywords.Line)) &&
                BaseInterpreter.TryParseNumeric(
                    KeywordsHelper.GetParamValue(paramKeywords, ParamKeywords.Line), 
                    out int line
                )
            )
            {
                executeLineIndex = line;
                if (
                    executeLineIndex < 0 || 
                    (executeLineIndex == BaseInterpreter.CurrentLine - 1 && 
                     commandParam == ScriptReader.CurrentScriptName)
                )
                    executeLineIndex = 0;
            }

            // 处理字体大小参数
            int defaultFontSize = 24;
            if (
                paramKeywords.Any(keyWord => keyWord.StartsWith(ParamKeywords.FontSize)) && 
                BaseInterpreter.TryParseNumeric(
                    KeywordsHelper.GetParamValue(paramKeywords, ParamKeywords.FontSize), 
                    out int fontSize
                )
            )
            {
                defaultFontSize = fontSize;
                if (defaultFontSize < 1)
                    defaultFontSize = 24;
                VisualNoveCore.SetFontSize(defaultFontSize);
            }

            // 处理全局变量声明
            bool isGlobal = paramKeywords.Any(keyword => keyword.Equals(ParamKeywords.Global));
            // 检查是否执行完该行命令后立即执行下一条命令
            WaitClick = !paramKeywords.Any(keyword => keyword.Equals(ParamKeywords.Next));

            // 临时调试用
            // ScriptConsole.PrintLog("WaitClick: " + WaitClick);            

            switch (keyword)
            {
                case CommandKeywords.Var:
                    VariableCommand(commandParam, isGlobal); // 变量声明、赋值
                    BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                    return;
                case CommandKeywords.Release:
                    VariableInterpreter.ReleaseVariable(commandParam); // 释放变量
                    BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                    return;
                case CommandKeywords.ChangeFigure:// 切换立绘
                    if (paramKeywords.Any(keyword => keyword.Equals(ParamKeywords.Left)))
                        DialogueInterpreter.ParseFigureCommand(commandParam, defaultAlpha, ImageController.FigurePosition.Left);
                    else if (paramKeywords.Any(keyword => keyword.Equals(ParamKeywords.Right)))
                        DialogueInterpreter.ParseFigureCommand(commandParam, defaultAlpha, ImageController.FigurePosition.Right);
                    else
                        DialogueInterpreter.ParseFigureCommand(commandParam, defaultAlpha, ImageController.FigurePosition.Center);
                    break;
                case CommandKeywords.ChangePortrait:// 切换肖像
                    DialogueInterpreter.ParsePortraitCommand(commandParam, defaultAlpha);
                    break;
                case CommandKeywords.ChangeBackground:// 切换背景
                    DialogueInterpreter.ParseBackgroundCommand(commandParam, defaultAlpha);
                    break;
                case CommandKeywords.Bgm:
                    AudioInterpreter.ParseBgmCommand(commandParam, defaultVolume, defaultFadeTime); // BGM
                    BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                    break;
                case CommandKeywords.Bgs:
                    AudioInterpreter.ParseBgsCommand(commandParam, defaultVolume, defaultFadeTime); // BGS
                    BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                    break;
                case CommandKeywords.Se:
                    AudioInterpreter.ParseSeCommand(commandParam, defaultVolume); // SE
                    BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
                    break;
                case CommandKeywords.Choose:
                    ChoiceInterpreter.ParseChoiceCommand(commandParam); // 选择选项
                    return;
                case CommandKeywords.Call:
                    ScriptReader.ReadAndExecute(commandParam, executeLineIndex); // 调用脚本
                    break;
                case CommandKeywords.SetTextbox:
                    SetTextBox(commandParam); // 显示/隐藏文本框
                    return;
                default: // 无法解析的命令
                    VisualNoveCore.History.Add(DialogueInterpreter.ParseDialogue(commandAndParam).ToString()); // 显示对话
                    VisualNoveCore.DisplayCurrentLine();
                    break;
            }

            if (!WaitClick || VisualNoveCore.OnAutoPlay || VisualNoveCore.OnSkiping)
                BaseInterpreter.ExecuteNextLine(); // 执行下一行命令
        }

        private static bool ProcessConditionCommand(string command)
        {
            string condition = KeywordsHelper.GetParamValue(command); // 截取条件部分

            // 评估条件
            return ConditionInterpreter.CheckCondition(condition);
        }

        // 处理命令
        private static void VariableCommand(string command, bool isGlobal = false)
        {
            // 处理变量声明（不初始化）
            if (command.Length > 0 && !command.Contains('='))
            {
                VariableInterpreter.HandleVariableDeclaration(command, isGlobal);
            }
            else if (command.Contains('='))// 处理变量赋值
            {
                VariableInterpreter.HandleVariableAssignment(command, isGlobal);
            }
            // 忽略其他情况
        }

        private static void SetTextBox(string hide)
        {
            if (hide == "hide")
            {
                VisualNoveCore.ShowTextBox(false);    
                ScriptHidedTextbox = true;
            }
            else
            {
                VisualNoveCore.ShowTextBox(true);
                ScriptHidedTextbox = false;
            }
        }
    }
}