#if UNITY_5_3_OR_NEWER
using System;
using System.Text.RegularExpressions;
using GensouLib.Unity.Core;
using GensouLib.Unity.ResourceLoader;
using UnityEngine;
#endif

namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 对话解析器
    /// </summary>
    public class DialogueInterpreter : BaseInterpreter
    {
        /// <summary>
        /// 对话结构体
        /// </summary>
        public struct Dialogue
        {
            /// <summary>
            /// 说话者
            /// </summary>
            public string Speaker;

            /// <summary>
            /// 内容
            /// </summary>
            public string Content;
            public Dialogue(string speaker, string content)
            {
                Speaker = speaker; // 说话者
                Content = content; // 内容
            }
            public override readonly string ToString()
                => string.IsNullOrEmpty(Speaker) ? Content : $"{Speaker}: {Content}";
        }

        /// <summary>
        /// 当前说话者
        /// </summary>
        public static string CurrentSpeaker {get; set;} = ""; 
        
        /// <summary>
        /// 当前对话内容
        /// </summary>
        public static string CurrentDialogue {get; set;} = "";

        /// <summary>
        /// 解析对话
        /// </summary>
        /// <param name="dialogue">对话命令</param>
        /// <returns>对话结构体</returns>
        public static Dialogue ParseDialogue(string dialogue)
        {
            if (dialogue.StartsWith(':')) // 无说话者
            {
                CurrentDialogue = ReplacePlaceholders(Regex.Replace(dialogue[1..], @"(?<!\\)\|", "\n"));
                CurrentSpeaker = string.Empty;
                return new(CurrentSpeaker, CurrentDialogue);
            }
            if (!dialogue.Contains(':')) // 沿用上一句对话的说话者
            {
                CurrentDialogue = ReplacePlaceholders(Regex.Replace(dialogue, @"(?<!\\)\|", "\n"));
                return new(CurrentSpeaker, CurrentDialogue);
            }
            int colonIndex = dialogue.IndexOf(':'); // 找到冒号
            CurrentSpeaker = ReplacePlaceholders(dialogue[..colonIndex]); // 保存说话者
            CurrentDialogue = ReplacePlaceholders(Regex.Replace(dialogue[(colonIndex + 1)..], @"(?<!\\)\|", "\n")); // 保存内容
            return new(CurrentSpeaker, CurrentDialogue); // 返回对话
        }

        /// <summary>
        /// 解析立绘命令
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="alpha">透明度</param>
        /// <param name="position">位置</param>
        public static void ParseFigureCommand(string param, float alpha = 1.0f, ImageController.FigurePosition position = ImageController.FigurePosition.Center)
            => ParseImageCommand(
                param,
                alpha,
                VisualNoveCore.FigurePath,
                (image, a, hide) => ImageController.ChangeFigure(image, a, position, hide));

        /// <summary>
        /// 解析头像命令
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="alpha">透明度</param>
        public static void ParsePortraitCommand(string param, float alpha = 1.0f)
            => ParseImageCommand(
                param,
                alpha,
                VisualNoveCore.PortraitPath,
                (image, a, hide) => ImageController.ChangePortrait(image, a, hide));

        /// <summary>
        /// 解析背景命令
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="alpha">透明度</param>
        public static void ParseBackgroundCommand(string param, float alpha = 1.0f)
            => ParseImageCommand(
                param,
                alpha,
                VisualNoveCore.BackgroundPath,
                (image, a, hide) => ImageController.ChangeBackground(image, a),
                false); // 背景不需要显式隐藏

        private static void ParseImageCommand(
            string param, 
            float alpha, 
            string pathPrefix, 
            Action<Sprite, float, bool> changeImageAction, 
            bool isHideOnNone = true)
        {
            if (param == "none")
            {
                // 移除精灵并隐藏图片
                changeImageAction(null, alpha, isHideOnNone);
                return;
            }

            string path = param;
#if UNITY_5_3_OR_NEWER
#if ENABLE_ADDRESSABLES == false
            path = string.Join("/", pathPrefix, param); // 资源路径
#endif
            AssetLoader.LoadResource<Sprite>(path); // 加载资源
            Sprite image = AssetLoader.GetLoadedAsset<Sprite>(path);
            if (image == null)
            {
                Debug.LogError($"Failed to load image {path} (无法加载图片{path}).");
                return;
            }
#endif
            // 更新图片
            changeImageAction(image, alpha, false);
        }
    }
}