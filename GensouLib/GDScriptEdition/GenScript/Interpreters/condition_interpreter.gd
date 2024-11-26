## 条件命令解释器[br]
## Condition command interpreter 
## 
## 专门处理条件命令 (-when:)[br]
## This class specifically handles condition commands (-when:). [br]
## 通过解析条件表达式，控制命令的执行流程。[br]
## It parses condition expressions to control the execution flow of commands. 
class_name ConditionInterpreter extends BaseInterpreter


## 检查给定的条件表达式是否有效并返回其评估结果。[br]
## Check if the given condition expression is valid and return its evaluation result.[br]
## 该方法首先检查条件是否为空，并查找其中的有效运算符。接着，将条件表达式进行拆分并验证其合法性，[br]
## The method first checks if the condition is empty and looks for valid operators within it.Then it splits the condition expression and verifies its validity.[br]
## 如果条件表达式中包含合法的运算符，则会进一步评估该条件。如果条件不包含有效运算符，则直接评估为简单条件。[br]
## If the condition expression contains valid operators, it evaluates the condition further. If the condition does not contain valid operators, it directly evaluates it as a simple condition.[br]
## [br]
## [param condition] : [br]
## 需要检查的条件表达式。可以是包含运算符的条件表达式，也可以是简单的条件。[br]
## [br] 
## 如果条件表达式合法且满足条件，返回 true。[br]
## Returns true if the condition expression is valid and satisfies the condition.[br]
## 如果条件表达式为空或不合法，返回 false。[br]
## Returns false if the condition expression is empty or invalid.[br]
static func check_condition(condition: String) -> bool:
	# 检查条件是否为空
	if condition.strip_edges().is_empty():
		push_error("The condition expression is empty. Command ignored.(条件表达式为空，命令已忽略)")
		return false
	# 定义条件运算符
	var condition_operator: Array[String] = [
		">=",
		"<=",
		"==",
		"!=",
		">",
		"<"
	]
	# 查找运算符
	var geted_operator: String = _find_operator(condition, condition_operator)
	# 切割条件表达式
	var expression = condition.rsplit(geted_operator, false)
	# 检查表达式的合法性
	if not _is_valid_expression(expression, geted_operator, condition_operator):
		push_error("The condition expression is invalid. Command ignored. (条件表达式不合法，命令已忽略)")
		return false
	# 如果匹配到条件运算符
	if expression.size() > 0 and not geted_operator.is_empty():
		return _process_check_condition(expression, geted_operator)
	return _evaluate_simple_condition(condition) # 处理简单条件


# 处理简单的布尔条件，支持布尔值、布尔类型变量以及字符串的布尔解析。
# 该函数首先检查传入的条件是否为已定义的布尔类型变量，
# 然后尝试将字符串解析为布尔值，最后处理语法错误和无效的条件。
static func _evaluate_simple_condition(condition: String) -> bool:
	# 如果条件是已定义的变量，则检查其是否为布尔类型
	if VariableInterpreter.variable_list.has(condition):
		if VariableInterpreter.variable_list[condition] is bool:
			return VariableInterpreter.variable_list[condition]
		push_error("Variable type mismatch: expected Boolean. Command ignored.（变量类型错误，命令已忽略）")
	# 尝试将字符串解析为布尔值
	elif try_parse_str_to_bool(condition)["success"]:
		return try_parse_str_to_bool(condition)["result"]
	# 如果变量名存在语法上有效但未定义
	if not VariableInterpreter.variable_list.has(condition) and VariableInterpreter.check_variable_name(condition):
		push_error("Undefined variable: %s.（未定义的变量：%s）" % [condition,condition])
	# 防止使用仅包含数学表达式的条件
	if VariableInterpreter.check_expression(condition):
		push_error("A standalone mathematical expression is not a valid condition.（单独的数学表达式不被视为条件）")
	# 检测并警告意外的赋值操作
	if condition.contains("="):
		push_error("Unexpected assignment operation, if you expect an equal comparison, use \"==\".(意外的赋值操作，若期望进行等于比较，使用“==”)")
	# 最后，处理任何其他不符合条件的情况
	push_error("Invalid argument: %s. Expected: Boolean variable, Boolean value, or conditional expression. Command ignored.（无效的参数：%s，期望：布尔类型变量，布尔值或条件表达式。命令已忽略）" % [condition,condition])
	return false


static func _find_operator(condition: String, operators: Array[String]) -> String:
	for op in operators:
		if condition.contains(op):
			return op
	return "" # 找不到运算符，返回空字符串


# 验证条件表达式的合法性。
# 当指定了运算符时，对表达式的格式和内容进行进一步检查，判断其是否符合合法的条件格式。
static func _is_valid_expression(expression: PackedStringArray, geted_operator: String, operators: Array[String]) -> bool:
	# 若满足以下任一条件则视为无效表达式：
	if (not geted_operator.is_empty() # 运算符不为空的情况下才进行检查
	and (expression.size() != 2 # 表达式长度不等于2（必须包含左右两个操作数）
	or Array(expression).any(func(e):return e.strip_edges().is_empty()) # 表达式中存在空白元素（为空或仅包含空格）
	or expression[0].contains("=") # 左右操作数中不应包含等号
	or expression[1].contains("=") # 左右操作数中不应包含等号
	or Array(expression).any(func(e):return e.begins_with(geted_operator) or e.ends_with(geted_operator)) # 操作数不应以运算符开头或结尾
	or Array(expression).any(func(e):return operators.any(func(op):return e.contains(op)))) # 操作数中不应包含其他运算符
	):
		return false
	# 有效表达式，返回true
	return true


# 处理条件检查，通过比较给定表达式中的两个值（数值或变量）来确定条件是否满足。
# 如果两个值均为数值，则解析并计算它们的数值，再根据指定的操作符进行比较；
# 否则，将它们作为变量直接进行比较。
static func _process_check_condition(expression: PackedStringArray, op: String) -> bool:
	var left = expression[0].strip_edges() # 左操作数
	var right = expression[0].strip_edges() # 右操作数
	# 如果左右操作数均为数学表达式或数值以及数值类型变量，执行数值比较
	if _is_numeric_comparison(left, right):
		var result = _exaluate_numbers(left, right) # 解析左右操作数（可以为表达式或数值以及数值类型变量）
		if result["success"]:
			return _compare_values(result["left_value_to_compare"], result["right_value_to_compare"], op)
		return false # 无法解析为数值时，返回false
	# 如果左右操作数为变量，执行变量比较
	return _compare_variables(left, right, op)


# 判断给定的左右操作数是否应视为数值比较。
# 若两个操作数均为数值或表达式，或一方为数值/变量且另一方为表达式，则视为数值比较。
static func _is_numeric_comparison(left: String, right: String) -> bool:
	return ((try_parse_numeric(left)["success"] and try_parse_numeric(right)["success"]) # 左右都是数字或
	or (VariableInterpreter.check_expression(left) and VariableInterpreter.check_expression(right)) # 左右都是表达式或
	or (VariableInterpreter.check_expression(left) and (VariableInterpreter.check_variable_name(right) or try_parse_numeric(right)["success"])) # 左是表达式，右是数字或变量或
	or ((VariableInterpreter.check_variable_name(left) or try_parse_numeric(left)["success"]) and VariableInterpreter.check_expression(right)) # 左是数字或变量，右是表达式
	)


# 比较两个变量的值，根据左右操作数的类型（布尔、字符串、数值）执行不同的比较逻辑。
# 如果左右操作数都是有效变量：
#   - 若类型相同，则根据类型（布尔、字符串或数值）执行相应的比较并返回结果；
#   - 若类型不兼容，调用错误处理方法并返回 false。
# 如果其中一个或两个操作数不是有效变量，则按字符串或布尔值进行直接比较。
static func _compare_variables(left: String, right: String, op: String) -> bool:
	var left_value = VariableInterpreter.try_get_variable_value(left)
	var right_value = VariableInterpreter.try_get_variable_value(right)
	# 左右操作数都是有效变量
	if left_value["success"] and right_value["success"]:
		if left_value["result"] is bool and right_value["result"] is bool:
			return _compare_booleans(left_value["result"], right_value["result"], op) # 布尔值比较
		if left_value["result"] is String and right_value["result"] is String:
			return _compare_strings(left_value["result"], right_value["result"], op) # 字符串比较
		if (left_value["result"] is float or left_value["result"] is int) and (right_value["result"] is float or right_value["result"] is int):
			return _compare_values(left_value["result"], right_value["result"], op) # 数值比较
		# 类型不兼容，调用错误处理并返回false
		_handle_incompatible_comparsion(left, right, left_value["result"], right_value["result"])
		return false
	# 左右操作数包含非变量值处理
	return _evaluate_no_variable(left, right ,op)


# 对非变量的左右操作数进行比较。若左右操作数类型相同（字符串、布尔值或数值），则执行对应的比较；
# 如果左右操作数不符合这些条件，则将其视为包含变量的情况进一步处理。
static func _evaluate_no_variable(left: String, right: String, op: String) -> bool:
	# 左右是字符串
	if left.begins_with("\"") and left.ends_with("\"") and right.begins_with("\"") and right.ends_with("\""):
		return _compare_strings(left, right, op)
	var bool_value_left = try_parse_str_to_bool(left)
	var bool_value_right = try_parse_str_to_bool(right)
	# 左右都是布尔值
	if bool_value_left["success"] and bool_value_right["success"]:
		return _compare_booleans(bool_value_left["result"], bool_value_right["result"], op)
	var number_value_left = try_parse_numeric(left)
	var number_value_right = try_parse_numeric(right)
	# 左右都是数字
	if number_value_left["success"] and number_value_right["success"]:
		return _compare_values(number_value_left["result"], number_value_right["result"], op)
	# 左或右是变量
	return _only_one_side_is_variable(left, right, op)


# 检查是否仅有一侧操作数为变量，根据情况处理单侧为变量的比较
# 如果左右操作数都符合或其一符合变量名规则但未定义则判定为不存在的变量
# 最后处理类型不兼容的比较
static func _only_one_side_is_variable(left: String, right: String, geted_operator: String) -> bool:
	var var_left = VariableInterpreter.try_get_variable_value(left)
	# 左侧为已定义变量，右侧为非变量
	if var_left["success"] and not VariableInterpreter.check_variable_name(right):
		return _compare_variable_with_value(var_left["result"], right, left, geted_operator, "L")
	var var_right = VariableInterpreter.try_get_variable_value(right)
	# 右侧为已定义变量，左侧为非变量
	if not VariableInterpreter.check_variable_name(left) and var_right["success"]:
		return _compare_variable_with_value(var_right["result"], left, right, geted_operator, "R")
	# 如果左右操作数都符合或其一符合变量命名规则但未定义，报告不存在的变量
	if VariableInterpreter.check_variable_name(left) and VariableInterpreter.check_variable_name(right) and not var_left["success"] and not var_right["success"]:
		push_error("Undefined variables: %s and %s. Command ignored.（未定义的变量：%s 和 %s，命令已忽略）" % [left,right,left,right])
		return false
	elif VariableInterpreter.check_variable_name(left) and not var_left["success"]:
		push_error("Undefined variables: %s. Command ignored.（未定义的变量：%s ，命令已忽略）" % [left,left])
		return false
	elif VariableInterpreter.check_variable_name(right) and not var_right["success"]:
		push_error("Undefined variables: %s. Command ignored.（未定义的变量：%s ，命令已忽略）" % [right,right])
		return false
	# 最后，处理类型不兼容的比较
	_handle_incompatible_comparsion(left, right)
	return false


# 变量与不同类型的值的比较
# 根据不同的类型进入不同的分支
static func _compare_variable_with_value(variable: Variant, value: String, variable_name: String, op: String, side: String) -> bool:
	match variable:
		var bool_value when bool_value is bool:
			return _compare_boolean_with_value(bool_value, value, variable_name, op, side)
		var string_value when string_value is String:
			return _compare_string_with_value(string_value, value, variable_name, op, side)
		var number_value:
			return _compare_number_with_value(number_value, value, variable_name, op, side)

# 比较一个布尔变量与一个非变量操作数的值
# 如果非变量操作数不能被解析为布尔值，则根据变量操作数的位置（左侧或右侧）报告不兼容的比较
static func _compare_boolean_with_value(bool_value: bool, value: String, vaeiable_name: String, op: String, side: String) -> bool:
	var result = try_parse_str_to_bool(value) # 尝试将非变量操作数解析为布尔值
	if result["success"]: 
		return _compare_booleans(bool_value, result["result"], op)
	# 如果非变量操作数无法解析为布尔值，根据变量的位置处理类型不兼容的比较
	if side == "L": # 左侧为变量时类型不兼容
		_handle_incompatible_comparsion(vaeiable_name, value, bool_value)
	else: # 右侧为变量时类型不兼容
		_handle_incompatible_comparsion(vaeiable_name, value, null, bool_value)
	return false # 类型不兼容，返回false


# 比较一个字符串变量与一个非变量操作数的值
# 如果非变量操作数不被双引号包裹，则根据变量操作数的位置（左侧或右侧）报告不兼容的比较
static func _compare_string_with_value(string_value: String, value: String, variable_name: String, op: String, side: String) -> bool:
	if value.begins_with("\"") and value.ends_with("\""): # 判定非变量操作数是否被双引号包裹
		return _compare_strings(string_value, value.trim_prefix("\"").trim_suffix("\""), op)
	# 如果非变量操作数不被双引号包裹，根据变量的位置处理类型不兼容的比较
	if side == "L": # 左侧为变量
		_handle_incompatible_comparsion(variable_name, value, string_value)
	else: # 右侧为变量
		_handle_incompatible_comparsion(variable_name, value, null, string_value)
	return false # 类型不兼容，返回false


# 比较一个数值类型变量与一个非变量操作数的值
# 如果非变量操作数不能被解析为数值，则根据变量操作数位置（左侧或右侧）报告不兼容的比较
static func _compare_number_with_value(number_value: Variant, value: String, variable_name: String, op: String, side: String) -> bool:
	var result = try_parse_numeric(value) # 尝试将非变量操作数解析为数值
	if result["success"]:
		return _compare_values(number_value, result["result"], op)
	# 如果非变量操作数无法解析为数值，根据变量的位置处理类型不兼容的比较
	if side == "L": # 左侧为变量
		_handle_incompatible_comparsion(variable_name, value, number_value)
	else: # 右侧为变量
		_handle_incompatible_comparsion(variable_name, value, null, number_value)
	return false # 类型不兼容，返回false


# 左右操作数进行比较时类型不兼容
# 根据操作数的类型以及是否为变量输出对应的错误信息
static func _handle_incompatible_comparsion(left: String, right: String, left_value: Variant = null, right_value: Variant = null):
	# 左右操作数符合变量名规则但未定义
	if not VariableInterpreter.try_get_variable_value(left)["success"] and not VariableInterpreter.try_get_variable_value(right)["success"] and VariableInterpreter.check_variable_name(left) and VariableInterpreter.check_variable_name(right):
		push_error("Variables %s and %s do not exist. Command ignored.(变量 %s 和 %s 不存在，命令已忽略)" % [left,right])
		return
	# 左右操作数是布尔间数值的比较
	if (try_parse_str_to_bool(left)["success"] and try_parse_numeric(right)["success"]) or (try_parse_numeric(left)["success"] and try_parse_str_to_bool(right)["success"]):
		push_error("Cannot compare Boolean value and numeric value. Command ignored.(不能将布尔值和数值比较，命令已忽略)")
		return
	# 数值与字符串的比较
	if (try_parse_numeric(left)["success"] and right.begins_with("\"") and right.ends_with("\"")) or (try_parse_numeric(right)["success"] and left.begins_with("\"") and left.ends_with("\"")):
		push_error("Cannot compare numeric value and string. Command ignored.(不能将数值和字符串比较，命令已忽略)")
		return
	# 布尔值与字符串的比较
	if (try_parse_str_to_bool(left)["success"] and right.begins_with("\"") and right.ends_with("\"")) or (try_parse_str_to_bool(right)["success"] and left.begins_with("\"") and left.ends_with("\"")):
		push_error("Cannot compare Boolean value and string. Command ignored.(不能将布尔值和字符串比较，命令已忽略)")
		return
	# 左侧是布尔变量，右侧是字符串变量
	if left_value is bool and right_value is String:
		push_error("Cannot compare string type variable: %s with a Boolean type variable: %s. Command ignored.(不能将字符串类型变量：%s 用以和布尔类型变量：%s 比较，命令已忽略)" % [right,left,right,left])
		return
	# 左侧字符串变量，右侧是布尔变量
	if left_value is String and right_value is bool:
		push_error("Cannot compare string type variable: %s with a Boolean type variable: %s. Command ignored.(不能将字符串类型变量：%s 用以和布尔类型变量：%s 比较，命令已忽略)" % [left,right,left,right])
		return
	# 左侧是布尔变量，右侧是其他类型的非变量操作数
	if left_value is bool:
		_handle_boolean_incompatible_comparsion(left, right)
		return
	# 左侧是数值类型变量，右侧是其他类型的非变量操作数
	if left_value is float or left_value is int:
		_handle_numeric_incompatible_comparsion(left, right)
		return
	# 左侧是字符串变量，右侧是其他类型的非变量操作数
	if left_value is String:
		_handle_string_incompatible_comparsion(left, right)
		return
	# 右侧是布尔变量，左侧是其他类型的非变量操作数
	if right_value is bool:
		_handle_boolean_incompatible_comparsion(right, left)
		return
	# 右侧是数值类型变量，左侧是其他类型的非变量操作数
	if right_value is float or right_value is int:
		_handle_numeric_incompatible_comparsion(right, left)
		return
	# 右侧是字符串变量，左侧是其他类型的非变量操作数
	if right_value is String: 
		_handle_string_incompatible_comparsion(right, left)
		return
	# 其他的意外情况
	push_error("Unexpected operand in conditional expression %s and %s, expected: Numbers and mathematical expressions, values of consistent types, number variables and mathematical expressions. Command ignored.(条件表达式意外的操作数：%s 和 %s ，期望：数值和数学表达式，类型一致的值，数值类型变量和数学表达式。命令已忽略)" % [left,right,left,right])
	return


# 布尔值变量与其他类型的非变量操作数的不兼容比较
static func _handle_boolean_incompatible_comparsion(variable: String, other: String) -> void:
	if try_parse_numeric(other)["success"]: # 布尔变量与数值比较
		push_error("Cannot compare Boolean type variable: %s with a numeric value. Command ignored.(不能将布尔类型变量： %s 用以和数值比较，命令已忽略)" % [variable,variable])
	elif not try_parse_str_to_bool(other)["success"]: # 布尔变量与字符串比较（无法被解析为布尔值则视作字符串）
		if other.begins_with("\"") and other.ends_with("\""):
			push_error("Cannot compare Boolean type variable: %s with a string. Command ignored.(不能将布尔类型变量： %s 用以和字符串比较，命令已忽略)" % [variable,variable])
		else:
			push_error("When doing comparison, you need to use \"\" to indicate the string to avoid confusion with the variable. However, you cannot compare numeric value variable: %s with a string. Command ignored.(进行比较时需要使用\"\"指明字符串，以免和变量混淆，但是不能将数值类型变量： %s 用以和字符串比较，命令已忽略)" % [variable,variable])
	push_error("Unexpected operand: %s .(意外的操作数：%s )" % [other,other]) # 指明意外的操作数


# 数值类型变量与其他类型非变量操作数的不兼容比较
static func _handle_numeric_incompatible_comparsion(variable: String, other: String) -> void:
	if try_parse_str_to_bool(other)["success"]: # 数值类型变量与布尔值比较
		push_error("Cannot compare numeric value variable: %s with a Boolean value. Command ignored.(不能将数值类型变量： %s 用以和布尔值比较，命令已忽略)" % [variable,variable])
	elif not try_parse_numeric(other)["success"]: # 数值类型变量与字符串比较（无法被解析为数值则视作字符串）
		if other.begins_with("\"") and other.ends_with("\""):
			push_error("Cannot compare numeric value variable: %s with a string. Command ignored.(不能将数值类型变量： %s 用以和字符串比较，命令已忽略)" % [variable,variable])
		else:
			push_error("When doing comparison, you need to use \"\" to indicate the string to avoid confusion with the variable. However, you cannot compare numeric value variable: %s with a string. Command ignored.(进行比较时需要使用\"\"指明字符串，以免和变量混淆，但是不能将数值类型变量： %s 用以和字符串比较，命令已忽略)" % [variable,variable])
	push_error("Unexpected operand: %s .(意外的操作数：%s )" % [other,other]) # 指明意外的操作数


# 字符串变量与其他类型操作数的不兼容比较
static func _handle_string_incompatible_comparsion(variable: String, other: String) -> void:
	if try_parse_str_to_bool(other)["success"]: # 字符串变量与布尔值比较
		push_error("Cannot compare string type variable: %s with a Boolean value. Command ignored.(不能将字符串类型变量： %s 用以和布尔值比较，命令已忽略)" % [variable,variable])
	elif try_parse_numeric(other)["success"]: # 字符串变量与数值比较
		push_error("Cannot compare string type variable: %s with a numeric value. Command ignored.(不能将字符串类型变量： %s 用以和数值比较，命令已忽略)" % [variable,variable])
	else: # 未使用双引号的字符串比较
		push_error("When doing comparison, you need to use \"\" to indicate the string to avoid confusion with the variable. Command ignored.(进行比较时需要使用\"\"指明字符串，以免和变量混淆，命令已忽略)")
	push_error("Unexpected operand: %s .(意外的操作数：%s )" % [other,other]) # 指明意外的操作数


# 处理数值比较
# 尝试将左右操作数解析为数值。
# 如果解析成功，字典字段"success"返回true，"left_value_to_compare"和"right_value_to_compare"返回解析后的值；
# 否则字典字段"success"返回false，其他两个字段返回null。
static func _exaluate_numbers(left: String, right: String) -> Dictionary:
	var left_value = _try_get_expression_value(left) # 解析左表达式
	if left_value["success"]:
		var right_value = _try_get_expression_value(right) # 解析右表达式
		if right_value["success"]: 
			return {
				"left_value_to_compare":left_value["result"],
				"right_value_to_compare":right_value["result"],
				"success":true
			}
	return {
		"left_value_to_compare":null,
		"right_value_to_compare":null,
		"success":false
	}


# 尝试解析表达式
# 如果是合法数学表达式则尝试解析它，
# 如果解析成功，字典字段"success"返回true，"result"返回解析后的值，
# 解析失败则输出错误信息并且字典字段"success"返回false，"result"返回null
# 否则，如果是已定义变量且是数值则返回true以及变量值
# 否则，如果是数字则返回true以及该数字
# 最后根据传入的表达式输出对应错误提示并返回false
static func _try_get_expression_value(expression: String) -> Dictionary:
	var var_is_number = true  # 默认为数值类型标志
	var get_variable_result = VariableInterpreter.try_get_variable_value(expression)  # 尝试获取变量的值
	var parse_numeric_result = try_parse_numeric(expression)  # 尝试解析为数值

	# 如果表达式不为空且符合表达式格式
	if not expression.is_empty() and VariableInterpreter.check_expression(expression):
		var postfix = VariableInterpreter.intfix_to_postfix(expression)  # 将中缀表达式转换为后缀表达式
		var result = VariableInterpreter.evaluate_postfix(postfix)  # 计算后缀表达式的值
		var parse_result = try_parse_numeric(result)  # 尝试解析计算结果为数值
		if parse_result["success"]:  # 如果解析成功，返回结果
			return {
				"result": parse_result["result"],
				"success": true
			}
		push_error("Unable to parse the expression: %s. Command ignored." % [expression])  # 无法解析表达式时的错误处理
		return {
			"result": null,
			"success": false
		}

	elif get_variable_result["success"]:  # 如果变量存在且成功获取其值
		if get_variable_result["result"] is float or get_variable_result["result"] is int:  # 检查变量类型是否为数值
			return {
				"result": get_variable_result["result"],
				"success": true
			}
		var_is_number = false  # 如果变量不是数值类型，标记为非数值类型
	elif parse_numeric_result["success"]:  # 如果表达式成功解析为数值
		return {
			"result": parse_numeric_result["result"],
			"success": true
		}

	if not var_is_number:  # 如果表达式涉及的变量不是数值类型，给出错误提示
		push_error("Only numeric value variables can be compared with mathematical expressions. Command ignored.")
	
	# 如果变量不存在或无法解析为数值，给出错误提示
	push_error("Numeric value variable: %s does not exist. Command ignored." % [expression])
	return {
		"result": null,
		"success": false
	}



# 字符串间的比较
static func _compare_strings(left_value: String, right_value: String, comparison_operator: String) -> bool:
	match comparison_operator:
		"==":
			return left_value == right_value
		"!=":
			return left_value != right_value
		_:
			push_error("Operator: \"%s\" is not supported for string types. Only '==' and '!=' are valid. Command ignored.(运算符：\"%s\" 不支持字符串类型，只有 '==' 和 '!=' 是有效的，命令已忽略)" % [comparison_operator,comparison_operator])
			return false


# 布尔值间的比较
static func _compare_booleans(left_value: bool, right_value: bool, comparison_operator: String) -> bool:
	match comparison_operator:
		"==":
			return left_value == right_value
		"!=":
			return left_value != right_value
		_:
			push_error("Operator: \"%s\" is not supported for Boolean types. Only '==' and '!=' are valid. Command ignored.(运算符：\"%s\" 不支持布尔类型，只有 '==' 和 '!=' 是有效的，命令已忽略)" % [comparison_operator,comparison_operator])
			return false


# 数值间的比较
static func _compare_values(left_value: Variant, right_value: Variant, comparison_operator: String) -> bool:
	if left_value is int and right_value is int: # 左右操作数为整数
		match comparison_operator:
			">":
				return left_value > right_value
			"<":
				return left_value < right_value
			">=":
				return left_value >= right_value
			"<=":
				return left_value <= right_value
			"==":
				return left_value == right_value
			"!=":
				return left_value != right_value
			_:
				return false
	elif left_value is float and right_value is float: # 左右操作数为浮点数
		match comparison_operator:
			">":
				return left_value > right_value
			"<":
				return left_value < right_value
			">=":
				return left_value >= right_value
			"<=":
				return left_value <= right_value
			"==":
				return left_value == right_value
			"!=":
				return left_value != right_value
			_:
				return false
	elif left_value is int and right_value is float: # 左操作数为整数/右操作数为浮点
		match comparison_operator:
			">":
				return left_value > right_value
			"<":
				return left_value < right_value
			">=":
				return left_value >= right_value
			"<=":
				return left_value <= right_value
			"==":
				return left_value == right_value
			"!=":
				return left_value != right_value
			_:
				return false
	elif left_value is float and right_value is int: # 左操作数为浮点/右操作数为整数
		match comparison_operator:
			">":
				return left_value > right_value
			"<":
				return left_value < right_value
			">=":
				return left_value >= right_value
			"<=":
				return left_value <= right_value
			"==":
				return left_value == right_value
			"!=":
				return left_value != right_value
			_:
				return false
	else: # 类型不匹配，返回false
		return false
