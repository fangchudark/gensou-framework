## 对话解析器
class_name DialogueInterpreter extends BaseInterpreter

## 对话结构体
class Dialogue:
	## 说话者
	var speaker: String
	## 内容
	var content: String
	func _init(_speaker: String, _content: String):
		speaker = _speaker
		content = _content
	func _to_string():
		return content if speaker.is_empty() else speaker + ": " + content

## 当前说话者
static var current_speaker: String = ""

## 当前对话内容
static var current_dialogue: String = ""

## 解析对话 [br]
## [br]
## [param dialogue] :  [br] 
## 对话命令 [br]
## 返回对话结构体
static func parse_dialogue(dialogue: String) -> Dialogue:
	var regex = RegEx.new()
	regex.compile(r"(?<!\\)\|") # 没有被转义的竖线

	if dialogue.begins_with(":"):
		current_dialogue = replace_placeholders(regex.sub(dialogue.substr(1), "\n"))
		current_speaker = ""
		return Dialogue.new(current_speaker, current_dialogue)
	if not dialogue.contains(":"):
		current_dialogue = replace_placeholders(regex.sub(dialogue, "\n"))
		return Dialogue.new(current_speaker, current_dialogue)
	
	var colon_index = dialogue.find(":")
	current_speaker = replace_placeholders(dialogue.substr(0, colon_index))
	current_dialogue = replace_placeholders(regex.sub(dialogue.substr(colon_index + 1), "\n"))
	return Dialogue.new(current_speaker, current_dialogue)

## 解析立绘命令 [br]
## [br]
## [param param] : [br]
## 参数 [br]
## [br]
## [param alpha] : [br]
## 透明度 [br]
## [br]
## [param position] : [br]
## 位置
static func parse_figure_command(param: String, alpha: float = 1.0, position: ImageController.FigurePosition = ImageController.FigurePosition.Center) -> void:
	_parse_image_command(param, alpha, VisualNoveCore.figure_path, func(image, a, hide): ImageController.change_figure(image, a, position, hide), false)

## 解析头像命令 [br]
## [br]
## [param param] : [br]
## 参数 [br]
## [br]
## [param alpha] : [br]
## 透明度
static func parse_portrait_command(param: String, alpha: float = 1.0) -> void:
	_parse_image_command(param, alpha, VisualNoveCore.portrait_path, func(image, a, hide): ImageController.change_portrait(image, a, hide))

## 解析背景命令 [br]
## [br]
## [param param] : [br]
## 参数 [br]
## [br]
## [param alpha] : [br]
## 透明度
static func parse_background_command(param: String, alpha: float = 1.0) -> void:
	_parse_image_command(param, alpha, VisualNoveCore.background_path, func(image, a, _hide): ImageController.change_background(image, a), false)

static func _parse_image_command(param: String, alpha: float, path_prefix: String, change_image_action: Callable, is_hide_on_none: bool = true) -> void:
	if param == "none":
		# 移除精灵并隐藏图片
		change_image_action.call(null, alpha, is_hide_on_none)
		return

	var path: String = param
	path = path_prefix.path_join(param)  # 拼接路径
	var image: Texture2D = ResourceLoader.load(path)  # 加载资源
	if not image:
		push_error("Failed to load image %s (无法加载图片%s)." % path)
		return

	# 更新图片
	change_image_action.call(image, alpha, false)
