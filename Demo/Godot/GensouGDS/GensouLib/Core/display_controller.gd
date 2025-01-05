## 文字显示控制器
class_name DisplayController extends VisualNoveCore

## 显示的文字
@export var text_to_display: Label

## 是否在打字
var is_typing: bool = false

var _cancellation_token: CancellationToken

## 显示一行文字 [br]
## [br]
## [param text] : [br]
## 要显示的文字
func display_line(text: String) -> void:
	if _cancellation_token:
		_cancellation_token.cancel()
	_cancellation_token = CancellationToken.new()
	await _type_text(text, _cancellation_token)

func _type_text(text: String, token: CancellationToken) -> void:
	is_typing = true
	text_to_display.text = text
	text_to_display.visible_characters = 0
	for i in range(text.length()):
		if token.is_cancelled(): return
		text_to_display.visible_characters = i + 1

		if on_skiping: await get_tree().create_timer(0.01).timeout
		else: await get_tree().create_timer(text_display_speed).timeout

	is_typing = false
	
	if on_auto_play and not on_skiping and not ChoiceInterpreter.on_choosing:
		await get_tree().create_timer(auto_play_interval).timeout
		BaseInterpreter.execute_next_line()
	elif on_skiping and not ChoiceInterpreter.on_choosing:
		BaseInterpreter.execute_next_line()

## 停止打字，显示完整的对话
func display_complete_line() -> void:
	if is_typing and _cancellation_token:
		_cancellation_token.cancel()
	if text_to_display.text != DialogueInterpreter.current_dialogue:
		text_to_display.text = DialogueInterpreter.current_dialogue
	text_to_display.visible_characters = -1
	is_typing = false
	if on_auto_play and not ChoiceInterpreter.on_choosing: BaseInterpreter.execute_next_line()
	elif on_skiping and not ChoiceInterpreter.on_choosing: BaseInterpreter.execute_next_line()
