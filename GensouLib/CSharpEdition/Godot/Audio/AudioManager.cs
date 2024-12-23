using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GensouLib.Godot.Audio
{
    /// <summary>
    /// 音频管理器 <br/>
    /// The audio manager.
    /// </summary>
    /// <remarks>
    /// 提供播放背景音乐，环境音，音效与语音的功能。<br/>
    /// Provides the ability to play background music, background sound, sound effects, and voice.<br/>
    /// 提供淡出音频和淡出并播放新背景音乐的功能。<br/>
    /// Provides the ability to fade out audio and fade out and play new background music.<br/>
    /// 设置音频时应使用音频文件作为音频流，而不是AudioStreamPlaylist等资源文件。<br/>
    /// When setting up audio, you should use an audio file as the audio stream, not an AudioStreamPlaylist resource file or something like that.<br/>
    /// </remarks>
    public partial class AudioManager : Node
    {
        /// <summary>
        /// AudioManager的单例，通过访问它来控制音频的播放 <br/>
        /// Singleton of AudioManager, which you can access to control the playback of audio.
        /// </summary>
        public static AudioManager Instance { get; private set; }
        
        /// <summary>
        /// 淡出并切换背景音乐完成时发出 <br/>
        /// Emitted when fading out and switching background music is completed
        /// </summary>
        [Signal]
        public delegate void BGMFadeCompletedAndChangedEventHandler();
        
        /// <summary>
        /// 播放器淡入淡出完成时发出 <br/>
        /// Emitted when the player's fade in and fade out is completed.
        /// </summary>
        /// <param name="player">
        /// 播放器名称 <br/>
        /// Player name. <br/>
        /// 会是以下三个值之一：<br/>
        /// Will be one of the following values: <br/>
        /// "BGM" - 背景音乐播放器 BGM player<br/>
        /// "BGS" - 环境音效播放器 BGS player<br/>
        /// "Voice" - 语音播放器 Voice player<br/>
        /// </param>
        [Signal]
        public delegate void FadeCompletedEventHandler(string player);

        /// <summary>
        /// 资源路径，默认是"res://Audio/"，将在该路径下加载音频文件。 <br/>
        /// Resource path, default is "res://Audio/", which will load audio files under this path.
        /// </summary>
        public string ResPath {get; set;} = "res://Audio/";
        
        /// <summary>
        /// 背景音乐播放器节点路径，默认是相对于根节点"AudioManager"的"BGMPlayer"<br/>
        /// BGM player node path, default to "BGMPlayer" relative to the root "AudioManager".
        /// </summary>
        public NodePath BGMNodePath {get; set;} = "BGMPlayer";
        
        /// <summary>
        /// 环境音效播放器节点路径，默认是相对于根节点"AudioManager"的"BGSPlayer"<br/>
        /// BGS player node path, default to "BGSPlayer" relative to the root "AudioManager".
        /// </summary>
        public NodePath BGSNodePath {get; set;} = "BGSPlayer";
        
        /// <summary>
        /// 音效播放器节点路径，默认是相对于根节点"AudioManager"的"SFXPlayer"<br/>
        /// SFX player node path, default to "SFXPlayer" relative to the root "AudioManager".
        /// </summary>
        public NodePath SFXNodePath {get; set;} = "SFXPlayer";
        
        /// <summary>
        /// 语音播放器节点路径，默认是相对于根节点"AudioManager"的"VoicePlayer"<br/>
        /// Voice player node path, default to "VoicePlayer" relative to the root "AudioManager".
        /// </summary>
        public NodePath VoiceNodePath {get; set;} = "VoicePlayer";
        
        /// <summary>
        /// 主音量，取决于默认的Master音频总线的音量，修改该属性即改变该音频总线的音量<br/>
        /// Master volume, depends on the default master audio bus volume, modify this property to change the audio bus volume.
        /// </summary>
        public float MasterVolume
        {
            get => Mathf.Max(Mathf.DbToLinear(AudioServer.GetBusVolumeDb(0)), 0.0f);
            set => AudioServer.SetBusVolumeDb(0, Mathf.LinearToDb(value));
        }
        
        /// <summary>
        /// 背景音乐音量，修改该属性即改变背景音乐的音量 <br/>
        /// BGM volume, modify this property to change the BGM volume.
        /// </summary>
        public float BGMVolume
        {
            get => Mathf.Max(Mathf.DbToLinear(BGMPlayer.VolumeDb), 0.0f); // 确保获取的值可读，而不是负无穷
            set => BGMPlayer.VolumeDb = Mathf.LinearToDb(value);
        }

        /// <summary>
        /// 环境音效音量，修改该属性即改变环境音效的音量 <br/>
        /// BGS volume, modify this property to change the BGS volume.
        /// </summary>
        public float BGSVolume
        {
            get => Mathf.Max(Mathf.DbToLinear(BGSPlayer.VolumeDb), 0.0f);
            set => BGSPlayer.VolumeDb = Mathf.LinearToDb(value);
        }

        /// <summary>
        /// 音效音量，修改该属性即改变音效的音量 <br/>
        /// SFX volume, modify this property to change the SFX volume.
        /// </summary>
        public float SFXVolume
        {
            get => Mathf.Max(Mathf.DbToLinear(SFXPlayer.VolumeDb), 0.0f);
            set => SFXPlayer.VolumeDb = Mathf.LinearToDb(value);
        }

        /// <summary>
        /// 语音音量，修改该属性即改变语音的音量 <br/>
        /// Voice volume, modify this property to change the voice volume.
        /// </summary>
        public float VoiceVolume
        {
            get => Mathf.Max(Mathf.DbToLinear(VoicePlayer.VolumeDb), 0.0f);
            set => VoicePlayer.VolumeDb = Mathf.LinearToDb(value);
        }
        
        private AudioStreamPlayer BGMPlayer;
        
        private AudioStreamPlayer BGSPlayer;
        
        private AudioStreamPlayer SFXPlayer;
        
        private AudioStreamPlayer VoicePlayer;

        /// <summary>
        /// 是否已静音 <br/>
        /// Whether the audio is muted.
        /// </summary>
        public bool IsMuted { get; private set; } = false;
        
        private float originalBGMVolume;
        
        private float originalBGSVolume;
        
        private float originalSFXVolume;
        
        private float originalVoiceVolume;
        
        private readonly Dictionary<string, AudioStream> audioPool = new();
        
        private readonly HashSet<string> AudioFileExtensions = new() 
        { 
            ".ogg",
            ".wav",
            ".mp3" 
        };

        public override void _Ready()
        {
            Instance = this;
            BGMPlayer = GetNode<AudioStreamPlayer>(BGMNodePath);
            BGSPlayer = GetNode<AudioStreamPlayer>(BGSNodePath);
            SFXPlayer = GetNode<AudioStreamPlayer>(SFXNodePath);
            VoicePlayer = GetNode<AudioStreamPlayer>(VoiceNodePath);
            if (BGMPlayer == null || 
                BGSPlayer == null || 
                SFXPlayer == null || 
                VoicePlayer == null)
            {
                GD.PushError("AudioManager: One or more nodes are missing.");
                return;
            }
            BGMPlayer.Autoplay = true;
            BGMPlayer.Playing = true;
            BGSPlayer.Autoplay = false;
            SFXPlayer.Autoplay = false;
            VoicePlayer.Autoplay = false;
        }

        private bool IsAudioFile(string name)
        {
            return AudioFileExtensions.Contains(System.IO.Path.GetExtension(name));
        }

        private void PlayAudio(AudioStreamPlayer player, string name)
        {
            if (!IsAudioFile(name))
            {
                GD.PushError($"AudioManager: Invalid audio file: {name}");
                return;
            }

            if (!audioPool.ContainsKey(name))
            {
                AudioStream audio = ResourceLoader.Load<AudioStream>($"{ResPath}{name}");
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
        /// Plays the background music.<br/>
        /// 如需循环播放请在文件导入选项勾选循环选项 <br/>
        /// To loop the music, check the "Loop" option in the file import options.
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 <br/>
        /// Audio file name, including extension, supports "ogg", "wav", "mp3" format.
        /// </param>
        public void PlayBGM(string name) => PlayAudio(BGMPlayer, name);

        /// <summary>
        /// 播放环境音效 <br/>
        /// Plays the background sound.<br/>
        /// 如需循环播放请在文件导入选项勾选循环选项 <br/>
        /// To loop the sound, check the "Loop" option in the file import options.
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 <br/>
        /// Audio file name, including extension, supports "ogg", "wav", "mp3" format.
        /// </param>
        public void PlayBGS(string name) => PlayAudio(BGSPlayer,  name);
        
        /// <summary>
        /// 播放语音 <br/>
        /// Plays the voice.<br/>
        /// 如需循环播放请在文件导入选项勾选循环选项 <br/>
        /// To loop the voice, check the "Loop" option in the file import options.
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 <br/>
        /// Audio file name, including extension, supports "ogg", "wav", "mp3" format.
        /// </param>
        public void PlayVoice(string name) => PlayAudio(VoicePlayer, name);

        /// <summary>
        /// 播放音效 <br/>
        /// Plays the sound effect.<br/>
        /// 如需循环播放请在文件导入选项勾选循环选项 <br/>
        /// To loop the sound, check the "Loop" option in the file import options.
        /// </summary>
        /// <param name="name">
        /// 音频文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 <br/>
        /// Audio file name, including extension, supports "ogg", "wav", "mp3" format.
        /// </param>
        public void PlaySFX(string name) => PlayAudio(SFXPlayer, name);

        /// <summary>
        /// 淡出当前背景音乐并播放新音乐 <br/>
        /// Fade out the current background music and plays a new one.
        /// </summary>
        /// <param name="newMusicName">
        /// 新音乐文件名，包含拓展名，支持"ogg", "wav", "mp3"格式 <br/>
        /// New music file name, including extension, supports "ogg", "wav", "mp3" format.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位秒 <br/>
        /// Fade out duration, in seconds.
        /// </param>
        /// <returns>
        /// 异步任务，返回 true 表示成功，false 表示失败 <br/>
        /// Asynchronous task, returns true if successful, false if failed.
        /// </returns>
        public async Task<bool> FadeOutAndPlayNewMusic(string newMusicName, float duration)
        {
            if (!IsAudioFile(newMusicName))
            {
                GD.PushError($"AudioManager: Invalid audio file: {newMusicName}");
                return false;
            }

            AudioStream newMusic;

            if (!audioPool.ContainsKey(newMusicName))
            {
                newMusic = ResourceLoader.Load<AudioStream>($"{ResPath}{newMusicName}");
            }
            else
            {
                newMusic = audioPool[newMusicName];
            }

            if (newMusic == null)
            {
                GD.PushError($"AudioManager: Could not load audio stream: {newMusicName}");
                return false;
            }

            float startVolume = BGMVolume;

            while (BGMVolume > 0.0f)
            {
                float volumeStep = startVolume * (float)GetProcessDeltaTime() / duration;
                if (float.IsNaN(BGMVolume - volumeStep) || BGMVolume - volumeStep < 0.0f)
                {
                    BGMVolume = 0.0f;
                    break;
                }
                BGMVolume -= volumeStep;
                await ToSignal(GetTree(), "process_frame");
            }

            BGMPlayer.Stop();
            BGMPlayer.Stream = newMusic;
            BGMVolume = startVolume;
            BGMPlayer.Play();
            EmitSignal(SignalName.BGMFadeCompletedAndChanged);
            return true;
        }

        private async Task<bool> FadeAudio(string player, float startVolume, Action<float> setVolume, float targetVolume, float duration)
        {
            if (targetVolume < 0.0f)
            {
                GD.PushError($"AudioManager: Invalid target volume: {targetVolume}");
                return false;
            }
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                elapsed += (float)GetProcessDeltaTime();
                float newVolume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                if (float.IsNaN(newVolume) || newVolume < 0.0f)
                {
                    setVolume(targetVolume);
                    break;
                }
                setVolume(newVolume);
                await ToSignal(GetTree(), "process_frame");
            }

            setVolume(targetVolume);
            EmitSignal(SignalName.FadeCompleted, player);
            return true;
        }

        /// <summary>
        /// 淡入或淡出背景音乐 <br/>
        /// Fade int or fade out the background music.
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量 <br/>
        /// Target volume.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位秒 <br/>
        /// Fade int or fade out duration, in seconds.
        /// </param>
        /// <returns>
        /// 异步任务，在淡入或淡出完成后返回 true，目标音量为负数时返回 false <br/>
        /// Asynchronous task, returns true when the fade in or fade out is complete, false if the target volume is negative.
        /// </returns>
        public async Task<bool> FadeBGM(float targetVolume, float duration) => await FadeAudio("BGM", BGMVolume, value => BGMVolume = value, targetVolume, duration);
        
        /// <summary>
        /// 淡入或淡出环境音效 <br/>
        /// Fade int or fade out the background sound.
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量 <br/>
        /// Target volume.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位秒 <br/>
        /// Fade int or fade out duration, in seconds.
        /// </param>
        /// <returns>
        /// 异步任务，在淡入或淡出完成后返回 true，目标音量为负数时返回 false <br/>
        /// Asynchronous task, returns true when the fade in or fade out is complete, false if the target volume is negative.
        /// </returns>
        public async Task<bool> FadeBGS(float targetVolume, float duration) => await FadeAudio("BGS", BGSVolume, value => BGSVolume = value, targetVolume, duration);
        
        /// <summary>
        /// 淡入或淡出语音 <br/>
        /// Fade in or fade out the voice.
        /// </summary>
        /// <param name="targetVolume">
        /// 目标音量 <br/>
        /// Target volume.
        /// </param>
        /// <param name="duration">
        /// 淡出时间，单位秒 <br/>
        /// Fade int or fade out duration, in seconds.
        /// </param>
        /// <returns>
        /// 异步任务，在淡入或淡出完成后返回 true，目标音量为负数时返回 false <br/>
        /// Asynchronous task, returns true when the fade in or fade out is complete, false if the target volume is negative.
        /// </returns>
        public async Task<bool> FadeVoice(float targetVolume, float duration) => await FadeAudio("Voice", VoiceVolume, value => VoiceVolume = value, targetVolume, duration);
        
        /// <summary>
        /// 停止播放背景音乐 <br/>
        /// Stops playing the background music.
        /// </summary>
        public void StopBGM() => BGMPlayer.Stop();

        /// <summary>
        /// 停止播放环境音效 <br/>
        /// Stops playing the background sound.
        /// </summary>
        public void StopBGS() => BGSPlayer.Stop();

        /// <summary>
        /// 停止播放语音 <br/>
        /// Stops playing the voice.
        /// </summary>
        public void StopVoice() => VoicePlayer.Stop();

        /// <summary>
        /// 设置静音 <br/>
        /// Sets the mute.
        /// </summary>
        /// <param name="mute">
        /// 是否静音 <br/>
        /// Whether to mute.
        /// </param>
        public void SetMute(bool mute)
        {
            if (mute)
            {
                originalBGMVolume = BGMVolume;
                originalBGSVolume = BGSVolume;
                originalSFXVolume = SFXVolume;
                originalVoiceVolume = VoiceVolume;

                BGMVolume = 0.0f;
                BGSVolume = 0.0f;
                SFXVolume = 0.0f;
                VoiceVolume = 0.0f;
            }
            else if (IsMuted)
            {
                BGMVolume = originalBGMVolume;
                BGSVolume = originalBGSVolume;
                SFXVolume = originalSFXVolume;
                VoiceVolume = originalVoiceVolume;
            }

            IsMuted = mute;
        }

    }
}