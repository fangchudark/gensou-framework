# AudioManager

Inherits: [Node](https://docs.godotengine.org/en/stable/classes/class_node.html)

## Description

A singleton audio manager for controlling audio playback.

Access this class instance via `AudioManager.Instance`.

The script for this class should be attached to a scene added to auto-load.

You can use the provided `AudioManager.tscn` scene from the library,  
or create a scene with a root node of type `Node` and four child nodes of type `AudioStreamPlayer`.

For audio playback, audio files should be used as audio streams instead of resources like `AudioStreamPlaylist` that inherit from `AudioStream`.

## Static Properties

|[Instance](#audiomanagerinstance)|The instance of the AudioManager.|
|:---|:---|

## Signals

|[BGMFadeCompletedAndChanged](#audiomanagerbgmfadecompletedandchanged)|Emitted when fading out and switching background music is completed|
|:---|:---|
|[FadeCompleted](#audiomanagerfadecompleted)|Emitted when the player's fade in or fade out is completed|

## Properties

|[ResPath](#audiomanagerrespath)|Path to the audio files.|
|:---|:---|
|[BGMNodePath](#audiomanagerbgmnodepath)|Node path of the background music player.|
|[BGSNodePath](#audiomanagerbgsnodepath)|Node path of the background sound player.|
|[SFXNodePath](#audiomanagersfxnodepath)|Node path of the sound effect player.|
|[VoiceNodePath](#audiomanagervoicenodepath)|Node path of the voice player.|
|[MasterVolume](#audiomanagermastervolume)|Master volume.|
|[BGMVolume](#audiomanagerbgmvolume)|Volume of the background music.|
|[BGSVolume](#audiomanagerbgsvolume)|Volume of the background sound.|
|[SFXVolume](#audiomanagersfxvolume)|Volume of sound effects.|
|[VoiceVolume](#audiomanagervoicevolume)|Volume of voice playback.|
|[IsMuted](#audiomanagerismuted)|Indicates if audio is muted.|

## Methods

|[PlayBGM](#audiomanagerplaybgm)|Plays background music.|
|:---|:---|
|[PlayBGS](#audiomanagerplaybgs)|Plays background sound.|
|[PlayVoice](#audiomanagerplayvoice)|Plays voice audio.|
|[PlaySFX](#audiomanagerplaysfx)|Plays sound effects.|
|[FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic)|Fades out and plays new background music.|
|[FadeBGM](#audiomanagerfadebgm)|Fade in or fade out background music.|
|[FadeBGS](#audiomanagerfadebgs)|Fade in or fade out background sound.|
|[FadeVoice](#audiomanagerfadevoice)|Fade in or fade out voice playback.|
|[StopBGM](#audiomanagerstopbgm)|Stops background music.|
|[StopBGS](#audiomanagerstopbgs)|Stops background sound.|
|[StopVoice](#audiomanagerstopvoice)|Stops voice playback.|
|[SetMute](#audiomanagersetmute)|Sets mute state.|

---

# AudioManager.Instance

`public static AudioManager Instance`

## Description

The instance of `AudioManager`. Use it to access properties and methods for controlling audio playback.

---

# AudioManager.BGMFadeCompletedAndChanged

`public delegate void BGMFadeCompletedAndChangedEventHandler()`

## Description

Emitted when fading out and switching background music is completed.

This signal is emitted after the method [FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic) completes fading out and switching background music.

---

# AudioManager.FadeCompleted

`public delegate void FadeCompletedEventHandler(string player)`

## Description

Emitted when the player's fade in or fade out is completed.

This signal is emitted after the methods [FadeBGM](#audiomanagerfadebgm), [FadeBGS](#audiomanagerfadebgs), and [FadeVoice](#audiomanagerfadevoice) have completed fading in or out.

The `player` parameter is the name of the player and will have the following three values:

- `BGM`

- `BGS`

- `Voice`

---

# AudioManager.ResPath

`public string ResPath`

## Description

The path to the audio files. Defaults to `res://Audio/` where audio files will be loaded.

---

# AudioManager.BGMNodePath

`public NodePath BGMNodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing background music.  

Defaults to `BGMPlayer` relative to the root node.

---

# AudioManager.BGSNodePath

`public NodePath BGSNodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing background sound.  

Defaults to `BGSPlayer` relative to the root node.

---

# AudioManager.SFXNodePath

`public NodePath SFXNodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing sound effects.  

Defaults to `SFXPlayer` relative to the root node.

---

# AudioManager.VoiceNodePath

`public NodePath VoiceNodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing voice audio.  

Defaults to `VoicePlayer` relative to the root node.

---

# AudioManager.MasterVolume

`public float MasterVolume`

## Description

Master volume.  

Modifying this property adjusts the volume of the Master audio bus.

---

# AudioManager.BGMVolume

`public float BGMVolume`

## Description

Background music volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing background music.

---

# AudioManager.BGSVolume

`public float BGSVolume`

## Description

Background sound volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing background sound.

---

# AudioManager.SFXVolume

`public float SFXVolume`

## Description

Sound effect volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing sound effects.

---

# AudioManager.VoiceVolume

`public float VoiceVolume`

## Description

Voice volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing voice.

---

# AudioManager.IsMuted

`public bool IsMuted`

## Description

Read-only property indicating whether audio is muted.

---

# AudioManager.PlayBGM

`public void PlayBGM(string name)`

## Parameters

|`name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing background music.  

To loop the music, enable the loop option in the file import settings.

---

# AudioManager.PlayBGS

`public void PlayBGS(string name)`

## Parameters

|`name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing background sound.  

To loop the sound, enable the loop option in the file import settings.

---

# AudioManager.PlayVoice

`public void PlayVoice(string name)`

## Parameters

|`name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing voice audio.  

To loop the audio, enable the loop option in the file import settings.

---

# AudioManager.PlaySFX

`public void PlaySFX(string name)`

## Parameters

|`name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing sound effects.  

To loop the audio, enable the loop option in the file import settings.

---

# AudioManager.FadeOutAndPlayNewMusic

`public async Task<bool> FadeOutAndPlayNewMusic(string newMusicName, float duration)`

## Parameters

|`newMusicName`|The name of the new audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|
|`duration`|The fade-out duration in seconds.|

## Description

Fades out the currently playing background music on the `AudioStreamPlayer` used for playing background music and plays the specified new background music.

## Returns

Asynchronous task, returns `true` if successful, `false` if failed.

---

# AudioManager.FadeBGM

`public async Task<bool> FadeBGM(float targetVolume, float duration)`

## Parameters

|`targetVolume`|Target volume (non-negative).|
|:---|:---|
|`duration`|The fade-in or fade-out duration in seconds.|

## Description

Fades the volume of the `AudioStreamPlayer` used for playing background music to the target volume over the specified duration.

## Returns

Asynchronous task, returns `true` when the fade in or fade out is complete, `false` if the target volume is negative.

---

# AudioManager.FadeBGS

`public async Task<bool> FadeBGS(float targetVolume, float duration)`

## Parameters

|`targetVolume`|Target volume (non-negative).|
|:---|:---|
|`duration`|The fade-in or fade-out duration in seconds.|

## Description

Fades the volume of the `AudioStreamPlayer` used for playing background sound to the target volume over the specified duration.

## Returns

Asynchronous task, returns `true` when the fade in or fade out is complete, `false` if the target volume is negative.

---

# AudioManager.FadeVoice

`public async Task<bool> FadeVoice(float targetVolume, float duration)`

## Parameters

|`targetVolume`|Target volume (non-negative).|
|:---|:---|
|`duration`|The fade-in or fade-out duration in seconds.|

## Description

Fades the volume of the `AudioStreamPlayer` used for playing voice to the target volume over the specified duration.

## Returns

Asynchronous task, returns `true` when the fade in or fade out is complete, `false` if the target volume is negative.

---

# AudioManager.StopBGM

`public void StopBGM()`

## Description

Stops the audio currently playing on the `AudioStreamPlayer` used for playing background music.

---

# AudioManager.StopBGS

`public void StopBGS()`

## Description

Stops the audio currently playing on the `AudioStreamPlayer` used for playing background sound.

---

# AudioManager.StopVoice

`public void StopVoice()`

## Description

Stops the audio currently playing on the `AudioStreamPlayer` used for playing voice.

---

# AudioManager.SetMute

`public void SetMute(bool mute)`

## Parameters

|`mute`|Whether to mute (`true`) or unmute (`false`) all audio.|
|:---|:---|

## Description

Sets the mute state
