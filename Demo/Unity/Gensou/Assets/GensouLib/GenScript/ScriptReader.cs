using GensouLib.GenScript.Interpreters;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
using GensouLib.Unity.ResourceLoader;
#else
using Godot;
#endif

namespace GensouLib.GenScript
{
    /// <summary>
    /// 脚本读取器
    /// </summary>
    public class ScriptReader
    {
#if GODOT
        /// <summary>
        /// 脚本路径
        /// </summary>
        public static string ScriptPath { get; set; } = "res://Story";
#elif UNITY_5_3_OR_NEWER && !ENABLE_ADDRESSABLES
        /// <summary>
        /// 脚本路径
        /// </summary>
        public static string ScriptPath { get; set; } = "Story";
#endif
        
        /// <summary>
        /// 当前脚本路径
        /// </summary>
        public static string CurrentScriptPath { get; private set; }
        
        /// <summary>
        /// 当前脚本原始数据
        /// </summary>
        public static string CurrentRawScriptData { get; private set; }

        /// <summary>
        /// 当前脚本名
        /// </summary>
        public static string CurrentScriptName { get; set; }

        /// <summary> 
        /// 读取脚本
        /// </summary>
        /// <param name="filePath">
        /// 文件路径
        /// </param>
        /// <returns>
        /// 脚本内容，读取失败时返回null。
        /// </returns>
        public static string ReadScript(string filePath)
        {
#if GODOT
            using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
            if (file == null) 
            {
                GD.PushError("Cannot reading file");
                return null;
            }
            CurrentScriptPath = filePath;
            CurrentRawScriptData = file.GetAsText();
            return CurrentRawScriptData;
#elif UNITY_5_3_OR_NEWER
            AssetLoader.LoadResource<TextAsset>(filePath);
            TextAsset textAsset = AssetLoader.GetLoadedAsset<TextAsset>(filePath);
            if (textAsset == null)
            {
                Debug.LogError("Cannot reading file: " + filePath);
                return null;
            }
            CurrentScriptPath = filePath;
            CurrentRawScriptData = textAsset.text;
            return CurrentRawScriptData;
#endif
        }
        
        /// <summary>
        /// 读取并执行脚本。
        /// </summary>
        /// <param name="script">
        /// 脚本文件名 
        /// </param>
        /// <param name="lineIndex">
        /// 起始执行行索引，默认为0。
        /// </param>
        public static void ReadAndExecute(string script, int lineIndex = 0)
        {
            CurrentScriptName = script;
#if UNITY_5_3_OR_NEWER
#if ENABLE_ADDRESSABLES == false
            string filePath = string.Join('/', ScriptPath, script);
#else
            string filePath = script;
#endif
#elif GODOT      
            string filePath = string.Join('/', ScriptPath, script + ".txt");
#endif

            string scriptContent = ReadScript(filePath);
            if (scriptContent != null)
            {
                BaseInterpreter.Init(scriptContent, lineIndex);
            }
        }
    }
}
