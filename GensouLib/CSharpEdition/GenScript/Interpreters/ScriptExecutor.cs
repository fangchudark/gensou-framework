#if GODOT
using System;
using Godot;
#endif

namespace GensouLib.GenScript.Interpreters
{
    /// <summary> 
    /// 脚本执行器<br/>
    /// Script executor
    /// </summary>
    /// <remarks>
    /// 负责进一步解析脚本，并执行相应命令<br/>
    /// parse the script further and execute the appropriate command <br/>
    /// 该类依赖于BaseInterpreter类及其派生类提供方法来处理命令的执行 <br/>
    /// This class relies on the BaseInterpreter class and its derivatives to provide methods to handle command execution
    /// </remarks>
    public class ScriptExecutor
    {
#if GODOT
        /// <summary>
        /// 执行命令 <br/>
        /// Execute commands
        /// </summary>
        /// <param name="command">
        /// 命令。<br/>
        /// command
        /// </param>
        /// <param name="node">
        /// 挂载到自动加载的脚本初始化器节点。<br/>
        /// Mount to the autoloaded script initializer node.
        /// </param>
        public static void ExecuteCommand(string command, Node node)
#elif UNITY_5_3_OR_NEWER
        /// <summary>
        /// 执行命令 <br/>
        /// Execute commands
        /// </summary>
        /// <param name="command">
        /// 命令。<br/>
        /// command
        /// </param>
        public static void ExecuteCommand(string command)
#endif
        {
            // 检查并处理条件命令
            if (command.Contains("-when="))
            {
                ProcessConditionCommand(command);
                return;  // 条件命令已处理，不再继续执行其他命令
            }
            
            // 处理普通命令
            CommandToExecute(command);
        }

        private static void ProcessConditionCommand(string command)
        {
            string[] code = command.Split(new string[] { "-when=" }, StringSplitOptions.None);

            if (code.Length < 2)
            {
                ScriptConsole.PrintErr("Invalid command format for condition.(条件命令格式无效。)");
                return;
            }

            string commandToExecute = code[0].Trim();
            string condition = code[1].Trim();

            // 如果条件成立，执行相应的命令
            if (ConditionInterpreter.CheckCondition(condition))
            {
                CommandToExecute(commandToExecute);
            }
        }

        private static void CommandToExecute(string command)
        {
            if (command.StartsWith("release:"))
            {
                // 从命令中提取变量名，调用释放方法
                string variableName = command[8..].Trim();
                VariableInterpreter.ReleaseVariable(variableName);
            }
            else if (command.StartsWith("-"))
            {
                // 处理其他命令（如控制台输出等）
                HandleCommand(command);
            }
        }

        
        // 处理命令
        private static void HandleCommand(string command)
        {
            // 截去命令标识符,获取命令内容
            string code = command[1..].Trim();
            // 处理变量声明（不初始化）
            if (code.Length > 0 && !code.Contains('=') && !code.StartsWith('@'))
            {
                VariableInterpreter.HandleVariableDeclaration(code);
            }
            // 调试输出
            else if (code.StartsWith("@")) // 检查命令是否以 "@" 开头
            {
                BaseInterpreter.HandleDebugOutput(code);
            }
            else if (code.Contains('='))// 处理变量赋值
            {
                VariableInterpreter.HandleVariableAssignment(code);
            }
        }

    }
}