## 音频命令解释器
class_name AudioInterpreter extends BaseInterpreter

## 播放BGM命令 [br]
## [br]
## [param param] : [br]
## 参数 [br]
## [br]
## [param volume] : [br]
## 音量 [br]
## [br]
## [param duration] : [br]
## 淡入淡出时间
static func parse_bgm_command(param: String, volume: float = 1.0, duration: float = 0.0) -> void:
	_parse_audio_command(
		param,
		volume,
		duration,
		VisualNoveCore.bgm_path,
		AudioManager.fade_bgm,
		AudioManager.play_bgm,
		func(v):AudioManager.bgm_volume = v
	)

## 播放BGS命令 [br]
## [br]
## [param param] : [br]
## 参数 [br]
## [br]
## [param volume] : [br]
## 音量 [br]
## [br]
## [param duration] : [br]
## 淡入淡出时间
static func parse_bgs_command(param: String, volume: float = 1.0, duration: float = 0.0) -> void:
	_parse_audio_command(
		param,
		volume,
		duration,
		VisualNoveCore.vocal_path,
		AudioManager.fade_bgs,
		AudioManager.play_bgs,
		func(v):AudioManager.bgs_volume = v
	)

## 播放音效命令 [br]
## [br]
## [param param] : [br]
## 参数 [br]
## [br]
## [param volume] : [br]
## 音量
static func parse_se_command(param: String, volume: float = 1.0) -> void:
	_parse_audio_command_without_fade(
		param,
		volume,
		VisualNoveCore.vocal_path,
		func(v):AudioManager.se_volume = v,
		AudioManager.play_se
	)

## 播放语音命令 [br]
## [br]
## [param param] : [br]
## 参数 [br]
## [br]
## [param volume] : [br]
## 音量
static func parse_voice_command(param: String, volume: float = 1.0) -> void:
	_parse_audio_command_without_fade(
		param,
		volume,
		VisualNoveCore.vocal_path,
		func(v):AudioManager.voice_volume = v,
		AudioManager.play_voice
	)


static func _parse_audio_command_without_fade(
	param: String,
	volume: float,
	path_prefix: String,
	set_volume: Callable,
	play_audio: Callable
) -> void:
	var path: String = path_prefix.path_join(param)
	set_volume.call(volume)
	play_audio.call(path)

static func _parse_audio_command(
	param: String,
	volume: float,
	duration: float,
	path_prefix: String,
	fade_audio: Callable,
	play_audio: Callable,
	set_volume: Callable
) -> void:
	if param == "none":
		fade_audio.call(0.0, duration)
		return
	
	var path: String = path_prefix.path_join(param)

	if duration > 0.0:
		set_volume.call(0.0)
		play_audio.call(path)
		fade_audio.call(volume, duration)
	else:
		set_volume.call(volume)
		play_audio.call(path)
