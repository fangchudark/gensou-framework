using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GensouLib.Godot.Audio
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    public partial class AudioManager : Node
    {

        /// <summary>
        /// 任一在节点树上的节点
        /// </summary>
        public static Node Manager {get; set;} = null;
        private static Task FadeBgmTask = null;
        private static Task FadeBgsTask = null;
        private static Task FadeVoiceTask = null;
        private static CancellationTokenSource fadeBgmCancellationTokenSource = null;
        private static CancellationTokenSource fadeBgsCancellationTokenSource = null;
        private static CancellationTokenSource fadeVoiceCancellationTokenSource = null;
        /// <summary>
        /// 主音量，取决于默认的Master音频总线的音量，修改该属性即改变该音频总线的音量
        /// </summary>
        public static float MasterVolume
        {
            get => Mathf.Max(Mathf.DbToLinear(AudioServer.GetBusVolumeDb(0)), 0.0f);
            set => AudioServer.SetBusVolumeDb(0, Mathf.LinearToDb(value));
        }
        
        /// <summary>
        /// 背景音乐音量，修改该属性即改变背景音乐的音量
        /// </summary>
        public static float BgmVolume
        {
            get
            {
                if (BgmPlayer == null) return bgmVolume;
                else return Mathf.Max(Mathf.DbToLinear(BgmPlayer.VolumeDb), 0.0f); // 确保获取的值可读，而不是负无穷
            }
            set
            {
                if (BgmPlayer == null) bgmVolume = value;
                else BgmPlayer.VolumeDb = Mathf.LinearToDb(value);
            }
        }

        private static float bgmVolume = 1.0f;

        /// <summary>
        /// 环境音效音量，修改该属性即改变环境音效的音量
        /// </summary>
        public static float BgsVolume
        {
            get
            {
                if (BgsPlayer == null) return bgsVolume;
                else return Mathf.Max(Mathf.DbToLinear(BgsPlayer.VolumeDb), 0.0f);
            }
            set 
            {
                if (BgsPlayer == null) bgsVolume = value;
                else BgsPlayer.VolumeDb = Mathf.LinearToDb(value);
            }
        }

        private static float bgsVolume = 1.0f;

        /// <summary>
        /// 音效音量，修改该属性即改变音效的音量
        /// </summary>
        public static float SeVolume
        {
            get
            {
                if (SePlayer == null) return seVolume;
                else return Mathf.Max(Mathf.DbToLinear(SePlayer.VolumeDb), 0.0f);
            }
            set
            {
                if (SePlayer == null) seVolume = value;
                else SePlayer.VolumeDb = Mathf.LinearToDb(value);
            }
        }

        private static float seVolume = 1.0f;

        /// <summary>
        /// 语音音量，修改该属性即改变语音的音量
        /// </summary>
        public static float VoiceVolume
        {
            get
            {
                if (VoicePlayer == null) return voiceVolume;
                else return Mathf.Max(Mathf.DbToLinear(VoicePlayer.VolumeDb), 0.0f);
            }
            set
            {
                if (VoicePlayer == null) voiceVolume = value;
                else VoicePlayer.VolumeDb = Mathf.LinearToDb(value);
            }
        }
        
        private static float voiceVolume = 1.0f;

        /// <summary>
        /// BGM播放器
        /// </summary>
        public static AudioStreamPlayer BgmPlayer {get; set;}
        
        /// <summary>
        /// BGS播放器
        /// </summary>
        public static AudioStreamPlayer BgsPlayer {get; set;}
        
        /// <summary>
        /// 音效播放器
        /// </summary>
        public static AudioStreamPlayer SePlayer {get; set;}
        
        /// <summary>
        /// 语音播放器
        /// </summary>
        public static AudioStreamPlayer VoicePlayer {get; set;}

        /// <summary>
        /// 是否已静音
        /// </summary>
        public static bool IsMuted { get; private set; } = false;
        
        /// <summary>
        /// 是否在淡入淡出
        /// </summary>
        public static bool Fading { get; private set; } = false;

        private static float originalBgmVolume;
        
        private static float originalBgsVolume;
        
        private static float originalSeVolume;
        
        private static float originalVoiceVolume;
        
        private static readonly Dictionary<string, AudioStream> audioPool = new();
        
        private static readonly HashSet<string> AudioFileExtensions = new() 
        { 
            ".ogg",
            ".wav",
            ".mp3" 
        };

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="manager">任一在节点树上的节点</param>
        public static void Init(Node manager)
        {
            Manager = manager;
        }

        private static bool IsAudioFile(string name)
        {
            return AudioFileExtensions.Contains(System.IO.Path.GetExtension(name));
        }

        private static void PlayAudio(AudioStreamPlayer player, string name)
        {
            if (!IsAudioFile(name))
            {
                GD.PushError($"AudioManager: Invalid audio file: {name}");
                return;
            }

            if (!audioPool.ContainsKey(name))
            {
                AudioStream audio = ResourceLoader.Load<AudioStream>(name);
                if (audio == null)
                {
                    GD.PushError($"AudioManager: Could not load audio stream: {name}");
                    return;
                }
                audioPool[name] = audio;
            }

            player.Stream = audioPool[name];
            player.Play();
        }

        /// <summary>
        /// 播放背景音乐 <br/>
        /// 如需循环播放请在文件导入选项勾选循环选项
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
        /// </param>
        public static void PlayBgm(string name) => PlayAudio(BgmPlayer, name);

        /// <summary>
        /// 播放环境音效 <br/>
        /// 如需循环播放请在文件导入选项勾选循环选项
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
        /// </param>
        public static void PlayBgs(string name) => PlayAudio(BgsPlayer,  name);
        
        /// <summary>
        /// 播放语音 <br/>
        /// 如需循环播放请在文件导入选项勾选循环选项
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
        /// </param>
        public static void PlayVoice(string name) => PlayAudio(VoicePlayer, name);

        /// <summary>
        /// 播放音效 <br/>
        /// 如需循环播放请在文件导入选项勾选循环选项
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
        /// </param>
        public static void PlaySe(string name) => PlayAudio(SePlayer, name);

        // /// <summary>
        // /// 淡出当前背景音乐并播放新音乐
        // /// </summary>
        // /// <param name="newMusicName">
        // /// 新音乐文件名，包含拓展名，支持"ogg", "wav", "mp3"格式
        // /// </param>
        // /// <param name="duration">
        // /// 淡出时间，单位秒
        // /// </param>
        // /// <returns>
        // /// 异步任务，返回 true 表示成功，false 表示失败
        // /// </returns>
        // public static async Task<bool> FadeOutAndPlayNewMusic(string newMusicName, float duration)
        // {
        //     if (Manager == null)
        //     {
        //         GD.PushError("AudioManager: Manager is null");
        //         return false;
        //     }

        //     if (!IsAudioFile(newMusicName))
        //     {
        //         GD.PushError($"AudioManager: Invalid audio file: {newMusicName}");
        //         return false;
        //     }

        //     AudioStream newMusic;

        //     if (!audioPool.ContainsKey(newMusicName))
        //     {
        //         newMusic = ResourceLoader.Load<AudioStream>("newMusicName");
        //     }
        //     else
        //     {
        //         newMusic = audioPool[newMusicName];
        //     }

        //     if (newMusic == null)
        //     {
        //         GD.PushError($"AudioManager: Could not load audio stream: {newMusicName}");
        //         return false;
        //     }

        //     Fading = true;
        //     float startVolume = BgmVolume;

        //     while (BgmVolume > 0.0f)
        //     {
        //         float volumeStep = startVolume * (float)Manager.GetProcessDeltaTime() / duration;
        //         if (float.IsNaN(BgmVolume - volumeStep) || BgmVolume - volumeStep < 0.0f)
        //         {
        //             BgmVolume = 0.0f;
        //             break;
        //         }
        //         BgmVolume -= volumeStep;
        //         await Manager.ToSignal(Manager.GetTree(), "process_frame");
        //     }

        //     BgmPlayer.Stop();
        //     BgmPlayer.Stream = newMusic;
        //     BgmVolume = startVolume;
        //     BgmPlayer.Play();
        //     Fading = false;
        //     return true;
        // }

        private static async Task FadeAudio(float startVolume, Action<float> setVolume, float targetVolume, float duration, CancellationToken cancellationToken)
        {
            if (Manager == null)
            {
                GD.PushError("AudioManager: Manager is null");
                return;
            }

            if (targetVolume < 0.0f)
            {
                GD.PushError($"AudioManager: Invalid target volume: {targetVolume}");
                return;
            }
            Fading = true;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                if (cancellationToken.IsCancellationRequested) return;
                elapsed += (float)Manager.GetProcessDeltaTime();
                float newVolume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                if (float.IsNaN(newVolume) || newVolume < 0.0f)
                {
                    setVolume(targetVolume);
                    break;
                }
                setVolume(newVolume);
                await Manager.ToSignal(Manager.GetTree(), "process_frame");
            }

            setVolume(targetVolume);
            Fading = false;
            return;
        }

        /// <summary>
        /// 淡入或淡出背景音乐
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位秒
        /// </param>
        public static void FadeBgm(float targetVolume, float duration)
        {
            // 如果已经有正在执行的任务，取消它
            fadeBgmCancellationTokenSource?.Cancel();
            fadeBgmCancellationTokenSource = new CancellationTokenSource();
            var token = fadeBgmCancellationTokenSource.Token;

            FadeBgmTask = FadeAudio(BgmVolume, value => BgmVolume = value, targetVolume, duration, token);
        }
        
        /// <summary>
        /// 淡入或淡出环境音效
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位秒
        /// </param>
        public static void FadeBgs(float targetVolume, float duration)
        {
            fadeBgsCancellationTokenSource?.Cancel();
            fadeBgsCancellationTokenSource = new CancellationTokenSource();
            var token = fadeBgsCancellationTokenSource.Token;

            FadeBgsTask = FadeAudio(BgsVolume, value => BgsVolume = value, targetVolume, duration, token);
        }
        
        /// <summary>
        /// 淡入或淡出语音
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位秒
        /// </param>
        public static void FadeVoice(float targetVolume, float duration)
        {
            fadeVoiceCancellationTokenSource?.Cancel();
            fadeVoiceCancellationTokenSource = new CancellationTokenSource();
            var token = fadeVoiceCancellationTokenSource.Token;

            FadeVoiceTask = FadeAudio(VoiceVolume, value => VoiceVolume = value, targetVolume, duration, token);
        }
        
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void StopBgm() => BgmPlayer.Stop();

        /// <summary>
        /// 停止播放环境音效
        /// </summary>
        public static void StopBgs() => BgsPlayer.Stop();

        /// <summary>
        /// 停止播放语音
        /// </summary>
        public static void StopVoice() => VoicePlayer.Stop();

        /// <summary>
        /// 设置静音
        /// </summary>
        /// <param name="mute">
        /// 是否静音
        /// </param>
        public static void SetMute(bool mute)
        {
            if (mute)
            {
                originalBgmVolume = BgmVolume;
                originalBgsVolume = BgsVolume;
                originalSeVolume = SeVolume;
                originalVoiceVolume = VoiceVolume;

                BgmVolume = 0.0f;
                BgsVolume = 0.0f;
                SeVolume = 0.0f;
                VoiceVolume = 0.0f;
            }
            else if (IsMuted)
            {
                BgmVolume = originalBgmVolume;
                BgsVolume = originalBgsVolume;
                SeVolume = originalSeVolume;
                VoiceVolume = originalVoiceVolume;
            }

            IsMuted = mute;
        }

    }
}