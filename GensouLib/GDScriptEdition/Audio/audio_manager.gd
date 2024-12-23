## 音频管理器 [br]
## The audio manager.
##
## 因 GDScript 的特性，该类为匿名类，因此，请使用 Autoload 所定义的节点名来访问该类，而不是直接使用 AudioManager 类名。[br]
## Due to the nature of GDScript, this class is anonymous, so use the node name defined by Autoload to access it instead of using the AudioManager class name directly.[br]
## 提供播放背景音乐，环境音，音效与语音的功能。[br]
## Provides the ability to play background music, background sound, sound effects, and voice.[br]
## 提供淡出音频和淡出并播放新背景音乐的功能。[br]
## Provides the ability to fade out audio and fade out and play new background music.[br]
## 设置音频时应使用音频文件作为音频流，而不是AudioStreamPlaylist等资源文件。[br]
## When setting up audio, you should use an audio file as the audio stream, not an AudioStreamPlaylist resource file or something like that.
extends Node

## 淡出并切换背景音乐完成时发出 [br]
## Emitted when fading out and switching background music is completed
signal bgm_fade_completed_and_changed()

## 播放器淡入淡出完成时发出 [br]
## Emitted when the player's fade in or fade out is completed [br]
## [param player] 会是以下三个值：[br]
## [param player] will have the following three values：[br]
## - "bgm" [br]
## - "bgs" [br]
## - "voice" [br]
signal fade_completed(player: String)

## 资源路径，默认是 "res://Audio/"，将在该路径下加载音频文件。[br]
## Resource path, default is "res://Audio/" , which will load audio files under this path.
var res_path: String = "res://Audio/"

## 背景音乐播放器节点路径，默认是相对于根节点 "AudioManager" 的 "BGMPlayer" 。[br]
## BGM player node path, default to "BGMPlayer" relative to the root node "AudioManager".
var bgm_node_path: NodePath = "BGMPlayer"

## 环境音效播放器节点路径，默认是相对于根节点 "AudioManager" 的 "BGSPlayer" 。[br]
## BGS player node path, default to "BGSPlayer" relative to the root node "AudioManager".
var bgs_node_path: NodePath = "BGSPlayer"

## 音效播放器节点路径，默认是相对于根节点 "AudioManager" 的 "SFXPlayer" 。[br]
## SFX player node path, default to "SFXPlayer" relative to the root node "AudioManager".
var sfx_node_path: NodePath = "SFXPlayer"

## 语音播放器节点路径，默认是相对于根节点 "AudioManager" 的 "VoicePlayer" 。[br]
## Voice player node path, default to "VoicePlayer" relative to the root node "AudioManager".
var voice_node_path: NodePath = "VoicePlayer"

## 主音量，取决于默认的 Master 音频总线的音量，修改该属性即改变该音频总线的音量。[br]
## Master volume, depends on the default Master audio bus volume, modify this property to change the audio bus volume.
var master_volume: float:
	get:
		return max(db_to_linear((AudioServer.get_bus_volume_db(0))), 0.0)
	set(value):
		AudioServer.set_bus_volume_db(0, linear_to_db(value))

## 背景音乐音量，修改该属性即改变背景音乐的音量 [br]
## BGM volume, modify this property to change the BGM volume.
var bgm_volume: float:
	get:
		return max(db_to_linear(_bgm_player.volume_db), 0.0)
	set(value):
		_bgm_player.volume_db = linear_to_db(value)

## 环境音效音量，修改该属性即改变环境音效的音量 [br]
## BGS volume, modify this property to change the BGS volume.
var bgs_volume: float:
	get:
		return max(db_to_linear(_bgs_player.volume_db), 0.0)
	set(value):
		_bgs_player.volume_db = linear_to_db(value)

## 音效音量，修改该属性即改变音效的音量 [br]
## SFX volume, modify this property to change the SFX volume.
var sfx_volume: float:
	get:
		return max(db_to_linear(_sfx_player.volume_db), 0.0)
	set(value):
		_sfx_player.volume_db = linear_to_db(value)

## 语音音量，修改该属性即改变语音的音量 [br]
## Voice volume, modify this property to change the voice volume.
var voice_volume: float:
	get:
		return max(db_to_linear(_voice_player.volume_db), 0.0)
	set(value):
		_voice_player.volume_db = linear_to_db(value)

var _bgm_player: AudioStreamPlayer

var _bgs_player: AudioStreamPlayer

var _sfx_player: AudioStreamPlayer

var _voice_player: AudioStreamPlayer

## 只读属性，是否已静音 [br]
## Read-only property, whether the audio is muted.
var is_muted: bool:
	get:
		return _is_muted

var _is_muted: bool = false

var _original_bgm_volume: float

var _original_bgs_volume: float

var _original_sfx_volume: float

var _original_voice_volume: float

var _audio_pool: Dictionary

var _audio_file_extensions: PackedStringArray = ["ogg", "wav", "mp3"]

func _ready():
	_bgm_player = get_node(bgm_node_path)
	_bgs_player = get_node(bgs_node_path)
	_sfx_player = get_node(sfx_node_path)
	_voice_player = get_node(voice_node_path)
	if (
		not _bgm_player
		or not _bgs_player
		or not _sfx_player
		or not _voice_player
	):
		push_error("AudioManager: Could not find all required nodes")
		return
	_bgm_player.autoplay = true
	_bgm_player.playing = true
	_bgs_player.autoplay = false
	_sfx_player.autoplay = false
	_voice_player.autoplay = false

func _is_audio_file(file_name: String) -> bool:
	return _audio_file_extensions.has(file_name.get_extension())

func _play_audio(player: AudioStreamPlayer, file_name: String) -> void:
	if not _is_audio_file(file_name):
		push_error("AudioManager: Invalid audio file: " + file_name)
		return
	if not _audio_pool.has(file_name):
		var audio: AudioStream = ResourceLoader.load(res_path + file_name)
		if not audio:
			push_error("AudioManager: Could not load audio stream: " + file_name)
		_audio_pool[file_name] = audio

	player.stream = _audio_pool[file_name]
	player.play()

## 播放背景音乐 [br]
## Plays the background music.[br]
## 如需循环播放请在文件导入选项勾选循环选项 [br]
## To loop the music, check the "Loop" option in the file import options.[br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 [br]
## Audio file name, including extension, supports "ogg", "wav", "mp3" format.
func play_bgm(file_name: String) -> void: _play_audio(_bgm_player, file_name)

## 播放环境音效 [br]
## Plays the background sound.[br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 [br]
## Audio file name, including extension, supports "ogg", "wav", "mp3" format.
func play_bgs(file_name: String) -> void: _play_audio(_bgs_player, file_name)

## 播放语音 [br]
## Plays the voice.[br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 [br]
## Audio file name, including extension, supports "ogg", "wav", "mp3" format.
func play_voice(file_name: String) -> void: _play_audio(_voice_player, file_name)

## 播放音效 [br]
## Plays the sound effect.[br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 [br]
## Audio file name, including extension, supports "ogg", "wav", "mp3" format.
func play_sfx(file_name: String) -> void: _play_audio(_sfx_player, file_name)

## 淡出当前背景音乐并播放新音乐 [br]
## Fade out the current background music and plays a new one.[br]
## [br]
## [param new_music_name] : [br]
## 新音乐文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 [br]
## New music file name, including extension, supports "ogg", "wav", "mp3" format.
## [br]
## [param duration] : [br]
## 淡出时间，单位为秒 [br]
## Fade out duration, in seconds.[br]
## [br]
## 返回 true 表示成功，false 表示失败 [br]
## Returns true if successful, false if failed.
func fade_out_and_play_new_music(new_music_name: String, duration: float) -> bool:
	if not _is_audio_file(new_music_name):
		push_error("AudioManager: Invalid audio file: " + new_music_name)
		return false

	var new_music: AudioStream

	if not _audio_pool.has(new_music_name):
		new_music = ResourceLoader.load(res_path + new_music_name)
	else:
		new_music = _audio_pool[new_music_name]

	if not new_music:
		push_error("AudioManager: Could not load audio stream: " + new_music_name)
		return false
	
	var start_volume: float = bgm_volume

	while bgm_volume > 0.0:
		var volume_step: float = start_volume * get_process_delta_time() / duration
		if is_nan(bgm_volume - volume_step) or bgm_volume - volume_step < 0.0:
			bgm_volume = 0.0
			break
		bgm_volume -= volume_step
		await get_tree().process_frame
	
	_bgm_player.stop()
	_bgm_player.stream = new_music
	bgm_volume = start_volume
	_bgm_player.play()
	bgm_fade_completed_and_changed.emit()
	return true

func _fade_audio(player: String, start_volume: float, set_volume: Callable, target_volume: float, duration: float) -> bool:
	if target_volume < 0.0:
		push_error("AudioManager: Invalid target volume:" + str(target_volume))
		return false
	
	var elapsed: float = 0.0

	while elapsed < duration:
		elapsed += get_process_delta_time()
		var new_volume: float = lerp(start_volume, target_volume, elapsed / duration)
		if is_nan(new_volume) or new_volume < 0.0:
			set_volume.call(target_volume)
			break
		set_volume.call(new_volume)
		await get_tree().process_frame

	set_volume.call(target_volume)
	fade_completed.emit(player)
	return true

## 淡入或淡出背景音乐 [br]
## Fade in or fade out the background music.[br]
## [br]
## [param target_volume] : [br]
## 目标音量 [br]
## Target volume.
## [br]
## [param duration] : [br]
## 淡出时间，单位为秒 [br]
## Fade out duration, in seconds.[br]
## [br]
## 在淡入或淡出完成后返回 true，目标音量为负数时返回 false[br]
## Returns true when the fade in or fade out is complete, false if the target volume is negative.
func fade_bgm(target_volume: float, duration: float) -> bool: return await _fade_audio("bgm", bgm_volume, func(value): bgm_volume = value, target_volume, duration)

## 淡入或淡出环境音效 [br]
## Fade in or fade out the background sound.[br]
## [br]
## [param target_volume] : [br]
## 目标音量 [br]
## Target volume.
## [br]
## [param duration] : [br]
## 淡出时间，单位为秒 [br]
## Fade out duration, in seconds.[br]
## [br]
## 在淡入或淡出完成后返回 true，目标音量为负数时返回 false[br]
## Returns true when the fade in or fade out is complete, false if the target volume is negative.
func fade_bgs(target_volume: float, duration: float) -> bool: return await _fade_audio("bgs", bgs_volume, func(value): bgs_volume = value, target_volume, duration)

## 淡入或淡出语音 [br]
## Fade in or fade out the voice.[br]
## [br]
## [param target_volume] : [br]
## 目标音量 [br]
## Target volume.
## [br]
## [param duration] : [br]
## 淡出时间，单位为秒 [br]
## Fade out duration, in seconds.[br]
## [br]
## 在淡入或淡出完成后返回 true，目标音量为负数时返回 false[br]
## Returns true when the fade in or fade out is complete, false if the target volume is negative.
func fade_voice(target_volume: float, duration: float) -> bool: return await _fade_audio("voice", voice_volume, func(value): voice_volume = value, target_volume, duration)

## 停止播放背景音乐 [br]
## Stops playing the background music.[br]
func stop_bgm() -> void: _bgm_player.stop()

## 停止播放环境音效 [br]
## Stops playing the background sound.[br]
func stop_bgs() -> void: _bgs_player.stop()

## 停止播放语音 [br]
## Stops playing the voice.[br]
func stop_voice() -> void: _voice_player.stop()

## 设置静音 [br]
## Sets the mute state.[br]
## [br]
## [param mute] : [br]
## 是否静音 [br]
## Whether to mute.
func set_mute(mute: bool) -> void:
	if mute:
		_original_bgm_volume = bgm_volume
		_original_bgs_volume = bgs_volume
		_original_sfx_volume = sfx_volume
		_original_voice_volume = voice_volume

		bgm_volume = 0.0
		bgs_volume = 0.0
		sfx_volume = 0.0
		voice_volume = 0.0
	elif _is_muted:
		bgm_volume = _original_bgm_volume
		bgs_volume = _original_bgs_volume
		sfx_volume = _original_sfx_volume
		voice_volume = _original_voice_volume
	
	_is_muted = mute
