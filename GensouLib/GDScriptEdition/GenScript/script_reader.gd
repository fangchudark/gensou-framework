## 脚本读取器类 [br] 
## Script reader class 
##
## 脚本读取器类 [br]
## Script reader class 
class_name ScriptReader extends Object

## 读取脚本。 [br]
## Reads the script. [br]
## [br]
## [param file_path] : [br]
## 文件路径 [br]
## The file path[br]
## [br]
## 如果读取文件成功返回读取到的脚本文本内容，否则返回空字符串。[br]
## If the file was read successfully, the script text content was returned; otherwise, an empty string was returned. [br]
static func read_script(file_path: String) -> String:
	var content:String = ""
	var file = FileAccess.open(file_path, FileAccess.READ)
	
	if file:
		content = file.get_as_text()
		file.close()
	else:
		push_error("Error reading file!")
	return content

## 读取并执行脚本。[br]
## Read and execute the script. [br]
## 默认读取入口脚本: [b]start.gs[/b] [br]
## By default, reads the entry script: [b]start.gs[/b] [br]
## 脚本应在: [code]res://Scripts/scriptfile.gs [/code][br]
## Scripts should be located at: [code]res://Scripts/scriptfile.gs [/code][br]
## [br]
## [param node] : [br]
## 挂载到自动加载的脚本初始化器节点。[br]
## Mount to the autoloaded script initializer node.[br]
## [param script] : [br]
## 脚本文件名 [br]
## Script file name
static func read_and_execute(node: Node, script: String = "start") -> void:
	var file_path: String = "res://Scripts/" + script + ".gs"
	var script_content: String = read_script(file_path)
	if script_content:
		BaseInterpreter.parse_script(script_content, node)
