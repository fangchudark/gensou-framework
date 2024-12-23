# AudioManager

## Description

Audio Manager, used to control audio playback.

## Static Properties

| [MasterVolume](#audiomanagermastervolume) | Master volume. |
| :--- | :--- |
| [BGMVolume](#audiomanagerbgmvolume) | Background music volume. |
| [BGSVolume](#audiomanagerbgsvolume) | Background sound volume. |
| [SFXVolume](#audiomanagersfxvolume) | Sound effects volume. |
| [VoiceVolume](#audiomanagervoicevolume) | Voice volume. |
| [BGMSource](#audiomanagerbgmsource) | Background music audio source. |
| [BGSSource](#audiomanagerbgssource) | Background sound audio source. |
| [SFXSource](#audiomanagersfxsource) | Sound effects audio source. |
| [VoiceSource](#audiomanagervoicesource) | Voice audio source. |
| [BGMClip](#audiomanagerbgmclip) | Background music audio clip. |
| [BGSClip](#audiomanagerbgsclip) | Background sound audio clip. |
| [SFXClip](#audiomanagersfxclip) | Sound effects audio clip. |
| [VoiceClip](#audiomanagervoiceclip) | Voice audio clip. |
| [IsMuted](#audiomanagerismuted) | Whether the audio is muted. |

## Static Methods

| [PlayBGM](#audiomanagerplaybgm) | Play background music. |
| :--- | :--- |
| [PlayBGS](#audiomanagerplaybgs) | Play Background sound. |
| [PlayVoice](#audiomanagerplayvoice) | Play voice. |
| [PlaySFX](#audiomanagerplaysfx) | Play sound effect. |
| [FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic) | Fade out and play new background music. |
| [FadeBGM](#audiomanagerfadebgm) | Fade out background music. |
| [FadeBGS](#audiomanagerfadebgs) | Fade out Background sound. |
| [FadeVoice](#audiomanagerfadevoice) | Fade out voice. |
| [StopBGM](#audiomanagerstopbgm) | Stop playing background music. |
| [StopBGS](#audiomanagerstopbgs) | Stop playing Background sound. |
| [StopVoice](#audiomanagerstopvoice) | Stop playing voice. |
| [SetMute](#audiomanagersetmute) | Set mute state. |

---

# AudioManager.MasterVolume

`public static float MasterVolume`

## Description

The overall audio volume of the game, range from 0 to 1.

Changing this property changes the volume limit for other `AudioSource` components.

---

# AudioManager.BGMVolume

`public static float BGMVolume`

## Description

The volume of background music, affected by the master volume, range from 0 to 1.

Changing this property changes the volume of the `AudioSource` component used to play background music.

---

# AudioManager.BGSVolume

`public static float BGSVolume`

## Description

The volume of background sound, affected by the master volume, range from 0 to 1.

Changing this property changes the volume of the `AudioSource` component used to play background sound.

---

# AudioManager.SFXVolume

`public static float SFXVolume`

## Description

The volume of sound effects, affected by the master volume, range from 0 to 1.

Changing this property changes the volume of the `AudioSource` component used to play sound effects.

---

# AudioManager.VoiceVolume

`public static float VoiceVolume`

## Description

The volume of voice, affected by the master volume, range from 0 to 1.

Changing this property changes the volume of the `AudioSource` component used to play voice.

---

# AudioManager.BGMSource

`public static AudioSource BGMSource`

## Description

The `AudioSource` component used for playing background music.

---

# AudioManager.BGSSource

`public static AudioSource BGSSource`

## Description

The `AudioSource` component used for playing background sound.

---

# AudioManager.SFXSource

`public static AudioSource SFXSource`

## Description

The `AudioSource` component used for playing sound effects.

---

# AudioManager.VoiceSource

`public static AudioSource VoiceSource`

## Description

The `AudioSource` component used for playing voice.

---

# AudioManager.BGMClip

`public static AudioClip BGMClip`

## Description

The audio clip for background music.

The audio clip played by the corresponding `AudioSource` component.

---

# AudioManager.BGSClip

`public static AudioClip BGSClip`

## Description

The audio clip for background sound.

The audio clip played by the corresponding `AudioSource` component.

---

# AudioManager.SFXClip

`public static AudioClip SFXClip`

## Description

The audio clip for sound effects.

The audio clip played by the corresponding `AudioSource` component.

---

# AudioManager.VoiceClip

`public static AudioClip VoiceClip`

## Description

The audio clip for voice.

The audio clip played by the corresponding `AudioSource` component.

---

# AudioManager.IsMuted

`public static bool IsMuted`

## Description

Read-only property, indicating whether the audio is muted.

---

# AudioManager.PlayBGM

`public static void PlayBGM(string name)`

## Parameters

| `name` | The audio resource path or address, depending on the [resource loading method](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource). |
| :--- | :--- |

## Description

Plays the specified audio on the `AudioSource` component used to play background music.

---

# AudioManager.PlayBGS

`public static void PlayBGS(string name)`

## Parameters

| `name` | The audio resource path or address, depending on the [resource loading method](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource). |
| :--- | :--- |

## Description

Plays the specified audio on the `AudioSource` component used to play background sound.

---

# AudioManager.PlayVoice

`public static void PlayVoice(string name)`

## Parameters

| `name` | The audio resource path or address, depending on the [resource loading method](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource). |
| :--- | :--- |

## Description

Plays the specified audio on the `AudioSource` component used to play voice.

---

# AudioManager.PlaySFX

`public static void PlaySFX(string name)`

## Parameters

| `name` | The audio resource path or address, depending on the [resource loading method](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource). |
| :--- | :--- |

## Description

Plays the specified audio on the `AudioSource` component used to play sound effects.

---

# AudioManager.FadeOutAndPlayNewMusic

`public static IEnumerator FadeOutAndPlayNewMusic(string newMusicName, float duration)`

## Parameters

| `newMusicName` | The new music resource path or address, depending on the [resource loading method](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource). |
| :--- | :--- |
| `duration` | The fade-out duration, in seconds. |

## Description

Fade out the current background music being played by the `AudioSource` component used for background music at the specified time, and play the specified new background music.

This method should be called using `MonoBehaviour.StartCoroutine(IEnumerator)`.

## Returns

An IEnumerator for the coroutine.

---

# AudioManager.FadeBGM

`public static IEnumerator FadeBGM(float targetVolume, float duration)`

## Parameters

| `targetVolume` | The target volume. |
| :--- | :--- |
| `duration` | The fade-in or fade-out duration, in seconds. |

## Description

Fade int or fade out the volume of the `AudioSource` component used to play background music to the target volume at the specified time.

This method should be called using `MonoBehaviour.StartCoroutine(IEnumerator)`.

## Returns

An IEnumerator for the coroutine.

---

# AudioManager.FadeBGS

`public static IEnumerator FadeBGS(float targetVolume, float duration)`

## Parameters

| `targetVolume` | The target volume. |
| :--- | :--- |
| `duration` | The fade-in or fade-out duration, in seconds. |

## Description

Fade int or fade out the volume of the `AudioSource` component used to play background sound to the target volume at the specified time.

This method should be called using `MonoBehaviour.StartCoroutine(IEnumerator)`.

## Returns

An IEnumerator for the coroutine.

---

# AudioManager.FadeVoice

`public static IEnumerator FadeVoice(float targetVolume, float duration)`

## Parameters

| `targetVolume` | The target volume. |
| :--- | :--- |
| `duration` | The fade-in or fade-out duration, in seconds. |

## Description

Fade int or fade out the volume of the `AudioSource` component used to play voice to the target volume at the specified time.

This method should be called using `MonoBehaviour.StartCoroutine(IEnumerator)`.

## Returns

An IEnumerator for the coroutine.

---

# AudioManager.StopBGM

`public static void StopBGM()`

## Description

Stops the audio being played by the `AudioSource` component used to play background music.

---

# AudioManager.StopBGS

`public static void StopBGS()`

## Description

Stops the audio being played by the `AudioSource` component used to play background sound.

---

# AudioManager.StopVoice

`public static void StopVoice()`

## Description

Stops the audio being played by the `AudioSource` component used to play voice.

---

# AudioManager.SetMute

`public static void SetMute(bool isMuted)`

## Parameters

| `isMuted` | Mute or not. `true` means muted or `false` means unmuted.|
| :--- | :--- |

## Description

Set the mute state.
