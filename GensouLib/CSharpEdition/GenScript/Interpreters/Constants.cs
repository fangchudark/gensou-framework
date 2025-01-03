using System.Linq;

namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 命令关键词
    /// </summary>
    public static class CommandKeywords
    {
        /// <summary>
        /// 释放变量
        /// </summary>
        public const string Release = "release"; // 释放变量
        
        /// <summary>
        /// 变量
        /// </summary>
        public const string Var = "var"; // 变量
        
        /// <summary>
        /// 切换立绘
        /// </summary>
        public const string ChangeFigure = "changeFigure"; // 切换立绘
        
        /// <summary>
        /// 切换头像
        /// </summary>
        public const string ChangePortrait = "changePortrait"; // 切换头像
        
        /// <summary>
        /// 切换背景
        /// </summary>
        public const string ChangeBackground = "changeBg"; // 切换背景

        /// <summary>
        /// 背景音乐
        /// </summary>
        public const string Bgm = "bgm"; // 背景音乐

        /// <summary>
        /// 音效
        /// </summary>
        public const string Bgs = "bgs"; // 音效
        
        /// <summary>
        /// 音效
        /// </summary>
        public const string Se = "se"; // 音效

        /// <summary>
        /// 切换剧本
        /// </summary>
        public const string Call = "call"; // 切换剧本

        /// <summary>
        /// 选择选项
        /// </summary>
        public const string Choose = "choose"; // 选择选项

        /// <summary>
        /// 返回标题
        /// </summary>
        public const string End = "end"; // 返回标题

        /// <summary>
        /// 隐藏或显示对话框
        /// </summary>
        public const string SetTextbox = "setTextbox"; // 隐藏或显示对话框

        /// <summary>
        /// 所有命令关键词
        /// </summary>
        public static readonly string[] AllCommandKeywords = 
        { 
            Release,
            Var, 
            ChangeFigure, 
            ChangePortrait,
            ChangeBackground,
            Bgm,
            Bgs,
            Se,
            Call,
            Choose,
            End,
            SetTextbox
        };
    }

    /// <summary>
    /// 可选参数关键词
    /// </summary>
    public static class ParamKeywords
    {
        /// <summary>
        /// 条件分支
        /// </summary>
        public const string When = "when"; // 条件分支

        /// <summary>
        /// 立即执行下一条语句
        /// </summary>
        public const string Next = "next"; // 立即执行下一条语句
        
        /// <summary>
        /// 立绘位置-左
        /// </summary>
        public const string Left = "left"; // 左侧立绘
        
        /// <summary>
        /// 立绘位置-右
        /// </summary>
        public const string Right = "right"; // 右侧立绘
        
        /// <summary>
        /// 立绘位置-中
        /// </summary>
        public const string Center = "center"; // 中部立绘

        /// <summary>
        /// 透明度
        /// </summary>
        public const string Alpha = "alpha"; // 透明度

        /// <summary>
        /// 音量
        /// </summary>
        public const string Volume = "volume"; // 音量

        /// <summary>
        /// 淡入淡出
        /// </summary>
        public const string Fade = "enter"; // 淡出淡入

        /// <summary>
        /// 声音
        /// </summary>
        public const string Voice = "voice"; // 声音

        /// <summary>
        /// 全局变量
        /// </summary>
        public const string Global = "global"; // 全局变量

        /// <summary>
        /// 行数
        /// </summary>
        public const string Line = "line"; // 行数

        /// <summary>
        /// 字体大小
        /// </summary>
        public const string FontSize = "fontSize"; // 字体大小

        /// <summary>
        /// 所有可选参数关键词
        /// </summary>
        public static readonly string[] AllParamKeywords = 
        { 
            When,
            Next,
            Left,
            Right,
            Center,
            Alpha,
            Volume,
            Fade,
            Voice,
            Global,
            Line,
            FontSize
        };

        /// <summary>
        /// 带值的可选参数关键词
        /// </summary>
        public static readonly string[] AllParamWithValueKeywords = 
        {
            When,
            Alpha,
            Volume,
            Fade,
            Voice,
            Line,
            FontSize
        };

    }

    /// <summary>
    /// 关键字辅助类
    /// </summary>
    public static class KeywordsHelper
    {
        /// <summary>
        /// 判断输入是否是任一命令关键词
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>匹配结果</returns>
        public static bool EqualsAnyCommandKeyWord(string input) 
            => CommandKeywords.AllCommandKeywords // 匹配任一的关键字
                                .Any(input
                                .Equals);

        /// <summary>
        /// 从字符串中获取第一个匹配的命令关键词
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>查找结果</returns>
        public static string GetCommandKeyWord(string input) 
            => CommandKeywords.AllCommandKeywords // 查找并返回第一个匹配的关键字
                                .FirstOrDefault(input
                                .Equals);

        /// <summary>
        /// 判断输入是否是任一可选参数关键词
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>匹配结果</returns>
        public static bool EqualsAnyParamKeyWord(string input) 
            => ParamKeywords.AllParamKeywords // 匹配任一的关键字
                                .Any(input
                                .Equals);

        /// <summary>
        /// 从字符串中获取第一个匹配的可选参数关键词
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>查找结果</returns>
        public static string GetParamKeyWord(string input) 
            => ParamKeywords.AllParamKeywords // 查找并返回第一个匹配的关键字
                                .FirstOrDefault(input 
                                .Equals);
        
        /// <summary>
        /// 判断输入是否是任一带值的可选参数关键词
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>匹配结果</returns>
        public static bool EqualsAnyParamWithValueKeyWord(string input) 
            => ParamKeywords.AllParamWithValueKeywords  // 匹配任一的关键字
                                .Any(input
                                .Equals);

        /// <summary>
        /// 从字符串中获取第一个匹配的带值的可选参数关键词
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetParamWithValueKeyWord(string input) 
            => ParamKeywords.AllParamWithValueKeywords // 查找并返回第一个匹配的关键字
                                .FirstOrDefault(input
                                .Equals);

        /// <summary>
        /// 判断输入是否是带值的可选参数关键词
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>匹配结果</returns>
        public static bool IsParamWithValueKeyWord(string input) 
            => input.Contains("=") && 
                ParamKeywords.AllParamWithValueKeywords // 截取等号前的关键字并进行匹配
                                .Any(keyword => input[..input.IndexOf("=")] // 截取等号前的关键字
                                .Trim() // 去除两端空格
                                .Equals(keyword)); // 匹配关键字

        /// <summary>
        /// 判断输入是否是指定带值的可选参数关键词
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="keyword">关键字</param>
        /// <returns>匹配结果</returns>
        public static bool IsParamWithValueKeyWord(string input, string keyword)
            => input.Contains("=") && // 确保是带值的参数
                ParamKeywords.AllParamWithValueKeywords.Contains(keyword) && // 确保指定关键字是已定义关键字
                input[..input.IndexOf("=")].Trim().Equals(keyword); // 匹配关键字


        /// <summary>
        /// 从字符串中获取带值的可选参数的值
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>获取的值，失败时返回空字符串</returns>
        public static string GetParamValue(string input)
            => !string.IsNullOrEmpty(input) && input.Contains("=") 
                ? input[(input.IndexOf("=") + 1)..].Trim()  // 截取等号后的值
                : string.Empty;

        /// <summary>
        /// 从字符串中获取指定带值的可选参数的值
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="keyword">关键字</param>
        /// <returns>获取的值，失败时返回空字符串</returns>
        public static string GetParamValue(string[] input, string keyword)
            => GetParamValue(input.FirstOrDefault(x => IsParamWithValueKeyWord(x, keyword)));
        
    }
}