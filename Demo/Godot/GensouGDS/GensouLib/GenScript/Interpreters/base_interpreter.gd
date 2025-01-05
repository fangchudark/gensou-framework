## 脚本解释器基类 
class_name BaseInterpreter extends Object

## 当前行索引
static var current_line: int = 0

## 最大行号
static var current_max_line = 0

## 当前脚本内容
static var current_script_content: PackedStringArray

## 初始化
## [br]
## [param script_content] : [br]
## 脚本内容 [br]
## [param initial_line_index] : [br]
## 初始行索引
static func init(script_content: String, initial_line_index: int) -> void:
	current_script_content = script_content.rsplit("\n", false)
	current_line = initial_line_index
	current_max_line = current_script_content.size()
	execute_next_line()

## 执行下一行 
static func execute_next_line() -> void:
	if VisualNoveCore.typewriter.is_typing and ParseScript.wait_click:
		VisualNoveCore.typewriter.display_complete_line()
		return
	if current_line < current_max_line:
		var regex: RegEx = RegEx.new()
		regex.compile(r"(?<!\\);") # 去注释
		var content: String
		var results = regex.search_all(current_script_content[current_line])
		if results.size() == 0:
			content = current_script_content[current_line]
		else:
			for result in results:
				content = current_script_content[current_line].substr(0, result.get_start())
		var replaced_line: String = content.replace("\\;", ";")  # 替换转义的分号
		var trimmed_line: String = replaced_line.strip_edges()  # 去除两端空白

		if trimmed_line.is_empty():  # 如果行为空，跳过
			current_line += 1
			execute_next_line()
			return
		current_line += 1  # 行索引加一

		ParseScript.parse_command(trimmed_line)

## 替换占位符 [br]
## [br]
## [param input] : [br]
## 输入的字符串 [br]
## [br]
## 返回替换占位符后的字符串。如果有错误（如变量名不合法或未定义），返回相应的错误信息。
static func replace_placeholders(input: String) -> String:
	# 定义转义字符及其对应的替换字符
	var escape_sequences: Dictionary = {
		r"\{" : "{",    # 转义花括号左
		r"\}" : "}",    # 转义花括号右
		r"\/" : "/",    # 转义斜杠
		r"\+" : "+",    # 转义加号
		r"\-" : "-",    # 转义减号
		r"\*" : "*",    # 转义星号
		r"\%" : "%",    # 转义百分号
		r"\\" : "\\",   # 转义反斜杠
		r"\"" : "\"",   # 转义双引号
		r"\;" : ";",    # 转义分号
		r"\|" : "|",    # 转义竖线
	}
	
	# 遍历转义字符字典，进行替换
	for escape in escape_sequences:
		input = input.replace(escape, escape_sequences[escape])
	
	# 创建正则表达式对象，匹配变量占位符（如：{variable_name}）
	var regex = RegEx.new()
	regex.compile(r"(?<!\\){([\p{L}\p{N}_]+)}")  # (?<!\\) 确保没有被转义的花括号
	
	# 初始化输出字符串为原始输入
	var output = input
	
	# 遍历所有匹配的变量占位符
	for token in regex.search_all(input):
		var variable_name = token.get_string(1).strip_edges()  # 获取变量名并去除两端空白
		
		# 检查变量名是否合法
		if not VariableInterpreter.check_variable_name(variable_name):
			push_error("Variable name: %s invalid (变量名: %s 不合法)" % [variable_name, variable_name])
			return "InvalidVariableName"  # 如果不合法，返回错误信息
		
		# 检查变量是否在变量列表中
		if VariableInterpreter.variable_list.has(variable_name):
			# 如果变量已定义，替换占位符为其对应的值
			output = output.replace("{%s}" % [variable_name], str(VariableInterpreter.variable_list[variable_name]))
		else:
			# 如果变量未定义，报告未定义错误
			push_error("Undefined variable: %s (未定义的变量: %s)" % [variable_name, variable_name])
			return "UndefinedVariable"  # 如果未定义，返回错误信息
	
	# 返回替换后的字符串
	return str(output)



## 尝试将字符串解析为布尔值 [br]
### [br]
## [param value] : [br]
## 输入的字符串 [br]
## [br]
## 返回一个字典，包含[code]result[/code]和[code]success[/code]字段。如果转换成功，[code]result[/code]为布尔值，且[code]success[/code]为 true；否则，[code]result[/code]为空，且[code]success[/code]为 false。 [br]
static func try_parse_str_to_bool(value: String) -> Dictionary:
	# 判断输入是否与 "false"（忽略大小写）匹配
	if not value.nocasecmp_to("false"):
		return {
			"result": false,  # 返回 false
			"success": true   # 解析成功
		}
	
	# 判断输入是否与 "true"（忽略大小写）匹配
	if not value.nocasecmp_to("true"):
		return {
			"result": true,   # 返回 true
			"success": true   # 解析成功
		}
	
	# 如果既不是 "true" 也不是 "false"，返回失败结果
	return {
		"result": null,    # 结果为 null，表示解析失败
		"success": false   # 解析失败
	}

	
	
## 尝试将字符串解析为数值 [br]
## [br]
## [param input] : [br]
## 输入的字符串，尝试转换为数值 [br]
## [br]
## 返回一个字典，包含[code]result[/code]和[code]success[/code]字段。如果转换成功，[code]result[/code]为数值（整数或浮点数），且[code]success[/code]为 true；否则，[code]result[/code]为空，且[code]success[/code]为 false。 [br]
static func try_parse_numeric(input: String) -> Dictionary:
	# 判断输入是否为有效整数
	if input.is_valid_int():
		return {
			"result": input.to_int(),
			"success": true
		}
	# 判断输入是否为有效浮点数（包括整数）
	if input.is_valid_float():
		return {
			"result": input.to_float(),
			"success": true
		}
	# 解析失败
	return {
		"result": null,
		"success": false
	}



#static func trim(string: String, trim_string: String) -> String:
	#if string.begins_with(trim_string) and string.ends_with(trim_string):
		#return string.trim_prefix(trim_string).trim_suffix(trim_string)
	#else: 
		#return string
