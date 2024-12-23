# AudioManager

继承：[Node](https://docs.godotengine.org/zh-cn/stable/classes/class_node.html)

## 描述

单例，音频管理器，用于控制音频播放。

请使用`AudioManager.Instance`来访问该类的实例。

该类所在的脚本应当被挂载到一个添加到自动加载的场景中。

你可以使用库中所提供的 AudioManager.tscn 场景，

或创建一个根节点为 `Node` 类型并且拥有四个 `AudioStreamPlayer` 类型的子节点场景。

播放音频时，应使用音频文件作为音频流，而不是 Godot 中像 `AudioStreamPlaylist` 这些继承自 `AudioStream` 的资源文件。

## 静态属性

|[Instance](#audiomanagerinstance)|AudioManager的实例|
|:---|:---|

## 信号

|[BGMFadeCompletedAndChanged](#audiomanagerbgmfadecompletedandchanged)|淡出并切换背景音乐完成时发出|
|:---|:---|
|[FadeCompleted](#audiomanagerfadecompleted)|播放器淡出淡入完成时发出|

## 属性

|[ResPath](#audiomanagerrespath)|音频文件所在的路径|
|:---|:---|
|[BGMNodePath](#audiomanagerbgmnodepath)|背景音乐播放器的节点路径。|
|[BGSNodePath](#audiomanagerbgsnodepath)|环境音乐播放器的节点路径。|
|[SFXNodePath](#audiomanagersfxnodepath)|音效播放器的节点路径。|
|[VoiceNodePath](#audiomanagervoicenodepath)|语音播放器的节点路径.|
|[MasterVolume](#audiomanagermastervolume)|主音量|
|[BGMVolume](#audiomanagerbgmvolume)|背景音乐音量|
|[BGSVolume](#audiomanagerbgsvolume)|环境音乐音量|
|[SFXVolume](#audiomanagersfxvolume)|音效音量|
|[VoiceVolume](#audiomanagervoicevolume)|语音音量|
|[IsMuted](#audiomanagerismuted)|是否处于静音状态|

## 方法

|[PlayBGM](#audiomanagerplaybgm)|播放背景音乐。|
|:---|:---|
|[PlayBGS](#audiomanagerplaybgs)|播放环境音。|
|[PlayVoice](#audiomanagerplayvoice)|播放语音。|
|[PlaySFX](#audiomanagerplaysfx)|播放音效。|
|[FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic)|淡出并播放新背景音乐。|
|[FadeBGM](#audiomanagerfadebgm)|淡入或淡出背景音乐。|
|[FadeBGS](#audiomanagerfadebgs)|淡入或淡出环境音。|
|[FadeVoice](#audiomanagerfadevoice)|淡入或淡出语音。|
|[StopBGM](#audiomanagerstopbgm)|停止播放背景音乐。|
|[StopBGS](#audiomanagerstopbgs)|停止播放环境音。|
|[StopVoice](#audiomanagerstopvoice)|停止播放语音。|
|[SetMute](#audiomanagersetmute)|设置是否静音。|

---

# AudioManager.Instance

`public static AudioManager Instance`

## 描述

`AudioManager`的实例，通过访问它的属性和方法来控制音频播放。

---

# AudioManager.BGMFadeCompletedAndChanged

`public delegate void BGMFadeCompletedAndChangedEventHandler()`

## 描述

淡出并切换背景音乐完成时发出。

在方法[FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic)完成淡出并切换背景音乐后，会发出该信号。

---

# AudioManager.FadeCompleted

`public delegate void FadeCompletedEventHandler(string player)`

## 描述

播放器淡出淡入完成时发出。

在方法[FadeBGM](#audiomanagerfadebgm)、[FadeBGS](#audiomanagerfadebgs)、[FadeVoice](#audiomanagerfadevoice)完成淡入或淡出后，会发出该信号。

`player`参数为播放器的名称，会是以下值之一：

- `BGM` 

- `BGS`

- `Voice`

---

# AudioManager.ResPath

`public string ResPath`

## 描述

音频文件所在的路径，默认为 `res://Audio/`，将在该路径下加载音频文件。

---

# AudioManager.BGMNodePath

`public NodePath BGMNodePath`

## 描述

用于播放背景音乐的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`BGMPlayer`。

---

# AudioManager.BGSNodePath

`public NodePath BGSNodePath`

## 描述

用于播放环境音乐的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`BGSPlayer`。

---

# AudioManager.SFXNodePath

`public NodePath SFXNodePath`

## 描述

用于播放音效的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`SFXPlayer`。

---

# AudioManager.VoiceNodePath

`public NodePath VoiceNodePath`

## 描述

用于播放语音的`AudioStreamPlayer`节点的路径。

默认是相对于根节点的`VoicePlayer`。

---

# AudioManager.MasterVolume

`public float MasterVolume`

## 描述

主音量，取决于默认的Master音频总线的音量。

修改此属性即改变该音频总线的音量。

---

# AudioManager.BGMVolume

`public float BGMVolume`

## 描述

背景音乐音量。

修改此属性即改变用于播放背景音乐的`AudioStreamPlayer`节点的音量。

---

# AudioManager.BGSVolume

`public float BGSVolume`

## 描述

环境音乐音量。

修改此属性即改变用于播放环境音乐的`AudioStreamPlayer`节点的音量。

---

# AudioManager.SFXVolume

`public float SFXVolume`

## 描述

音效音量。

修改此属性即改变用于播放音效的`AudioStreamPlayer`节点的音量。

---

# AudioManager.VoiceVolume

`public float VoiceVolume`

## 描述

语音音量。

修改此属性即改变用于播放语音的`AudioStreamPlayer`节点的音量。

---

# AudioManager.IsMuted

`public bool IsMuted`

## 描述

只读属性，是否处于静音状态。

---

# AudioManager.PlayBGM

`public void PlayBGM(string name)`

## 参数

|`name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放背景音乐的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.PlayBGS

`public void PlayBGS(string name)`

## 参数

|`name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放环境音乐的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.PlayVoice

`public void PlayVoice(string name)`

## 参数

|`name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放语音的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.PlaySFX

`public void PlaySFX(string name)`

## 参数

|`name`|音频文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|

## 描述

在用于播放音效的`AudioStreamPlayer`节点上播放指定音频。

如需循环播放，请在文件导入选项中勾选循环选项。

---

# AudioManager.FadeOutAndPlayNewMusic

`public async Task<bool> FadeOutAndPlayNewMusic(string newMusicName, float duration)`

## 参数

|`newMusicName`|新音乐文件名，包含扩展名，支持`ogg`、`wav`、`mp3`格式。|
|:---|:---|
|`duration`|淡出时间，单位为秒。|

## 描述

按指定时间淡出用于播放背景音乐的`AudioStreamPlayer`节点当前播放的背景音乐，并播放指定的新背景音乐。

## 返回

异步任务，返回 `true` 表示成功，`false` 表示失败

---

# AudioManager.FadeBGM

`public async Task<bool> FadeBGM(float targetVolume, float duration)`

## 参数

|`targetVolume`|目标音量（非负数）。|
|:---|:---|
|`duration`|淡入或淡出时间，单位为秒。|

## 描述

按指定时间淡入或淡出用于播放背景音乐的`AudioStreamPlayer`节点的音量到目标音量。

## 返回

异步任务，在淡出或淡入完成后返回 `true`，目标音量为负数时返回 `false`

---

# AudioManager.FadeBGS

`public async Task<bool> FadeBGS(float targetVolume, float duration)`

## 参数

|`targetVolume`|目标音量（非负数）。|
|:---|:---|
|`duration`|淡入或淡出时间，单位为秒。|

## 描述

按指定时间淡入或淡出用于播放环境音乐的`AudioStreamPlayer`节点的音量到目标音量。

## 返回

异步任务，在淡出或淡入完成后返回 `true`，目标音量为负数时返回 `false`

---

# AudioManager.FadeVoice

`public async Task<bool> FadeVoice(float targetVolume, float duration)`

## 参数

|`targetVolume`|目标音量（非负数）。|
|:---|:---|
|`duration`|淡入或淡出时间，单位为秒。|

## 描述

按指定时间淡入或淡出用于播放语音的`AudioStreamPlayer`节点的音量到目标音量。

## 返回

异步任务，在淡出或淡入完成后返回 `true`，目标音量为负数时返回 `false`

---

# AudioManager.StopBGM

`public void StopBGM()`

## 描述

停止播放用于播放背景音乐的`AudioStreamPlayer`节点正在播放的音频。

---

# AudioManager.StopBGS

`public void StopBGS()`

## 描述

停止播放用于播放环境音乐的`AudioStreamPlayer`节点正在播放的音频。

---

# AudioManager.StopVoice

`public void StopVoice()`

## 描述

停止播放用于播放语音的`AudioStreamPlayer`节点正在播放的音频。

---

# AudioManager.SetMute

`public void SetMute(bool mute)`

## 参数

|`mute`|是否静音，`true`为静音，`false`为取消静音。|
|:---|:---|

## 描述

设置是否静音。