# AudioManager

## 描述

音频管理器，用于控制音频播放。

## 静态属性

|[MasterVolume](#audiomanagermastervolume)|主音量。|
|:---|:---|
|[BGMVolume](#audiomanagerbgmvolume)|背景音乐音量。|
|[BGSVolume](#audiomanagerbgsvolume)|环境音音量。|
|[SFXVolume](#audiomanagersfxvolume)|音效音量。|
|[VoiceVolume](#audiomanagervoicevolume)|语音音量。|
|[BGMSource](#audiomanagerbgmsource)|背景音乐音源。|
|[BGSSource](#audiomanagerbgssource)|环境音音源。|
|[SFXSource](#audiomanagersfxsource)|音效音源。|
|[VoiceSource](#audiomanagervoicesource)|语音音源。|
|[BGMClip](#audiomanagerbgmclip)|背景音乐音频。|
|[BGSClip](#audiomanagerbgsclip)|环境音音频。|
|[SFXClip](#audiomanagersfxclip)|音效音频。|
|[VoiceClip](#audiomanagervoiceclip)|语音音频。|
|[IsMuted](#audiomanagerismuted)|是否处于静音。|

## 静态方法

|[PlayBGM](#audiomanagerplaybgm)|播放背景音乐。|
|:---|:---|
|[PlayBGS](#audiomanagerplaybgs)|播放环境音。|
|[PlayVoice](#audiomanagerplayvoice)|播放语音。|
|[PlaySFX](#audiomanagerplaysfx)|播放音效。|
|[FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic)|淡出并播放新背景音乐。|
|[FadeBGM](#audiomanagerfadebgm)|淡出背景音乐。|
|[FadeBGS](#audiomanagerfadebgs)|淡出环境音。|
|[FadeVoice](#audiomanagerfadevoice)|淡出语音。|
|[StopBGM](#audiomanagerstopbgm)|停止播放背景音乐。|
|[StopBGS](#audiomanagerstopbgs)|停止播放环境音。|
|[StopVoice](#audiomanagerstopvoice)|停止播放语音。|
|[SetMute](#audiomanagersetmute)|设置是否静音。|

---

# AudioManager.MasterVolume

`public static float MasterVolume`

## 描述

整个游戏的音频音量，范围为0~1。

修改此属性即改变其他`AudioSource`组件的音量上限。

---

# AudioManager.BGMVolume

`public static float BGMVolume`

## 描述

背景音乐的音量，受主音量影响，范围为0~1。

修改此属性即改变用于播放背景音乐的`AudioSource`组件的音量。

---

# AudioManager.BGSVolume

`public static float BGSVolume`

## 描述

环境音的音量，受主音量影响，范围为0~1。

修改此属性即改变用于播放环境音的`AudioSource`组件的音量。

---

# AudioManager.SFXVolume

`public static float SFXVolume`

## 描述

音效的音量，受主音量影响，范围为0~1。

修改此属性即改变用于播放音效的`AudioSource`组件的音量。

---

# AudioManager.VoiceVolume

`public static float VoiceVolume`

## 描述

语音的音量，受主音量影响，范围为0~1。

修改此属性即改变用于播放语音的`AudioSource`组件的音量。

---

# AudioManager.BGMSource

`public static AudioSource BGMSource`

## 描述

用于播放背景音乐的`AudioSource`组件。

---

# AudioManager.BGSSource

`public static AudioSource BGSSource`

## 描述

用于播放环境音的`AudioSource`组件。

---

# AudioManager.SFXSource

`public static AudioSource SFXSource`

## 描述

用于播放音效的`AudioSource`组件。

---

# AudioManager.VoiceSource

`public static AudioSource VoiceSource`

## 描述

用于播放语音的`AudioSource`组件。

---

# AudioManager.BGMClip

`public static AudioClip BGMClip`

## 描述

背景音乐的音频剪辑。

对应`AudioSource`组件播放的音频剪辑。

---

# AudioManager.BGSClip

`public static AudioClip BGSClip`

## 描述

环境音的音频剪辑。

对应`AudioSource`组件播放的音频剪辑。

---

# AudioManager.SFXClip

`public static AudioClip SFXClip`

## 描述

音效的音频剪辑。

对应`AudioSource`组件播放的音频剪辑。

---

# AudioManager.VoiceClip

`public static AudioClip VoiceClip`

## 描述

语音的音频剪辑。

对应`AudioSource`组件播放的音频剪辑。

---

# AudioManager.IsMuted

`public static bool IsMuted`

## 描述

只读属性，是否处于静音状态。

---

# AudioManager.PlayBGM

`public static void PlayBGM(string name)`

## 参数

|`name`|音频资源地址或路径，视[资源加载方式](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)而定。|
|:---|:---|

## 描述

在用于播放背景音乐的`AudioSource`组件上播放指定的音频。

---

# AudioManager.PlayBGS

`public static void PlayBGS(string name)`

## 参数

|`name`|音频资源地址或路径，视[资源加载方式](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)而定。|
|:---|:---|

## 描述

在用于播放环境音的`AudioSource`组件上播放指定的音频。

---

# AudioManager.PlayVoice

`public static void PlayVoice(string name)`

## 参数

|`name`|音频资源地址或路径，视[资源加载方式](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)而定。|
|:---|:---|

## 描述

在用于播放语音的`AudioSource`组件上播放指定的音频。

---

# AudioManager.PlaySFX

`public static void PlaySFX(string name)`

## 参数

|`name`|音频资源地址或路径，视[资源加载方式](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)而定。|
|:---|:---|

## 描述

在用于播放音效的`AudioSource`组件上播放指定的音频。

---

# AudioManager.FadeOutAndPlayNewMusic

`public static IEnumerator FadeOutAndPlayNewMusic(string newMusicName, float duration)`

## 参数

|`newMusicName`|新音乐资源地址或路径，视[资源加载方式](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)而定。|
|:---|:---|
|`duration`|淡出时间，单位为秒。|

## 描述

按指定时间淡出用于播放背景音乐的`AudioSource`组件当前播放的背景音乐，并播放指定的新背景音乐。

请使用`MonoBehaviour.StartCoroutine(IEnumerator)`方法调用该方法。

## 返回

协程迭代器。

---

# AudioManager.FadeBGM

`public static IEnumerator FadeBGM(float targetVolume, float duration)`

## 参数

|`targetVolume`|目标音量。|
|:---|:---|
|`duration`|淡出时间，单位为秒。|

## 描述

按指定时间淡入或淡出用于播放背景音乐的`AudioSource`组件的音量到目标音量。

请使用`MonoBehaviour.StartCoroutine(IEnumerator)`方法调用该方法。

## 返回

协程迭代器。

---

# AudioManager.FadeBGS

`public static IEnumerator FadeBGS(float targetVolume, float duration)`

## 参数

|`targetVolume`|目标音量。|
|:---|:---|
|`duration`|淡出时间，单位为秒。|

## 描述

按指定时间淡入或淡出用于播放环境音的`AudioSource`组件的音量到目标音量。

请使用`MonoBehaviour.StartCoroutine(IEnumerator)`方法调用该方法。

## 返回

协程迭代器。

---

# AudioManager.FadeVoice

`public static IEnumerator FadeVoice(float targetVolume, float duration)`

## 参数

|`targetVolume`|目标音量。|
|:---|:---|
|`duration`|淡出时间，单位为秒。|

## 描述

按指定时间淡入或淡出用于播放语音的`AudioSource`组件的音量到目标音量。

请使用`MonoBehaviour.StartCoroutine(IEnumerator)`方法调用该方法。

## 返回

协程迭代器。

---

# AudioManager.StopBGM

`public static void StopBGM()`

## 描述

停止播放用于播放背景音乐的`AudioSource`组件正在播放的音频。

---

# AudioManager.StopBGS

`public static void StopBGS()`

## 描述

停止播放用于播放环境音的`AudioSource`组件正在播放的音频。

---

# AudioManager.StopVoice

`public static void StopVoice()`

## 描述

停止播放用于播放语音的`AudioSource`组件正在播放的音频。

---

# AudioManager.SetMute

`public static void SetMute(bool isMuted)`

## 参数

|`isMuted`|是否静音，`true`为静音，`false`为取消静音。|
|:---|:---|

## 描述

设置是否静音。