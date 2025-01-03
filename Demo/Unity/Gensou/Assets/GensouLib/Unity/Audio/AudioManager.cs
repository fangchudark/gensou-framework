using UnityEngine;
using GensouLib.Unity.ResourceLoader;
using System.Collections;
using System.Threading.Tasks;
using System;

namespace GensouLib.Unity.Audio
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    public class AudioManager
    {
        /// <summary>
        /// 主音量 
        /// </summary>
        public static float MasterVolume 
        { 
            get => masterVolume;
            set => SetMasterVolume(value); 
        }

        private static void SetMasterVolume(float value)
        {
            masterVolume = value;
            if (BgmSource != null) BgmSource.volume = value;
            else bgmVolume = value;

            if (BgsSource != null) BgsSource.volume = value;
            else bgsVolume = value;

            if (SeSource  != null) SeSource.volume  = value;
            else seVolume = value;

            if (VoiceSource!= null) VoiceSource.volume = value;
            else voiceVolume = value;
        }

        private static float masterVolume = 1.0f;
        
        /// <summary>
        /// 背景音乐音量 
        /// </summary>
        public static float BgmVolume 
        { 
            get 
            {
                if (BgmSource == null) return bgmVolume;
                else return BgmSource.volume;
            } 
            set 
            {
                if (BgmSource == null) bgmVolume = value * MasterVolume;
                else BgmSource.volume = value * MasterVolume;
            }
        }

        private static float bgmVolume = 1.0f;

        /// <summary>
        /// 环境音音量 
        /// </summary>
        public static float BgsVolume 
        { 
            get
            {
                if (BgsSource == null) return bgsVolume;
                else return BgsSource.volume;
            }
            set
            {
                if (BgsSource == null) bgsVolume = value * MasterVolume;
                else BgsSource.volume = value * MasterVolume;
            }
        }

        private static float bgsVolume = 1.0f;

        /// <summary>
        /// 音效音量
        /// </summary>
        public static float SeVolume
        {
            get
            {
                if (SeSource == null) return seVolume;
                else return SeSource.volume;
            }
            set
            {
                if (SeSource == null) seVolume = value * MasterVolume;
                else SeSource.volume = value * MasterVolume;
            }
        }
        
        private static float seVolume = 1.0f;

        /// <summary>
        /// 语音音量
        /// </summary>
        public static float VoiceVolume
        {
            get
            {
                if (VoiceSource == null) return voiceVolume;
                else return VoiceSource.volume;
            }
            set
            {
                if (VoiceSource == null) voiceVolume = value * MasterVolume;
                else VoiceSource.volume = value * MasterVolume;
            }
        }

        private static float voiceVolume = 1.0f;
        
        /// <summary>
        /// 背景音乐音源
        /// </summary>
        public static AudioSource BgmSource { get; set; }
        
        /// <summary>
        /// 环境音音源
        /// </summary>
        public static AudioSource BgsSource { get; set; }
        
        /// <summary>
        /// 音效音源
        /// </summary>
        public static AudioSource SeSource { get; set; }
        
        /// <summary>
        /// 语音音源
        /// </summary>
        public static AudioSource VoiceSource { get; set; }
        
        /// <summary>
        /// 背景音乐音频
        /// </summary>
        public static AudioClip BgmClip { get; set; }
        
        /// <summary>
        /// 环境音音频
        /// </summary>
        public static AudioClip BgsClip { get; set; }
        
        /// <summary>
        /// 音效音频
        /// </summary>
        public static AudioClip SeClip { get; set; }
        
        /// <summary>
        /// 语音音频
        /// </summary>
        public static AudioClip VoiceClip { get; set; }
        
        /// <summary>
        /// 是否处于静音
        /// </summary>
        public static bool IsMuted { get; private set; } = false;

        private static bool Stop = false;

        /// <summary>
        /// 只读，是否在淡入淡出
        /// </summary>
        public static bool Fading { get; private set; } = false;
        
        private static void PlayAudio(AudioSource source, AudioClip oldClip, Action<AudioClip> setAudioClip, string name)
        {
            if (source.clip != oldClip || source.clip == null)
            {
                AssetLoader.LoadResource<AudioClip>(name);
                AudioClip clip = AssetLoader.GetLoadedAsset<AudioClip>(name);
                if (clip != null)
                {
                    source.clip = clip;
                    setAudioClip(clip);
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
        /// 播放背景音乐
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。
        /// </param>
        public static void PlayBgm(string name) => PlayAudio(BgmSource, BgmClip, clip => BgmClip = clip, name);
       
        /// <summary>
        /// 播放环境音
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。
        /// </param>
        public static void PlayBgs(string name) => PlayAudio(BgsSource, BgsClip, clip => BgsClip = clip, name);
        
        /// <summary>
        /// 播放语音
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。
        /// </param>
        public static void PlayVoice(string name) => PlayAudio(VoiceSource, VoiceClip, clip => VoiceClip = clip, name);
        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name">
        /// 音频资源地址或路径，视资源加载方式而定。
        /// </param>
        public static void PlaySe(string name)
        {
            if (SeSource.clip != SeClip || SeClip == null)
            {
                AssetLoader.LoadResource<AudioClip>(name);
                SeClip = AssetLoader.GetLoadedAsset<AudioClip>(name);
                if (SeClip != null)
                {
                    SeSource.PlayOneShot(SeClip);
                }
                else
                {
                    Debug.LogError($"{name} not found");
                }
            }
            else
            {
                SeSource.PlayOneShot(SeClip);
            }
        }
        
        /// <summary>
        /// 淡出并播放新背景音乐
        /// </summary>
        /// <param name="newMusicName">
        /// 新音乐资源地址或路径，视资源加载方式而定。
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。
        /// </param>
        /// <returns>
        /// 异步任务，不被打断完成时返回 true，否则返回 false。
        /// </returns>
        public static async Task<bool> FadeOutAndPlayNewMusic(string newMusicName, float duration)
        {
            Fading = true;
            float startVolume = BgmSource.volume;
            AssetLoader.LoadResource<AudioClip>(newMusicName);
            AudioClip clip = AssetLoader.GetLoadedAsset<AudioClip>(newMusicName);
            if (clip == null)
            {
                Debug.LogError($"Music clip : {newMusicName} not found");
                return false;
            }

            while (BgmSource.volume > 0)
            {
                if (Stop) return false;
                BgmSource.volume -= startVolume * Time.deltaTime / duration;
                await Task.Yield();
            }
            
            BgmSource.Stop();
            BgmSource.clip = clip;
            BgmClip = clip;
            BgmSource.volume = startVolume;
            BgmSource.Play();
            Fading = false;
            return true;
        }
        
        private static async Task<bool> FadeAudioSource(AudioSource audioSource, float targetVolume, float duration)
        {
            Fading = true;
            float startVolume = audioSource.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (Stop) return false;
                elapsed += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                await Task.Yield();
            }

            audioSource.volume = targetVolume;
            Fading = false;
            return true;
        }
        
        /// <summary>
        /// 淡出背景音乐
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量。
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。
        /// </param>
        /// <returns>
        /// 异步任务，不被打断完成时返回 true，否则返回 false。
        /// </returns>
        public static async Task<bool> FadeBgm(float targetVolume, float duration) => await FadeAudioSource(BgmSource, targetVolume, duration);
        
        /// <summary>
        /// 淡出环境音
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量。
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。
        /// </param>
        /// <returns>
        /// 异步任务，不被打断完成时返回 true，否则返回 false。
        /// </returns>
        public static async Task<bool> FadeBgs(float targetVolume, float duration) => await FadeAudioSource(BgsSource, targetVolume, duration);
        
        /// <summary>
        /// 淡出语音 
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量。
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位：秒。
        /// </param>
        /// <returns>
        /// 异步任务，不被打断完成时返回 true，否则返回 false。
        /// </returns>
        public static async Task<bool> FadeVoice(float targetVolume, float duration) => await FadeAudioSource(VoiceSource, targetVolume, duration);
        
        /// <summary>
        /// 停止播放背景音乐 
        /// </summary>
        public static void StopBgm() => BgmSource.Stop();
        
        /// <summary>
        /// 停止播放环境音 
        /// </summary>
        public static void StopBgs() => BgsSource.Stop();
        
        /// <summary>
        /// 停止播放语音 
        /// </summary>
        public static void StopVoice() => VoiceSource.Stop();
        
        /// <summary>
        /// 设置是否静音 
        /// </summary>
        /// <param name="mute">
        /// 是否静音。
        /// </param>
        public static void SetMute(bool mute)
        {
            IsMuted = mute;
            BgmSource.mute = mute;
            BgsSource.mute = mute;
            SeSource.mute = mute;
            VoiceSource.mute = mute;
        }

        /// <summary>
        /// 停止淡入淡出
        /// </summary>
        public static void StopFade()
        {
            Stop = true; //停止淡入淡出
            Stop = false; // 重置状态
        }
    }
}