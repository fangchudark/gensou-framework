## 脚本执行器 [br]
## Script executor
##
## 负责进一步解析脚本，并执行相应命令 [br]
## parse the script further and execute the appropriate command [br]
## 该类依赖于[BaseInterpreter]类及其派生类提供方法来处理命令的执行 [br]
## This class relies on the [BaseInterpreter] class and its derivatives to provide methods to handle command execution
class_name ScriptExecutor extends Object


## 执行命令 [br]
## Execute commands [br]
## [br]
## [param command] : [br]
## 命令。[br]
## Command. [br]
## [param node] : [br]
## 挂载到自动加载的脚本初始化器节点。[br]
## Mount to the autoloaded script initializer node.
static func execute_command(command: String, node: Node) -> void:
	if command.contains("-when:"):
		_process_condition_command(command)
		return
	_command_to_execute(command)
		

static func _process_condition_command(command: String) -> void:
	var code: PackedStringArray = command.rsplit("-when:")
		
	if code.size() < 2:
		push_error("Invalid command format for condition.(条件命令格式无效。)")
		return
		
	var command_to_execute: String = code[0].strip_edges()
	var condition: String = code[1].strip_edges()
	if ConditionInterpreter.check_condition(condition):
		_command_to_execute(command_to_execute)


static func _command_to_execute(command: String) -> void:
	if command.begins_with("release:"):
		var variable_name = command.substr(8).strip_edges()
		VariableInterpreter.release_variable(variable_name)
		return
	elif command.begins_with("-"):
		_handle_command(command)


static func _handle_command(command: String) -> void:
	var code: String = command.substr(1).strip_edges()
	
	if code.length() > 0 and not code.contains("=") and not code.begins_with("@"):
		VariableInterpreter.handle_variable_declaration(code)  
	elif code.begins_with("@"):
		BaseInterpreter.handle_debug_output(code)  
	elif code.contains("="):
		VariableInterpreter.handle_variable_assignment(code)
	
