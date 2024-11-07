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
        /// 读取脚本（同步）<br/>
        /// Reads the script (synchronously)  
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
                GD.Print("Error reading file: " + e.Message);
                return null; // 或者处理错误
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

        /// <summary>
        /// 初始化脚本读取器并加载入口脚本。<br/>
        /// Initializes the script reader and loads the entry script. 
        /// </summary>
        /// <remarks>         
        /// 读取入口脚本: main.gs <br/>
        /// Reads the entry script: main.gs <br/>
        /// Godot平台入口脚本应在: res://Scripts/main.gs <br/>      
        /// Godot platform entry script should be located at: res://Scripts/main.gs <br/>
        /// Unity平台入口脚本应在: Assets/Scripts/main.gs <br/>
        /// Unity platform entry script should be located at: Assets/Scripts/main.gs 
        /// </remarks>
        public static void Initialization()
        {
#if GODOT
            string filePath = "res://Scripts/main.gs";
#elif UNITY_5_3_OR_NEWER
            string filePath = Path.Combine(Application.dataPath, "Scripts", "main.gs"); // Unity 中的路径
#endif
            string scriptContent = ReadScript(filePath);
            if (scriptContent != null)
            {
                BaseInterpreter.ParseScript(scriptContent);
            }
        }
    }
}
