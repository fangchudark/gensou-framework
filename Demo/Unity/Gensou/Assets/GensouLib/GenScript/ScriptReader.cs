using System;
using GensouLib.GenScript.Interpreters;
using System.Linq;


#if GODOT
using Godot;
#elif UNITY_5_3_OR_NEWER
using UnityEngine;
using System.IO;
using GensouLib.Unity.ResourceLoader;
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
        public static string ScriptPath { get; set; } = "res://Stories";
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
            CurrentScriptName = filePath;
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
#if UNITY_5_3_OR_NEWER
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
#if ENABLE_ADDRESSABLES == false
            string filePath = string.Join('/', ScriptPath, script);
#else
            string filePath = script;
#endif
            string scriptContent = ReadScript(filePath);
            if (scriptContent != null)
            {
                BaseInterpreter.Init(scriptContent, lineIndex);
            }
        }
#elif GODOT
        /// <summary>
        /// 读取并执行脚本。<br/>
        /// Read and execute the script. 
        /// </summary>
        /// <param name="node">
        /// 挂载到自动加载的脚本初始化器节点。<br/>
        /// Mount to the autoloaded script initializer node.
        /// </param>
        /// <param name="script">
        /// 脚本文件名 <br/>
        /// Script file name
        /// </param>
        /// <remarks>         
        /// 默认读取入口脚本: start.gs <br/>
        /// By default, reads the entry script: start.gs  <br/>
        /// Godot平台脚本应在: res://Scripts/scriptfile.gs <br/>      
        /// Godot platform scripts should be located at: res://Scripts/scriptfile.gs <br/>
        /// Unity平台脚本应在: Assets/Scripts/scriptfile.gs <br/>
        /// Unity platform scripts should be located at: Assets/Scripts/scriptfile.gs 
        /// </remarks>
        public static void ReadAndExecute(Node node, string script = "start")
        {
            string filePath = "res://Scripts/"+script+".gs";

            string scriptContent = ReadScript(filePath);
            if (scriptContent != null)
            {
                BaseInterpreter.ParseScript(scriptContent, node);
            }
        }
#endif
    }
}
