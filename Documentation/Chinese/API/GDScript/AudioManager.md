# AudioManager

继承：[Node](https://docs.godotengine.org/zh-cn/stable/classes/class_node.html)

## 描述

音频管理器，用于控制音频播放。

因 GDScript 的特性，该类为匿名类，因此，请使用 Autoload 所定义的节点名来访问该类，而不是直接使用 `AudioManager` 类名。

该类所在的脚本应当被挂载到一个添加到自动加载的场景中。

你可以使用库中所提供的 AudioManager.tscn 场景，

或创建一个根节点为 `Node` 类型并且拥有四个 `AudioStreamPlayer` 类型的子节点场景。

播放音频时，应使用音频文件作为音频流，而不是 Godot 中像 `AudioStreamPlaylist` 这些继承自 `AudioStream` 的资源文件。

## 信号

|[bgm_fade_completed_and_changed](#audiomanagerbgm_fade_completed_and_changed)|淡出并切换背景音乐完成时发出|
|:---|:---|
|[fade_completed](#audiomanagerfade_completed)|播放器淡出淡入完成时发出|

## 属性

|[res_path](#audiomanagerres_path)|音频文件所在的路径|
|:---|:---|
|[bgm_node_path](#audiomanagerbgm_node_path)|背景音乐播放器的节点路径。|
|[bgs_node_path](#audiomanagerbgs_node_path)|环境音乐播放器的节点路径。|
|[sfx_node_path](#audiomanagersfx_node_path)|音效播放器的节点路径。|
|[voice_node_path](#audiomanagervoice_node_path)|语音播放器的节点路径.|
|[master_volume](#audiomanagermaster_volume)|主音量|
|[bgm_volume](#audiomanagerbgm_volume)|背景音乐音量|
|[bgs_volume](#audiomanagerbgs_volume)|环境音乐音量|
|[sfx_volume](#audiomanagersfx_volume)|音效音量|
|[voice_volume](#audiomanagervoice_volume)|语音音量|
|[is_muted](#audiomanageris_muted)|是否处于静音状态|

## 方法

|[play_bgm](#audiomanagerplay_bgm)|播放背景音乐。|
|:---|:---|
|[play_bgs](#audiomanagerplay_bgs)|播放环境音。|
|[play_voice](#audiomanagerplay_voice)|播放语音。|
|[play_sfx](#audiomanagerplay_sfx)|播放音效。|
|[fade_out_and_play_new_music](#audiomanagerfade_out_and_play_new_music)|淡出并播放新背景音乐。|
|[fade_bgm](#audiomanagerfade_bgm)|淡入或淡出背景音乐。|
|[fade_bgs](#audiomanagerfade_bgs)|淡入或淡出环境音。|
|[fade_voice](#audiomanagerfade_voice)|淡入或淡出语音。|
|[stop_bgm](#audiomanagerstop_bgm)|停止播放背景音乐。|
|[stop_bgs](#audiomanagerstop_bgs)|停止播放环境音。|
|[stop_voice](#audiomanagerstop_voice)|停止播放语音。|
|[set_mute](#audiomanagerset_mute)|设置是否静音。|

---

# AudioManager.bgm_fade_completed_and_changed

`signal bgm_fade_completed_and_changed()`

## 描述

淡出并切换背景音乐完成时发出。

在方法[fade_out_and_play_new_music](#audiomanagerfade_out_and_play_new_music)完成淡出并切换背景音乐后，会发出该信号。

---

# AudioManager.fade_completed

`signal fade_completed(player: String)`

## 描述

播放器淡出淡入完成时发出。

在方法[fade_bgm](#audiomanagerfade_bgm)、[fade_bgs](#audiomanagerfade_bgs)、[fade_voice](#audiomanagerfade_voice)完成淡入或淡出后，会发出该信号。

`player`参数为播放器的名称，会是以下值之一：

- `bgm` 

- `bgs`

- `voice`

---

# AudioManager.res_path

`var res_path: String`

## 描述

音频文件所在的路径，默认为 `res://Audio/`，将在该路径下加载音频文件。

---

# AudioManager.bgm_node_path

`var bgm_node_path: NodePath`

## 描述

用于播放背景音乐的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`BGMPlayer`。

---

# AudioManager.bgs_node_path

`var bgs_node_path: NodePath`

## 描述

用于播放环境音乐的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`BGSPlayer`。

---

# AudioManager.sfx_node_path

`var sfx_node_path: NodePath`

## 描述

用于播放音效的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`SFXPlayer`。

---

# AudioManager.voice_node_path

`var voice_node_path: NodePath`

## 描述

用于播放语音的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`VoicePlayer`。

---

# AudioManager.master_volume

`var master_volume: float`

## 描述

主音量，取决于默认的Master音频总线的音量。

修改此属性即改变该音频总线的音量。

---

# AudioManager.bgm_volume

`var bgm_volume: float`

## 描述

背景音乐音量。

修改此属性即改变用于播放背景音乐的`AudioStreamPlayer`节点的音量。

---

# AudioManager.bgs_volume

`var bgs_volume: float`

## 描述

环境音乐音量。

修改此属性即改变用于播放环境音乐的`AudioStreamPlayer`节点的音量。

---

# AudioManager.sfx_volume

`var sfx_volume: float`

## 描述

音效音量。

修改此属性即改变用于播放音效的`AudioStreamPlayer`节点的音量。

---

# AudioManager.voice_volume

`var voice_volume: float`

## 描述

语音音量。

修改此属性即改变用于播放语音的`AudioStreamPlayer`节点的音量。

---

# AudioManager.is_muted

`var is_muted: bool`

## 描述

只读属性，是否处于静音状态。

---

# AudioManager.play_bgm

`func play_bgm(file_name: String) -> void`

## 参数

|`file_name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放背景音乐的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.play_bgs

`func play_bgs(file_name: String) -> void`

## 参数

|`file_name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放环境音乐的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.play_voice

`func play_voice(file_name: String) -> void`

## 参数

|`file_name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放语音的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.play_sfx

`func play_sfx(file_name: String) -> void`

## 参数

|`file_name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放音效的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.fade_out_and_play_new_music

`func fade_out_and_play_new_music(new_music_name: String, duration: float) -> bool`

## 参数

|`new_music_name`|新音乐文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|
|`duration`|淡出时间，单位为秒。|

## 描述

可等待。

按指定时间淡出用于播放背景音乐的`AudioStreamPlayer`节点当前播放的背景音乐，并播放指定的新背景音乐。

## 返回

返回 `true` 表示成功，`false` 表示失败

---

# AudioManager.fade_bgm

`func fade_bgm(target_volume: float, duration: float) -> bool`

## 参数

|`target_volume`|目标音量（非负数）。|
|:---|:---|
|`duration`|淡入或淡出时间，单位为秒。|

## 描述

可等待。

按指定时间淡入或淡出用于播放背景音乐的`AudioStreamPlayer`节点的音量到目标音量。

## 返回

在淡出或淡入完成后返回 `true`，目标音量为负数时返回 `false`

---

# AudioManager.fade_bgs

`func fade_bgs(target_volume: float, duration: float) -> bool`

## 参数

|`target_volume`|目标音量（非负数）。|
|:---|:---|
|`duration`|淡入或淡出时间，单位为秒。|

## 描述

可等待。

按指定时间淡入或淡出用于播放环境音乐的`AudioStreamPlayer`节点的音量到目标音量。

## 返回

在淡出或淡入完成后返回 `true`，目标音量为负数时返回 `false`

---

# AudioManager.fade_voice

`func fade_voice(target_volume: float, duration: float) -> bool`

## 参数

|`target_volume`|目标音量（非负数）。|
|:---|:---|
|`duration`|淡入或淡出时间，单位为秒。|

## 描述

可等待。

按指定时间淡入或淡出用于播放语音的`AudioStreamPlayer`节点的音量到目标音量。

## 返回

在淡出或淡入完成后返回 `true`，目标音量为负数时返回 `false`

---

# AudioManager.stop_bgm

`func stop_bgm() -> void`

## 描述

停止播放用于播放背景音乐的`AudioStreamPlayer`节点正在播放的音频。

---

# AudioManager.stop_bgs

`func stop_bgs() -> void`

## 描述

停止播放用于播放环境音乐的`AudioStreamPlayer`节点正在播放的音频。

---

# AudioManager.stop_voice

`func stop_voice() -> void`

## 描述

停止播放用于播放语音的`AudioStreamPlayer`节点正在播放的音频。

---

# AudioManager.set_mute

`func set_mute(mute: bool) -> void`

## 参数

|`mute`|是否静音，`true`为静音，`false`为取消静音。|
|:---|:---|

## 描述

设置是否静音。