## 脚本解释器基类 [br]
## Script interpreter base class 
##
## 负责解析脚本并执行命令。[br]
## Responsible for parsing scripts and executing commands.[br]
## 提供控制台命令处理和字符串解析。[br]
## Provides console command processing and string parsing.[br]
## 可供其他类继承和扩展以实现更多功能。[br]
## This class can be inherited and extended by other classes for additional methodality. 
class_name BaseInterpreter extends Object

# 多行日志计数器
static var _multi_line_log_conut: int = 0


## 解析脚本 [br]
## Parses the script. [br]
## 该方法将脚本内容按行解析，并对每一行进行处理，支持多行日志、注释标记处理和命令执行。 [br]
## This method parses the script content line by line and processes each line, supporting multi-line logging, comment marker handling, and command execution. [br]
## [br]
## [param script_content] : [br]
## 读取到的脚本内容。 [br]
## The content of the script that has been read. [br]
## [param node] : [br]
## 挂载到自动加载的脚本初始化器节点，用于支持脚本命令的执行上下文。 [br]
## Mount to the autoloaded script initializer node for command execution context.
static func parse_script(script_content: String, node: Node) -> void:
	# 将输入的脚本内容按行分割，使用 "\r\n" 作为分隔符
	var lines: PackedStringArray = script_content.rsplit("\r\n", false)
	
	# 遍历每一行脚本内容
	for line in lines:
		# 移除行两端的空格字符
		var trimmed_line = line.strip_edges()
		
		# 如果该行为空行，跳过该行
		if trimmed_line.is_empty():
			continue
		
		# 如果存在多行日志标记，则替换占位符并打印，减少多行日志计数
		if _multi_line_log_conut > 0:
			print(replace_placeholders(line))  # 打印替换占位符后的行
			_multi_line_log_conut -= 1  # 多行日志计数减一
			continue
		
		# 查找行中是否存在注释标记 "|:"
		var comment_index: int = trimmed_line.find("|:")
		
		# 如果找到了注释标记，提取代码部分并执行
		if comment_index != -1:
			var code_part = trimmed_line.substr(0, comment_index)  # 提取注释前的代码部分
			ScriptExecutor.execute_command(code_part, node)  # 执行代码部分
		else:
			# 如果没有注释标记，直接执行整行代码
			ScriptExecutor.execute_command(trimmed_line, node)



## 处理控制台命令 [br]
## Processes console commands. [br]
## 该方法解析控制台命令并执行相应的操作，包括设置多行日志计数、解析和计算表达式或打印替换后的字符串。 [br]
## This method parses console commands and executes the corresponding operations, such as setting multi-line log counts, parsing and evaluating expressions, or printing replaced strings. [br]
## [br]
## [param code] : [br]
## 命令标识符后的代码，包含调试指令或表达式。 [br]
## The code following the command identifier, including debug instructions or expressions. 
static func handle_debug_output(code: String) -> void:
	# 从代码的第二个字符开始截取，并去除两端的空白字符
	var console_print = code.substr(1).strip_edges()
	
	# 检查是否是以 "<" 开头并以 ">" 结尾
	if console_print.begins_with("<") and console_print.ends_with(">"):
		# 提取并去除两端的 "< >" 部分
		var inner_text = console_print.substr(1, console_print.length() - 2).strip_edges()
		
		# 如果提取出的文本是有效的整数
		if inner_text.is_valid_int():
			# 如果整数大于 0，设置多行日志的计数
			if inner_text.to_int() > 0:
				_multi_line_log_conut = inner_text.to_int()
		return
	
	# 如果 console_print 是有效的表达式
	if VariableInterpreter.check_expression(console_print):
		# 将表达式转换为后缀表达式
		var postfix_expression = VariableInterpreter.intfix_to_postfix(console_print)
		
		# 评估后缀表达式并获取结果
		var result = VariableInterpreter.evaluate_postfix(postfix_expression)
		
		# 如果结果是有效的整数
		if result.is_valid_int():
			# 如果整数溢出，则报告错误
			if VariableInterpreter.check_int_overflow(result):
				push_error("Integer calculation result out of range; you are seeing a floating-point result.(整数计算结果超出范围,你看到的是浮点数结果)")
			# 打印结果
			print(result)
		# 如果结果是有效的浮点数
		elif result.is_valid_float():
			print(result)
		# 如果无法解析表达式，则报告错误
		else:
			push_error("Unable to parse the expression: %s (无法解析的表达式: %s)" % [console_print, console_print])
	else:
		# 如果不是有效的表达式，打印占位符替换后的结果
		print(replace_placeholders(console_print))


## 替换占位符 [br]
## Replaces placeholders. [br]
## 该方法会替换输入字符串中的占位符（如：{variable_name}、\+、\\、\{），如果该变量存在并已定义，将使用其值替换占位符。 [br]
## The method replaces placeholders (e.g., {variable_name}、\+、\\、\{) in the input string. If the variable exists and is defined, its value will replace the placeholder. [br]
## [br]
## [param input] : [br]
## 输入的字符串，其中包含可能需要替换的占位符 [br]
## The input string which may contain placeholders to be replaced[br]
## [br]
## 返回替换占位符后的字符串。如果有错误（如变量名不合法或未定义），返回相应的错误信息。 [br]
## Returns the string with placeholders replaced. If there is an error (e.g., invalid or undefined variable names), an error message is returned.
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
## Tries to parse a string to a boolean value. [br]
## 该方法尝试将输入的字符串转换为布尔值（true 或 false）。如果输入值为“true”或“false”（不区分大小写），则返回相应的布尔值；否则，返回失败结果。 [br]
## The method attempts to parse the input string to a boolean value (true or false). If the input is "true" or "false" (case insensitive), it returns the corresponding boolean value; otherwise, it returns a failure result. [br]
## [br]
## [param value] : [br]
## 输入的字符串，尝试转换为布尔值 [br]
## The input string, which is attempted to be parsed to a boolean value. [br]
## [br]
## 返回一个字典，包含[code]result[/code]和[code]success[/code]字段。如果转换成功，[code]result[/code]为布尔值，且[code]success[/code]为 true；否则，[code]result[/code]为空，且[code]success[/code]为 false。 [br]
## Returns a dictionary with [code]result[/code] and [code]success[/code] fields. If the conversion is successful, [code]result[/code] is a boolean value and [code]success[/code] is true; otherwise, [code]result[/code] is null and [code]success[/code] is false. [br]
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
## Tries to parse a string to a numeric value. [br]
## 该方法尝试将输入的字符串转换为整数或浮点数。如果字符串是有效的整数或浮点数格式，则返回对应的数值；否则，返回失败结果。 [br]
## The method attempts to parse the input string into an integer or a floating-point number. If the string is a valid integer or floating-point format, the corresponding numeric value is returned; otherwise, a failure result is returned. [br]
## [br]
## [param input] : [br]
## 输入的字符串，尝试转换为数值 [br]
## The input string, which is attempted to be parsed into a numeric value. [br]
## [br]
## 返回一个字典，包含[code]result[/code]和[code]success[/code]字段。如果转换成功，[code]result[/code]为数值（整数或浮点数），且[code]success[/code]为 true；否则，[code]result[/code]为空，且[code]success[/code]为 false。 [br]
## Returns a dictionary with [code]result[/code] and [code]success[/code] fields. If the conversion is successful, [code]result[/code] is a numeric value (either integer or floating-point) and [code]success[/code] is true; otherwise, [code]result[/code] is null and [code]success[/code] is false. [br]
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
