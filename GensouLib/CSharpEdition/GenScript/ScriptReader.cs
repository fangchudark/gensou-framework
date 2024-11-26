using System;
using GensouLib.GenScript.Interpreters;

#if GODOT
using Godot;
#elif UNITY_5_3_OR_NEWER
using UnityEngine;
using System.IO;
#endif

namespace GensouLib.GenScript
{
    /// <summary>
    /// 脚本读取器类<br/>
    /// Script reader class 
    /// </summary>
    public class ScriptReader
    {
        /// <summary> 
        /// 读取脚本<br/>
        /// Reads the script 
        /// </summary>
        /// <param name="filePath">
        /// 文件路径<br/>
        /// The file path 
        /// <br/>
        /// <remark> 
        /// Godot平台使用 res://path/to/script.gs <br/>
        /// For Godot platform use res://path/to/script.gs <br/>
        /// Unity平台使用 Path.Combine(Application.dataPath,"path","to","script.gs") <br/>
        /// For Unity platform use Path.Combine(Application.dataPath,"path","to","script.gs") 
        /// </remark>
        /// </param>
        /// <returns>
        /// 如果读取文件成功返回读取到的脚本文本内容，否则返回null。 <br/>
        /// If the file was read successfully, the script text content was returned; otherwise, null was returned.
        /// </returns>
        public static string ReadScript(string filePath)
        {
            string content = null;

#if GODOT
            FileAccess file = null;

            try
            {
                // 打开文件
                file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
                content = file.GetAsText();
            }
            catch (Exception e)
            {
                GD.PushError("Error reading file: " + e.Message);
                return null;
            }
            finally
            {
                // 确保文件被关闭
                if (file != null)
                {
                    file.Close();
                }
            }
#elif UNITY_5_3_OR_NEWER
            try
            {
                // Unity读取文件
                content = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading file: " + e.Message);
                return null;
            }
#endif
            return content;
        }
#if UNITY_5_3_OR_NEWER
        /// <summary>
        /// 读取并执行脚本。<br/>
        /// Read and execute the script. 
        /// </summary>
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
        public static void ReadAndExecute(string script="start")
        {
            string filePath = Path.Combine(Application.dataPath, "Scripts", script+".gs"); // Unity 中的路径
            string scriptContent = ReadScript(filePath);
            if (scriptContent != null)
            {
                BaseInterpreter.ParseScript(scriptContent);
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
        public static void ReadAndExecute(Node node, string script="start")
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
