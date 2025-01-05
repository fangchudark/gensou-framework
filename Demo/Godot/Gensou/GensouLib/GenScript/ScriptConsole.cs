#if GODOT
using Godot;
#elif UNITY_5_3_OR_NEWER
using UnityEngine;
#endif
using GensouLib.GenScript.Interpreters;

namespace GensouLib.GenScript
{
    /// <summary> 
    /// 脚本控制台输出
    /// </summary>
    public class ScriptConsole
    {
        /// <summary> 
        /// 日志输出
        /// </summary>
        /// <param name="message">
        /// 信息
        /// </param>
        public static void PrintLog(params object[] message)
        {
            string formattedMessage = string.Concat("Line:",BaseInterpreter.CurrentLine,": ", string.Concat(message));
            
#if GODOT
                GD.Print(formattedMessage);
#elif UNITY_5_3_OR_NEWER
                Debug.Log(formattedMessage);
#endif
        }
        
        /// <summary> 
        /// 错误输出
        /// </summary>
        /// <param name="message">
        /// 信息
        /// </param>
        public static void PrintErr(params object[] message)
        {
            string formattedMessage = string.Concat("Line:",BaseInterpreter.CurrentLine,": ", string.Concat(message));
            
#if GODOT
                GD.PushError(formattedMessage);
#elif UNITY_5_3_OR_NEWER
                Debug.LogError(formattedMessage);
#endif
        }
    }
}
