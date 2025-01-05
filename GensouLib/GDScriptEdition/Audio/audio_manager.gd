## 音频管理器
class_name AudioManager extends Node

## 任一在节点树上的节点
static var manger: Node

static var _fade_bgm_thread: Thread

static var _fade_bgs_thread: Thread

static var _fade_voice_thread: Thread

static var _fade_bgm_cancellation_token: CancellationToken

static var _fade_bgs_cancellation_token: CancellationToken

static var _fade_voice_cancellation_token: CancellationToken

## 主音量，取决于默认的 Master 音频总线的音量，修改该属性即改变该音频总线的音量。
static var master_volume: float:
	get:
		return max(db_to_linear((AudioServer.get_bus_volume_db(0))), 0.0)
	set(value):
		AudioServer.set_bus_volume_db(0, linear_to_db(value))

## 背景音乐音量，修改该属性即改变背景音乐的音量
static var bgm_volume: float:
	get:
		if not bgm_player: return _bgm_volume
		else: return max(db_to_linear(bgm_player.volume_db), 0.0)
	set(value):
		if not bgm_player: _bgm_volume = value
		else: bgm_player.volume_db = linear_to_db(value)

static var _bgm_volume: float = 1.0

## 环境音效音量，修改该属性即改变环境音效的音量
static var bgs_volume: float:
	get:
		if not bgs_player: return _bgs_volume
		else: return max(db_to_linear(bgs_player.volume_db), 0.0)
	set(value):
		if not bgs_player: _bgs_volume = value
		else: bgs_player.volume_db = linear_to_db(value)

static var _bgs_volume: float = 1.0

## 音效音量，修改该属性即改变音效的音量
static var se_volume: float:
	get:
		if not se_player: return _se_volume
		else: return max(db_to_linear(se_player.volume_db), 0.0)
	set(value):
		if not se_player: _se_volume = value
		else: se_player.volume_db = linear_to_db(value)

static var _se_volume: float = 1.0

## 语音音量，修改该属性即改变语音的音量
static var voice_volume: float:
	get:
		if not voice_player: return _voice_volume
		else: return max(db_to_linear(voice_player.volume_db), 0.0)
	set(value):
		if not voice_player: _voice_volume = value
		else: voice_player.volume_db = linear_to_db(value)

static var _voice_volume: float = 1.0

## BGM播放器
static var bgm_player: AudioStreamPlayer

## BGS播放器
static var bgs_player: AudioStreamPlayer

## 音效播放器
static var se_player: AudioStreamPlayer

## 语音播放器
static var voice_player: AudioStreamPlayer

## 是否已静音
static var is_muted: bool:
	get:
		return _is_muted

static var _is_muted: bool = false

## 是否在淡入淡出
static var fading: bool:
	get:
		return _fading

static var _fading: bool = false

static var _original_bgm_volume: float

static var _original_bgs_volume: float

static var _original_se_volume: float

static var _original_voice_volume: float

static var _audio_pool: Dictionary

static var _audio_file_extensions: PackedStringArray = ["ogg", "wav", "mp3"]

static func init(_manger: Node) -> void:
	manger = _manger

static func _is_audio_file(file_name: String) -> bool:
	return _audio_file_extensions.has(file_name.get_extension())

static func _play_audio(player: AudioStreamPlayer, file_name: String) -> void:
	if not _is_audio_file(file_name):
		push_error("AudioManager: Invalid audio file: " + file_name)
		return
	if not _audio_pool.has(file_name):
		var audio: AudioStream = ResourceLoader.load(file_name)
		if not audio:
			push_error("AudioManager: Could not load audio stream: " + file_name)
		_audio_pool[file_name] = audio

	player.stream = _audio_pool[file_name]
	player.play()

## 播放背景音乐 [br]
## 如需循环播放请在文件导入选项勾选循环选项 [br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
static func play_bgm(file_name: String) -> void: _play_audio(bgm_player, file_name)

## 播放环境音效 [br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
static func play_bgs(file_name: String) -> void: _play_audio(bgs_player, file_name)

## 播放语音 [br]
## Plays the voice.[br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
static func play_voice(file_name: String) -> void: _play_audio(voice_player, file_name)

## 播放音效 [br]
## Plays the sound effect.[br]
## [br]
## [param file_name] : [br]
## 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 [br]
## Audio file name, including extension, supports "ogg", "wav", "mp3" format.
static func play_se(file_name: String) -> void: _play_audio(se_player, file_name)

# ## 淡出当前背景音乐并播放新音乐 [br]
# ## [br]
# ## [param new_music_name] : [br]
# ## 新音乐文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 [br]
# ## [br]
# ## [param duration] : [br]
# ## 淡出时间，单位为秒 [br]
# func fade_out_and_play_new_music(new_music_name: String, duration: float) -> Thread:
# 	if not _is_audio_file(new_music_name):
# 		push_error("AudioManager: Invalid audio file: " + new_music_name)
# 		return

# 	var new_music: AudioStream

# 	if not _audio_pool.has(new_music_name):
# 		new_music = ResourceLoader.load(new_music_name)
# 	else:
# 		new_music = _audio_pool[new_music_name]

# 	if not new_music:
# 		push_error("AudioManager: Could not load audio stream: " + new_music_name)
# 		return
	
# 	var start_volume: float = bgm_volume

# 	while bgm_volume > 0.0:
# 		var volume_step: float = start_volume * get_process_delta_time() / duration
# 		if is_nan(bgm_volume - volume_step) or bgm_volume - volume_step < 0.0:
# 			bgm_volume = 0.0
# 			break
# 		bgm_volume -= volume_step
# 		await get_tree().process_frame
	
# 	bgm_player.stop()
# 	bgm_player.stream = new_music
# 	bgm_volume = start_volume
# 	bgm_player.play()
# 	return

static func _fade_audio(start_volume: float, set_volume: Callable, target_volume: float, duration: float, token: CancellationToken) -> Thread:
	if not manger:
		push_error("AudioManager: Manager is null")
		return

	if target_volume < 0.0:
		push_error("AudioManager: Invalid target volume:" + str(target_volume))
		return
	
	var elapsed: float = 0.0

	while elapsed < duration:
		print("AudioManager: Fading " + str(start_volume) + " to " + str(target_volume) + " in " + str(duration) + " seconds, elapsed: " + str(elapsed) + " seconds")
		if token.is_cancelled(): return
		elapsed += manger.get_process_delta_time()
		var new_volume: float = lerp(start_volume, target_volume, elapsed / duration)
		if is_nan(new_volume) or new_volume < 0.0:
			set_volume.call(target_volume)
			break
		set_volume.call(new_volume)
		await manger.get_tree().process_frame

	set_volume.call(target_volume)
	return

## 淡入或淡出背景音乐 [br]
## [br]
## [param target_volume] : [br]
## 目标音量
## [br]
## [param duration] : [br]
## 淡出时间，单位为秒 
static func fade_bgm(target_volume: float, duration: float) -> void: 
	if _fade_bgm_cancellation_token and _fade_bgm_thread: # 如果有正在执行的线程
		_fade_bgm_cancellation_token.cancel() # 取消当前线程
		_fade_bgm_thread.wait_to_finish() # 释放当前线程资源
	_fade_bgm_thread = Thread.new() # 创建新的线程
	_fade_bgm_cancellation_token = CancellationToken.new() # 创建取消标记
	_fade_bgm_thread.start(_fade_audio.bind(bgm_volume, func(value): bgm_volume = value, target_volume, duration, _fade_bgm_cancellation_token))

## 淡入或淡出环境音效 [br]
## [br]
## [param target_volume] : [br]
## 目标音量 [br]
## [br]
## [param duration] : [br]
## 淡出时间，单位为秒 
static func fade_bgs(target_volume: float, duration: float) -> void: 
	if _fade_bgs_cancellation_token and _fade_bgs_thread: # 如果有正在执行的线程
		_fade_bgs_cancellation_token.cancel() # 取消当前线程
		_fade_bgs_thread.wait_to_finish() # 释放当前线程资源
	_fade_bgs_thread = Thread.new() # 创建新的线程
	_fade_bgs_cancellation_token = CancellationToken.new() # 创建取消标记
	_fade_bgs_thread.start(_fade_audio.bind(bgs_volume, func(value): bgs_volume = value, target_volume, duration, _fade_bgs_cancellation_token))

## 淡入或淡出语音 [br]
## [br]
## [param target_volume] : [br]
## 目标音量 [br]
## [br]
## [param duration] : [br]
## 淡出时间，单位为秒
static func fade_voice(target_volume: float, duration: float) -> void: 
	if _fade_voice_cancellation_token and _fade_voice_thread: # 如果有正在执行的线程
		_fade_voice_cancellation_token.cancel() # 取消当前线程
		_fade_voice_thread.wait_to_finish() # 释放当前线程资源
	_fade_voice_thread = Thread.new() # 创建新的线程
	_fade_voice_cancellation_token = CancellationToken.new() # 创建取消标记
	_fade_voice_thread.start(_fade_audio.bind(voice_volume, func(value): voice_volume = value, target_volume, duration, _fade_voice_cancellation_token))

## 停止播放背景音乐
static func stop_bgm() -> void: bgm_player.stop()

## 停止播放环境音效
static func stop_bgs() -> void: bgs_player.stop()

## 停止播放语音
static func stop_voice() -> void: voice_player.stop()

## 设置静音 [br]
## [br]
## [param mute] : [br]
## 是否静音
static func set_mute(mute: bool) -> void:
	if mute:
		_original_bgm_volume = bgm_volume
		_original_bgs_volume = bgs_volume
		_original_se_volume = se_volume
		_original_voice_volume = voice_volume

		bgm_volume = 0.0
		bgs_volume = 0.0
		se_volume = 0.0
		voice_volume = 0.0
	elif _is_muted:
		bgm_volume = _original_bgm_volume
		bgs_volume = _original_bgs_volume
		se_volume = _original_se_volume
		voice_volume = _original_voice_volume
	
	_is_muted = mute
