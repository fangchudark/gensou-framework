## 常量
class_name Constants extends Object

## 命令关键字
class CommandKeywords:

	## 释放变量
	const Release: String = "release"

	## 变量
	const Var: String = "var"

	## 切换立绘
	const ChangeFigure: String = "changeFigure"

	## 切换头像
	const ChangePortrait: String = "changePortrait"

	## 切换背景
	const ChangeBackground: String = "changeBg"

	## 背景音乐
	const Bgm: String = "bgm"

	## 音效
	const Bgs: String = "bgs"

	## 音效
	const Se: String = "se"

	## 切换剧本
	const Call: String = "call"

	## 选择选项
	const Choose: String = "choose"

	## 返回标题
	const End: String = "end"

	## 隐藏或显示对话框
	const SetTextbox: String = "setTextbox"

	## 所有命令关键字
	const AllCommandKeywords: PackedStringArray = [
		Release,
		Var,
		ChangeFigure,
		ChangePortrait,
		ChangeBackground,
		Bgm,
		Bgs,
		Se,
		Call,
		Choose,
		End,
		SetTextbox
	]

## 可选参数关键字
class ParamKeywords:

	## 条件分支
	const When: String = "when"

	## 立即执行下一条语句
	const Next: String = "next"

	## 立绘位置-左
	const Left: String = "left"

	## 立绘位置-右
	const Right: String = "right"

	## 立绘位置-中
	const Center: String = "center"

	## 透明度
	const Alpha: String = "alpha"

	## 音量
	const Volume: String = "volume"

	## 淡入淡出
	const Fade: String = "enter"

	## 声音
	const Voice: String = "voice"

	## 全局变量
	const Global: String = "global"

	## 行数
	const Line: String = "line"

	## 字体大小
	const FontSize: String = "fontSize"

	## 所有可选参数关键字
	const AllParamKeywords: PackedStringArray = [
		When,
		Next,
		Left,
		Right,
		Center,
		Alpha,
		Volume,
		Fade,
		Voice,
		Global,
		Line,
		FontSize
	]

	## 带值可选参数关键字
	const AllParamWithValueKeywords: PackedStringArray = [
		When,
		Alpha,
		Volume,
		Fade,
		Voice,
		Line,
		FontSize
	]

## 关键字辅助类
class KeywordsHelper:

	## 判断输入是否是任一命令关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回匹配的结果
	static func equals_any_command_keyword(input: String) -> bool:
		return CommandKeywords.AllCommandKeywords.find(input) >= 0

	## 从字符串中获取第一个匹配的命令关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回匹配的结果
	static func get_command_keyword(input: String) -> String:
		if CommandKeywords.AllCommandKeywords.find(input) >= 0:
			return input
		return ""

	## 判断输入是否是任一可选参数关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回匹配的结果
	static func equals_any_param_keyword(input: String) -> bool:
		return ParamKeywords.AllParamKeywords.find(input) >= 0

	## 从字符串中获取第一个匹配的可选参数关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回匹配的结果
	static func get_param_keyword(input: String) -> String:
		if ParamKeywords.AllParamKeywords.find(input) >= 0:
			return input
		return ""

	## 判断输入是否是带值可选参数关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回匹配的结果
	static func equals_any_param_with_value_keyword(input: String) -> bool:
		return ParamKeywords.AllParamWithValueKeywords.find(input) >= 0

	## 从字符串中获取第一个匹配的带值可选参数关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回匹配的结果
	static func get_param_with_value_keyword(input: String) -> String:
		if ParamKeywords.AllParamWithValueKeywords.find(input) >= 0:
			return input
		return ""

	## 判断输入是否是带值的可选参数关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回匹配的结果
	static func is_param_with_value_keyword(input: String) -> bool:
		return (
				input.contains("=") and 
				ParamKeywords.AllParamWithValueKeywords
				.find(input.split("=")[0].strip_edges()) >= 0
		)

	## 判断输入是否是指定带值的可选参数关键字 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## [param keyword] 指定的关键字 [br]
	## 返回匹配的结果
	static func is_param_with_value_keyword_by_keyword(input: String, keyword: String) -> bool:
		return (
				input.contains("=") and 
				ParamKeywords.AllParamWithValueKeywords.has(keyword) and
				input.split("=")[0].strip_edges() == keyword
		)

	## 从字符串中获取带值可选参数的值 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## 返回获取的结果，失败时返回空字符串
	static func get_param_value(input: String) -> String:
		if not input.is_empty() and input.contains("="):
			return input.substr(input.find("=") + 1).strip_edges() 
		else:
			return ""
	
	## 从字符串中获取指定带值的可选参数的值 [br]
	## [br]
	## [param input] 输入字符串 [br]
	## [param keyword] 指定的关键字 [br]
	## 返回获取的结果，失败时返回空字符串
	static func get_param_value_by_keyword(input: PackedStringArray, keyword: String) -> String:
		for i in input:
			if is_param_with_value_keyword_by_keyword(i, keyword):
				return get_param_value(i)
		return ""
