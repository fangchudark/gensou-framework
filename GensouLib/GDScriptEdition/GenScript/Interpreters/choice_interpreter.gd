## 选择指令解释器
class_name ChoiceInterpreter extends BaseInterpreter

static var on_choosing: bool = false

static var _choice_texts: Array[String] = []

## 选项对应的目标脚本名
static var choice_targets: Array[String] = []

## 选项对应的目标脚本名所对应的执行行索引
static var choice_lines: Array[int] = []

## 解析选择指令 [br]
## [br]
## [param param] : [br]
## 参数
static func parse_choice_command(param: String) -> void:
	on_choosing = true
	var choices = param.split("|")

	for choice in choices:
		var text_and_target = choice.split("->", false)

		if text_and_target.size() > 3: continue
		var text = text_and_target[0].strip_edges()
		var story_or_line = text_and_target[1].strip_edges()
		var line_index = -1
		var parse_line_success = false

		if text_and_target.size() == 3:
			var result = try_parse_numeric(text_and_target[2])
			if result["success"]:
				parse_line_success = true
				line_index = result["result"]
			else:
				continue

		if text.is_empty(): continue

		if story_or_line.is_empty(): continue
		var story_and_line = story_or_line.split(":", false)

		_choice_texts.append(text)
		if story_and_line[0] != Constants.ParamKeywords.Line:
			choice_targets.append(story_or_line)
			if line_index < 0 and parse_line_success:
				choice_lines.append(0)
			elif parse_line_success:
				choice_lines.append(line_index)
			else:
				choice_lines.append(-1)
			continue
		elif story_and_line.size() == 2:
			var result = try_parse_numeric(story_and_line[1])
			if result["success"]:
				var line = result["result"]
				choice_targets.append(ScriptReader.current_script_name)
				if line < 0:
					choice_lines.append(0)
				choice_lines.append(line)
			else:
				choice_targets.append(ScriptReader.current_script_name)
				choice_lines.append(-1)
	
	VisualNoveCore.create_choice_buttons(PackedStringArray(_choice_texts), 15)
	_choice_texts.clear()

## 选择选项 [br]
## [br]
## [param index] : [br]
## 选项索引
static func select_choice(index: int) -> void:
	var target = choice_targets[index]
	var line = choice_lines[index]
	VisualNoveCore.clear_choice_buttons()
	if line == -1: return
	ScriptReader.read_and_execute(target, line)
	on_choosing = false
	choice_targets.clear()
	choice_lines.clear()
