# AudioManager

Inherits: [Node](https://docs.godotengine.org/en/stable/classes/class_node.html)

## Description

Audio manager for controlling audio playback.

Due to the nature of GDScript, this class is anonymous, so use the node name defined by Autoload to access it instead of using the `AudioManager` class name directly.

The script for this class should be attached to a scene added to auto-load.

You can use the provided `AudioManager.tscn` scene from the library,  
or create a scene with a root node of type `Node` and four child nodes of type `AudioStreamPlayer`.

For audio playback, audio files should be used as audio streams instead of resources like `AudioStreamPlaylist` that inherit from `AudioStream`.

## Signals

|[bgm_fade_completed_and_changed](#audiomanagerbgm_fade_completed_and_changed)|Emitted when fading out and switching background music is completed|
|:---|:---|
|[fade_completed](#audiomanagerfade_completed)|Emitted when the player's fade in or fade out is completed|

## Properties

|[res_path](#audiomanagerres_path)|Path to the audio files.|
|:---|:---|
|[bgm_node_path](#audiomanagerbgm_node_path)|Node path of the background music player.|
|[bgs_node_path](#audiomanagerbgs_node_path)|Node path of the background sound player.|
|[sfx_node_path](#audiomanagersfx_node_path)|Node path of the sound effect player.|
|[voice_node_path](#audiomanagervoice_node_path)|Node path of the voice player.|
|[master_volume](#audiomanagermaster_volume)|Master volume.|
|[bgm_volume](#audiomanagerbgm_volume)|Volume of the background music.|
|[bgs_volume](#audiomanagerbgs_volume)|Volume of the background sound.|
|[sfx_volume](#audiomanagersfx_volume)|Volume of sound effects.|
|[voice_volume](#audiomanagervoice_volume)|Volume of voice playback.|
|[is_muted](#audiomanageris_muted)|Indicates if audio is muted.|

## Methods

|[play_bgm](#audiomanagerplay_bgm)|Plays background music.|
|:---|:---|
|[play_bgs](#audiomanagerplay_bgs)|Plays background sound.|
|[play_voice](#audiomanagerplay_voice)|Plays voice audio.|
|[play_sfx](#audiomanagerplay_sfx)|Plays sound effects.|
|[fade_out_and_play_new_music](#audiomanagerfade_out_and_play_new_music)|Fades out and plays new background music.|
|[fade_bgm](#audiomanagerfade_bgm)|Fade in or fade out background music.|
|[fade_bgs](#audiomanagerfade_bgs)|Fade in or fade out background sound.|
|[fade_voice](#audiomanagerfade_voice)|Fade in or fade out voice playback.|
|[stop_bgm](#audiomanagerstop_bgm)|Stops background music.|
|[stop_bgs](#audiomanagerstop_bgs)|Stops background sound.|
|[stop_voice](#audiomanagerstop_voice)|Stops voice playback.|
|[set_mute](#audiomanagerset_mute)|Sets mute state.|

---

# AudioManager.bgm_fade_completed_and_changed

`signal bgm_fade_completed_and_changed()`

## Description

Emitted when fading out and switching background music is completed.

This signal is emitted after the method [fade_out_and_play_new_music](#audiomanagerfade_out_and_play_new_music) completes fading out and switching background music.

---

# AudioManager.fade_completed

`signal fade_completed(player: String)`

## Description

Emitted when the player's fade in or fade out is completed.

This signal is emitted after the methods [fade_bgm](#audiomanagerfade_bgm), [fade_bgs](#audiomanagerfade_bgs), and [fade_voice](#audiomanagerfade_voice) have completed fading in or out.

The `player` parameter is the name of the player and will have the following three values:

- `bgm` 

- `bgs`

- `voice`

---

# AudioManager.res_path

`var res_path: String`

## Description

The path to the audio files. Defaults to `res://Audio/` where audio files will be loaded.

---

# AudioManager.bgm_node_path

`var bgm_node_path: NodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing background music.  

Defaults to `BGMPlayer` relative to the root node.

---

# AudioManager.bgs_node_path

`var bgs_node_path: NodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing background sound.  

Defaults to `BGSPlayer` relative to the root node.

---

# AudioManager.sfx_node_path

`var sfx_node_path: NodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing sound effects.  

Defaults to `SFXPlayer` relative to the root node.

---

# AudioManager.voice_node_path

`var voice_node_path: NodePath`

## Description

The node path of the `AudioStreamPlayer` used for playing voice audio.  

Defaults to `VoicePlayer` relative to the root node.

---

# AudioManager.master_volume

`var master_volume: float`

## Description

Master volume.  

Modifying this property adjusts the volume of the Master audio bus.

---

# AudioManager.bgm_volume

`var bgm_volume: float`

## Description

Background music volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing background music.

---

# AudioManager.bgs_volume

`var bgs_volume: float`

## Description

Background sound volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing background sound.

---

# AudioManager.sfx_volume

`var sfx_volume: float`

## Description

Sound effect volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing sound effects.

---

# AudioManager.voice_volume

`var voice_volume: float`

## Description

Voice volume.  

Modifying this property adjusts the volume of the `AudioStreamPlayer` used for playing voice.

---

# AudioManager.is_muted

`var is_muted: bool`

## Description

Read-only property indicating whether audio is muted.

---

# AudioManager.play_bgm

`func play_bgm(file_name: String) -> void`

## Parameters

|`file_name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing background music.  

To loop the music, enable the loop option in the file import settings.

---

# AudioManager.play_bgs

`func play_bgs(file_name: String) -> void`

## Parameters

|`file_name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing background sound.  

To loop the sound, enable the loop option in the file import settings.

---

# AudioManager.play_voice

`func play_voice(file_name: String) -> void`

## Parameters

|`file_name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing voice audio.  

To loop the audio, enable the loop option in the file import settings.

---

# AudioManager.play_sfx

`func play_sfx(file_name: String) -> void`

## Parameters

|`file_name`|The name of the audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|

## Description

Plays the specified audio file on the `AudioStreamPlayer` used for playing sound effects.  

To loop the audio, enable the loop option in the file import settings.

---

# AudioManager.fade_out_and_play_new_music

`func fade_out_and_play_new_music(new_music_name: String, duration: float) -> bool`

## Parameters

|`new_music_name`|The name of the new audio file, including extension. Supports `ogg`, `wav`, `mp3` formats.|
|:---|:---|
|`duration`|The fade-out duration in seconds.|

## Description

Awaitable.

Fades out the currently playing background music on the `AudioStreamPlayer` used for playing background music and plays the specified new background music.

## Returns

Returns `true` if successful, `false` if failed.

---

# AudioManager.fade_bgm

`func fade_bgm(target_volume: float, duration: float) -> bool`

## Parameters

|`target_volume`|Target volume (non-negative).|
|:---|:---|
|`duration`|The fade-in or fade-out duration in seconds.|

## Description

Awaitable.

Fades the volume of the `AudioStreamPlayer` used for playing background music to the target volume over the specified duration.

## Returns

Returns `true` when the fade in or fade out is complete, `false` if the target volume is negative.

---

# AudioManager.fade_bgs

`func fade_bgs(target_volume: float, duration: float) -> bool`

## Parameters

|`target_volume`|Target volume (non-negative).|
|:---|:---|
|`duration`|The fade-in or fade-out duration in seconds.|

## Description

Awaitable.

Fades the volume of the `AudioStreamPlayer` used for playing background sound to the target volume over the specified duration.

## Returns

Returns `true` when the fade in or fade out is complete, `false` if the target volume is negative.

---

# AudioManager.fade_voice

`func fade_voice(target_volume: float, duration: float) -> bool`

## Parameters

|`target_volume`|Target volume (non-negative).|
|:---|:---|
|`duration`|The fade-in or fade-out duration in seconds.|

## Description

Awaitable.

Fades the volume of the `AudioStreamPlayer` used for playing voice to the target volume over the specified duration.

## Returns

Returns `true` when the fade in or fade out is complete, `false` if the target volume is negative.

---

# AudioManager.stop_bgm

`func stop_bgm() -> void`

## Description

Stops the audio currently playing on the `AudioStreamPlayer` used for playing background music.

---

# AudioManager.stop_bgs

`func stop_bgs() -> void`

## Description

Stops the audio currently playing on the `AudioStreamPlayer` used for playing background sound.


---

# AudioManager.stop_voice

`func stop_voice() -> void`

## Description

Stops the audio currently playing on the `AudioStreamPlayer` used for playing voice.

---

# AudioManager.set_mute

`func set_mute(mute: bool) -> void`

## Parameters

|`mute`|Whether to mute (`true`) or unmute (`false`) all audio.|
|:---|:---|

## Description

Sets the mute state