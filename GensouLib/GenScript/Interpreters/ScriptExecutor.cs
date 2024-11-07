namespace GensouLib.GenScript.Interpreters
{
    /// <summary> 脚本执行器 </summary>
    /// <remarks>
    /// 负责进一步解析脚本，并执行相应命令<br/>
    /// 该类依赖于BaseInterpreter类及其派生类提供方法来处理命令的执行
    /// </remarks>
    public class ScriptExecutor
    {

        /// <summary> 执行命令 </summary>
        /// <param name="command">命令</param>
        public static void ExecuteCommand(string command)
        {
            // 检查是否包含条件命令
            if (command.Contains("-when:"))
            {
                string[] code = command.Split("-when:");

                if (code.Length < 2)
                {
                    ScriptConsole.PrintErr("Invalid command format for condition.(条件命令格式无效。)"); // 错误提示
                    return;
                }

                string commandToExecute = code[0];
                string condition = code[1];

                // 如果条件成立，执行相应的命令
                if (ConditionInterperter.CheckCondition(condition))
                {
                    HandleCommand(commandToExecute);
                }
                return;
            }
            
            if (command.StartsWith("-"))
            {
                HandleCommand(command);
            }

        }
        
        /// <summary> 处理命令 </summary>
        /// <param name="command">命令</param>
        private static void HandleCommand(string command)
        {
            // 截去命令标识符,获取命令内容
            string code = command[1..].Trim();
            // 处理变量声明（不初始化）
            if (code.Length > 0 && !code.Contains('=') && !code.StartsWith('@'))
            {
                BaseInterpreter.HandleVariableDeclaration(code);
                return;
            }
            // 处理变量赋值
            if(code.Contains("="))
            {
                BaseInterpreter.HandleVariableAssignment(code);
                return; // 结束方法
            }
            // 调试输出
            if (code.StartsWith("@")) // 检查命令是否以 "@" 开头
            {
                BaseInterpreter.HandleDebugOutput(code);
                return;
            }
        }

    }
}