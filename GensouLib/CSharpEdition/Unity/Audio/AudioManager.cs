using UnityEngine;
using GensouLib.Unity.ResourceLoader;
using System.Collections;

namespace GensouLib.Unity.Audio
{
    /// <summary>
    /// 音频管理器 <br/>
    /// The audio manager.
    /// </summary>
    /// <remarks>
    /// 提供播放背景音乐，环境音，音效与语音的功能。<br/>
    /// Provides the ability to play background music, background sound, sound effects, and voice.<br/>
    /// 提供淡出音频和淡出并播放新背景音乐的功能。<br/>
    /// Provides the ability to fade out audio and fade out and play new background music.
    /// </remarks>
    public class AudioManager
    {
        /// <summary>
        /// 主音量 <br/>
        /// The main volume of the audio.
        /// </summary>
        public static float MasterVolume 
        { 
            get => masterVolume;
            set
            {
                masterVolume = value;
                SetMasterVolume();
            } 
        }
        
        private static float masterVolume = 1.0f;
        
        /// <summary>
        /// 背景音乐音量 <br/>
        /// The volume of the background music.
        /// </summary>
        public static float BGMVolume 
        { 
            get => BGMSource.volume;
            set => BGMSource.volume = value * MasterVolume;
        }
        
        /// <summary>
        /// 环境音音量 <br/>
        /// The volume of the background sound.
        /// </summary>
        public static float BGSVolume 
        { 
            get => BGSSource.volume;
            set => BGSSource.volume = value * MasterVolume;
        }

        /// <summary>
        /// 音效音量 <br/>
        /// The volume of the sound effects.
        /// </summary>
        public static float SFXVolume
        {
            get => SFXSource.volume;
            set => SFXSource.volume = value * MasterVolume;
        }
        
        /// <summary>
        /// 语音音量 <br/>
        /// The volume of the voice.
        /// </summary>
        public static float VoiceVolume
        {
            get => VoiceSource.volume;
            set => VoiceSource.volume = value * MasterVolume;
        }
        
        /// <summary>
        /// 背景音乐音源 <br/>
        /// The audio source of the background music.
        /// </summary>
        public static AudioSource BGMSource { get; set; }
        
        /// <summary>
        /// 环境音音源 <br/>
        /// The audio source of the background sound.
        /// </summary>
        public static AudioSource BGSSource { get; set; }
        
        /// <summary>
        /// 音效音源 <br/>
        /// The audio source of the sound effects.
        /// </summary>
        public static AudioSource SFXSource { get; set; }
        
        /// <summary>
        /// 语音音源 <br/>
        /// The audio source of the voice.
        /// </summary>
        public static AudioSource VoiceSource { get; set; }
        
        /// <summary>
        /// 背景音乐音频 <br/>
        /// The audio clip of the background music.
        /// </summary>
        public static AudioClip BGMClip { get; set; }
        
        /// <summary>
        /// 环境音音频 <br/>
        /// The audio clip of the background sound.
        /// </summary>
        public static AudioClip BGSClip { get; set; }
        
        /// <summary>
        /// 音效音频 <br/>
        /// The audio clip of the sound effects.
        /// </summary>
        public static AudioClip SFXClip { get; set; }
        
        /// <summary>
        /// 语音音频 <br/>
        /// The audio clip of the voice.
        /// </summary>
        public static AudioClip VoiceClip { get; set; }
        
        /// <summary>
        /// 是否处于静音 <br/>
        /// Whether the audio is muted or not.
        /// </summary>
        public static bool IsMuted { get; private set; } = false;
        
        private static void SetMasterVolume()
        {
            BGMSource.volume = BGMSource.volume * masterVolume;
            BGSSource.volume = BGSSource.volume * masterVolume;
            SFXSource.volume = SFXSource.volume * masterVolume;
            VoiceSource.volume = VoiceSource.volume * masterVolume;
        }
        
        private static void PlayAudio(AudioSource source, AudioClip clip, string name)
        {
            if (source.clip != clip || source.clip == null)
            {
                AssetLoader.LoadResource<AudioClip>(name);
                clip = AssetLoader.GetLoadedAsset<AudioClip>(name);
                if (clip != null)
                {
                    source.clip = clip;
                    source.Play();
                }
                else
                {
                    Debug.LogError($"{name} not found");
                }
            }
            else
            {
                source.Play();
            }
        }
        
        /// <summary>
        /// 播放背景音乐 <br/>
        /// Play the background music.
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。<br/>
        /// The audio resource address or path, depending on the resource loading method.
        /// </param>
        public static void PlayBGM(string name) => PlayAudio(BGMSource, BGMClip, name);
       
        /// <summary>
        /// 播放环境音 <br/>
        /// Play the background sound.
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。<br/>
        /// The audio resource address or path, depending on the resource loading method.
        /// </param>
        public static void PlayBGS(string name) => PlayAudio(BGSSource, BGSClip, name);
        
        /// <summary>
        /// 播放语音 <br/>
        /// Play the voice.
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。<br/>
        /// The audio resource address or path, depending on the resource loading method.
        /// </param>
        public static void PlayVoice(string name) => PlayAudio(VoiceSource, VoiceClip, name);
        
        /// <summary>
        /// 播放音效 <br/>
        /// Play the sound effect.
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。<br/>
        /// The audio resource address or path, depending on the resource loading method.
        /// </param>
        public static void PlaySFX(string name)
        {
            if (SFXSource.clip != SFXClip || SFXClip == null)
            {
                AssetLoader.LoadResource<AudioClip>(name);
                SFXClip = AssetLoader.GetLoadedAsset<AudioClip>(name);
                if (SFXClip != null)
                {
                    SFXSource.PlayOneShot(SFXClip);
                }
                else
                {
                    Debug.LogError($"{name} not found");
                }
            }
            else
            {
                SFXSource.PlayOneShot(SFXClip);
            }
        }
        
        /// <summary>
        /// 淡出并播放新背景音乐 <br/>
        /// Fade out and play a new background music.<br/>
        /// 请使用 <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> 调用。<br/>
        /// Please use <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> to call.
        /// </summary>
        /// <param name="newMusicName">
        /// 新音乐资源地址或路径，视资源加载方式而定。<br/>
        /// The new music resource address or path, depending on the resource loading method.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。<br/>
        /// Fade out duration, unit: seconds.
        /// </param>
        /// <returns>
        /// 协程迭代器。<br/>
        /// Coroutine iterator.
        /// </returns>
        public static IEnumerator FadeOutAndPlayNewMusic(string newMusicName, float duration)
        {
            AssetLoader.LoadResource<AudioClip>(newMusicName);
            AudioClip clip = AssetLoader.GetLoadedAsset<AudioClip>(newMusicName);
            if (clip == null)
            {
                Debug.LogError($"Music clip : {newMusicName} not found");
                yield break;
            }

            float startVolume = BGMSource.volume;
            while (BGMSource.volume > 0)
            {
                BGMSource.volume -= startVolume * Time.deltaTime / duration;
                yield return null;
            }
            
            BGMSource.Stop();
            BGMSource.clip = clip;
            BGMClip = clip;
            BGMSource.volume = startVolume;
            BGMSource.Play();
        }
        
        private static IEnumerator FadeAudioSource(AudioSource audioSource, float targetVolume, float duration)
        {
            float startVolume = audioSource.volume;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                yield return null;
            }

            audioSource.volume = targetVolume;
        }
        
        /// <summary>
        /// 淡入或淡出背景音乐 <br/>
        /// Fade in or fade out the background music.<br/>
        /// 请使用 <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> 调用。<br/>
        /// Please use <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> to call.
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量。<br/>
        /// Target volume.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。<br/>
        /// Fade out duration, unit: seconds.
        /// </param>
        /// <returns>
        /// 协程迭代器。<br/>
        /// Coroutine iterator.
        /// </returns>
        public static IEnumerator FadeBGM(float targetVolume, float duration) => FadeAudioSource(BGMSource, targetVolume, duration);
        
        /// <summary>
        /// 淡入或淡出环境音 <br/>
        /// Fade in or fade out the background sound.<br/>
        /// 请使用 <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> 调用。<br/>
        /// Please use <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> to call.
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量。<br/>
        /// Target volume.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。<br/>
        /// Fade out duration, unit: seconds.
        /// </param>
        /// <returns>
        /// 协程迭代器。<br/>
        /// Coroutine iterator.
        /// </returns>
        public static IEnumerator FadeBGS(float targetVolume, float duration) => FadeAudioSource(BGSSource, targetVolume, duration);
        
        /// <summary>
        /// 淡入或淡出语音 <br/>
        /// Fade in or fade out the voice.<br/>
        /// 请使用 <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> 调用。<br/>
        /// Please use <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> to call.
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量。<br/>
        /// Target volume.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。<br/>
        /// Fade out duration, unit: seconds.
        /// </param>
        /// <returns>
        /// 协程迭代器。<br/>
        /// Coroutine iterator.
        /// </returns>
        public static IEnumerator FadeVoice(float targetVolume, float duration) => FadeAudioSource(VoiceSource, targetVolume, duration);
        
        /// <summary>
        /// 停止播放背景音乐 <br/>
        /// Stop playing the background music.
        /// </summary>
        public static void StopBGM() => BGMSource.Stop();
        
        /// <summary>
        /// 停止播放环境音 <br/>
        /// Stop playing the background sound.
        /// </summary>
        public static void StopBGS() => BGSSource.Stop();
        
        /// <summary>
        /// 停止播放语音 <br/>
        /// Stop playing the voice.
        /// </summary>
        public static void StopVoice() => VoiceSource.Stop();
        
        /// <summary>
        /// 设置是否静音 <br/>
        /// Set whether to mute or not.
        /// </summary>
        /// <param name="mute">
        /// 是否静音。<br/>
        /// Whether to mute or not.
        /// </param>
        public static void SetMute(bool mute)
        {
            IsMuted = mute;
            BGMSource.mute = mute;
            BGSSource.mute = mute;
            SFXSource.mute = mute;
            VoiceSource.mute = mute;
        }
    }
}