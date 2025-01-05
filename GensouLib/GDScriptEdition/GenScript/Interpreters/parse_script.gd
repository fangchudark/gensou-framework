## 解析脚本命令
class_name ParseScript extends Object

## 是否等待点击
static var wait_click: bool = true

## 是否脚本隐藏了文本框
static var script_hided_textbox = false

## 执行命令 [br]
## [br]
## [param raw] : [br]
## 原始命令字符串。
static func parse_command(raw: String) -> void:
	if raw == Constants.CommandKeywords.End:
		VisualNoveCore.game_manager_node.get_tree().change_scene_to_file(VisualNoveCore.title_scene_path)
		return

	wait_click = true # 重置等待点击状态
	var command: PackedStringArray = raw.split(" -")
	var command_and_param: String = command[0].strip_edges() # 命令与必选参数
	# 可选参数
	var optional_param: String = raw.substr(raw.find(" -")) if command.size() > 1 else ""

	# 查找关键字
	var keyword: String = "" # 关键字
	var command_param: String = "" # 命令参数
	var index: int = command_and_param.find(":") # 冒号索引
	# 有冒号没有关键字，keyword为空字符串；无冒号，command_param为空字符串
	if index == -1:
		keyword = command_and_param # 无冒号
	else:
		keyword = command_and_param.substr(0, index).strip_edges() # 有冒号
		command_param = command_and_param.substr(index + 1).strip_edges() # 命令参数

	
	# 临时调试用
	# print("keyword: " + keyword + " command_param: " + command_param + " optional_param: " + optional_param)

	if optional_param.is_empty():
		# 处理无可选参数命令
		match keyword:
			Constants.CommandKeywords.Var:
				_variable_command(command_param) # 变量
				BaseInterpreter.execute_next_line()
				return
			Constants.CommandKeywords.Release:
				VariableInterpreter.release_variable(command_param) # 释放变量
				BaseInterpreter.execute_next_line()
				return
			Constants.CommandKeywords.ChangeFigure:
				DialogueInterpreter.parse_figure_command(command_param)
			Constants.CommandKeywords.ChangePortrait:
				DialogueInterpreter.parse_portrait_command(command_param)
			Constants.CommandKeywords.ChangeBackground:
				DialogueInterpreter.parse_background_command(command_param)
			Constants.CommandKeywords.Bgm:
				AudioInterpreter.parse_bgm_command(command_param) # 背景音乐
				BaseInterpreter.execute_next_line()
				return
			Constants.CommandKeywords.Bgs:
				AudioInterpreter.parse_bgs_command(command_param) # 背景音效
				BaseInterpreter.execute_next_line()
				return
			Constants.CommandKeywords.Se:
				AudioInterpreter.parse_se_command(command_param) # 音效
				BaseInterpreter.execute_next_line()
				return
			Constants.CommandKeywords.Choose:
				ChoiceInterpreter.parse_choice_command(command_param) # 选择选项
				return
			Constants.CommandKeywords.Call:
				ScriptReader.read_and_execute(command_param) # 调用脚本
				return
			Constants.CommandKeywords.SetTextbox:
				_set_textbox(command_param) # 显示/隐藏文本框
				return
			_:
				VisualNoveCore.history.append(str(DialogueInterpreter.parse_dialogue(command_and_param)))
				VisualNoveCore.display_current_line()
				return
		return
	
	var param_keywords: PackedStringArray = optional_param.split(" -", false)

	# 临时调试用
	# print("param_keywords: " + ",".join(param_keywords))

	# 如果可选参数列表不为空，但是没有能够解析的关键字，则视作对话命令
	if (
		param_keywords.size() != 0 and 
		not Array(param_keywords).any(func(x): return Constants.KeywordsHelper.equals_any_param_keyword(x)) and
		not Array(param_keywords).any(func(x): return Constants.KeywordsHelper.is_param_with_value_keyword(x)) and 
		not Constants.KeywordsHelper.equals_any_command_keyword((keyword))
	):
		# 临时调试用
		# print("cannot parse command: " + command_and_param)
		# print("EqualsAnyCommandKeyWord: " + str(Array(param_keywords).any(func(x): return Constants.KeywordsHelper.equals_any_param_keyword(x))))
		# print("IsParamWithValueKeyWord: " + str(Array(param_keywords).any(func(x): return Constants.KeywordsHelper.is_param_with_value_keyword(x))))
		# print("EqualsAnyCommandKeyWord: " + str(Constants.KeywordsHelper.equals_any_command_keyword((keyword))))

		DialogueInterpreter.parse_dialogue(command_and_param)
		VisualNoveCore.display_current_line()
		return

	# 处理条件参数
	if Array(param_keywords).any(func(keyWord): return keyWord.begins_with(Constants.ParamKeywords.When) and not _process_condition_command(keyWord)):
		# 临时调试用
		# print("condition not met: " + command_and_param)
		
		BaseInterpreter.execute_next_line()
		return

	# 处理透明度参数
	var default_alpha: float = 1.0
	if (Array(param_keywords).any(func(keyWord): return keyWord.begins_with(Constants.ParamKeywords.Alpha))):
		var result = BaseInterpreter.try_parse_numeric(
			Constants.KeywordsHelper.get_param_value_by_keyword(
				param_keywords, 
				Constants.ParamKeywords.Alpha
			)
		)
		if result["success"]:
			default_alpha = result["result"]

	# 处理音量参数
	var default_volume: float = 1.0
	if (Array(param_keywords).any(func(keyWord): return keyWord.begins_with(Constants.ParamKeywords.Volume))):
		var result = BaseInterpreter.try_parse_numeric(
			Constants.KeywordsHelper.get_param_value_by_keyword(
				param_keywords, 
				Constants.ParamKeywords.Volume
			)
		)
		if result["success"]:
			default_volume = result["result"]

	# 处理淡入淡出时间参数
	var default_fade_time: float = 0.0
	if (Array(param_keywords).any(func(keyWord): return keyWord.begins_with(Constants.ParamKeywords.Fade))):
		var result = BaseInterpreter.try_parse_numeric(
			Constants.KeywordsHelper.get_param_value_by_keyword(
				param_keywords, 
				Constants.ParamKeywords.Fade
			)
		)
		if result["success"]:
			default_fade_time = result["result"]

	# 处理语音参数
	if (Array(param_keywords).any(func(keyWord): return keyWord.begins_with(Constants.ParamKeywords.Voice))):
		var voice_name: String = Constants.KeywordsHelper.get_param_value_by_keyword(param_keywords, Constants.ParamKeywords.Voice)
		AudioInterpreter.parse_voice_command(voice_name, default_volume) # 语音

	# 处理指定执行行参数
	var execute_line_index: int = 0
	if (Array(param_keywords).any(func(keyWord): return keyWord.begins_with(Constants.ParamKeywords.Line))):
		var result = BaseInterpreter.try_parse_numeric(
			Constants.KeywordsHelper.get_param_value_by_keyword(
				param_keywords, 
				Constants.ParamKeywords.Line
			)
		)
		if result["success"]:
			execute_line_index = result["result"]
			if (execute_line_index < 0 or 
				(execute_line_index == BaseInterpreter.current_line - 1 and
				command_param == ScriptReader.current_script_name)
			):
				execute_line_index = 0

	# 处理字体大小参数
	var default_font_size: int = 24
	if (Array(param_keywords).any(func(keyWord): return keyWord.begins_with(Constants.ParamKeywords.FontSize))):
		var result = BaseInterpreter.try_parse_numeric(
			Constants.KeywordsHelper.get_param_value_by_keyword(
				param_keywords, 
				Constants.ParamKeywords.FontSize
			)
		)
		if result["success"]:
			default_font_size = result["result"]
			if default_font_size < 1:
				default_font_size = 24
			VisualNoveCore.set_font_size(default_font_size)

	# 处理全局变量声明
	var is_global: bool = Array(param_keywords).any(func(keyWord): return keyWord == Constants.ParamKeywords.Global)

	# 检查是否执行完该行命令后立即执行下一条命令
	wait_click = not Array(param_keywords).any(func(keyWord): return keyWord == Constants.ParamKeywords.Next)

	# 临时调试用
	# print("WaitClick: " + str(wait_click))

	match keyword:
		Constants.CommandKeywords.Var:
			_variable_command(command_param, is_global) # 变量
			BaseInterpreter.execute_next_line()
			return
		Constants.CommandKeywords.Release:
			VariableInterpreter.release_variable(command_param) # 释放变量
			BaseInterpreter.execute_next_line()
			return
		Constants.CommandKeywords.ChangeFigure:
			if Array(param_keywords).any(func(keyWord): return keyWord == Constants.ParamKeywords.Left):
				DialogueInterpreter.parse_figure_command(command_param, default_alpha, ImageController.FigurePosition.Left)
			elif Array(param_keywords).any(func(keyWord): return keyWord == Constants.ParamKeywords.Right):
				DialogueInterpreter.parse_figure_command(command_param, default_alpha, ImageController.FigurePosition.Right)
			else:
				DialogueInterpreter.parse_figure_command(command_param, default_alpha, ImageController.FigurePosition.Center)
		Constants.CommandKeywords.ChangePortrait:
			DialogueInterpreter.parse_portrait_command(command_param, default_alpha)
		Constants.CommandKeywords.ChangeBackground:
			DialogueInterpreter.parse_background_command(command_param, default_alpha)
		Constants.CommandKeywords.Bgm:
			AudioInterpreter.parse_bgm_command(command_param, default_volume, default_fade_time)
			BaseInterpreter.execute_next_line()
		Constants.CommandKeywords.Bgs:
			AudioInterpreter.parse_bgs_command(command_param, default_volume, default_fade_time)
			BaseInterpreter.execute_next_line()
		Constants.CommandKeywords.Se:
			AudioInterpreter.parse_se_command(command_param, default_volume)
			BaseInterpreter.execute_next_line()
		Constants.CommandKeywords.Choose:
			ChoiceInterpreter.parse_choice_command(command_param) # 选择选项
			return
		Constants.CommandKeywords.Call:
			ScriptReader.read_and_execute(command_param) # 调用脚本
		Constants.CommandKeywords.SetTextbox:
			_set_textbox(command_param) # 显示/隐藏文本框
			return
		_:
			VisualNoveCore.history.append(str(DialogueInterpreter.parse_dialogue(command_and_param)))
			VisualNoveCore.display_current_line()

	if not wait_click or VisualNoveCore.on_auto_play or VisualNoveCore.on_skiping:
		BaseInterpreter.execute_next_line()

static func _process_condition_command(command: String) -> bool:
	var condition: String = Constants.KeywordsHelper.get_param_value(command)
	ConditionInterpreter.check_condition(condition)
	return ConditionInterpreter.check_condition(condition)


static func _variable_command(command: String, is_gloabl: bool = false) -> void:
	if command.length() > 0 and not command.contains("="):
		VariableInterpreter.handle_variable_declaration(command, is_gloabl)  
	elif command.contains("="):
		VariableInterpreter.handle_variable_assignment(command, is_gloabl)
	
static func _set_textbox(hide: String) -> void:
	if hide == "hide":
		VisualNoveCore.show_text_box(false)
		script_hided_textbox = true
	else:
		VisualNoveCore.show_text_box(true)
		script_hided_textbox = false
