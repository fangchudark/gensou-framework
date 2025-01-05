## 脚本读取器
class_name ScriptReader extends Object

## 脚本路径
static var script_path: String = "res://Story"

## 当前脚本路径
static var current_script_path: String

## 当前脚本原始数据
static var current_raw_script_data: String

## 当前脚本名
static var current_script_name: String

## 读取脚本。 [br]
## [br]
## [param file_path] : [br]
## 文件路径 [br]
## [br]
## 如果读取文件成功返回读取到的脚本文本内容，否则返回空字符串。
static func read_script(file_path: String) -> String:
	var file: FileAccess = FileAccess.open(file_path, FileAccess.READ)
	if not file:
		push_error("Cannot reading file")
		return ""
	current_script_path = file_path
	current_raw_script_data = file.get_as_text()
	return current_raw_script_data

## 读取并执行脚本。[br]
## [br]
## [param script] : [br]
## 脚本文件名 [br]
## [br]
## [param line_index] : [br]
## 起始执行行索引，默认为0
static func read_and_execute(script: String, line_index: int = 0) -> void:
	current_script_name = script
	var file_path: String = script_path.path_join(script + ".txt")
	
	var script_content: String = read_script(file_path)
	if script_content:
		BaseInterpreter.init(script_content, line_index)
