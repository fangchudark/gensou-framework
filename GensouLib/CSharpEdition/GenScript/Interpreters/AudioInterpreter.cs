#if UNITY_5_3_OR_NEWER
using GensouLib.Unity.Audio;
using GensouLib.Unity.Core;

#endif
using System;

namespace GensouLib.GenScript.Interpreters
{
    /// <summary>
    /// 音频命令解释器
    /// </summary>
    public class AudioInterpreter : BaseInterpreter
    {
        /// <summary>
        /// 播放BGM命令
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="volume">音量</param>
        /// <param name="duration">淡入淡出时间</param>
        public static void ParseBgmCommand(
            string param,
            float volume = 1.0f,
            float duration = 0.0f
        )
            => ParseAudioCommand(
                param,
                volume,
                duration,
                VisualNoveCore.BgmPath,
                async (v, d) => await AudioManager.FadeBgm(v, d),
                filePath => AudioManager.PlayBgm(filePath),
                vol => AudioManager.BgmVolume = vol
            );

        /// <summary>
        /// 播放BGS命令
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="volume">音量</param>
        /// <param name="duration">淡入淡出时间</param>
        public static void ParseBgsCommand(
            string param,
            float volume = 1.0f,
            float duration = 0.0f
        )
            => ParseAudioCommand(
                param,
                volume,
                duration,
                VisualNoveCore.VocalPath,
                async (v, d) => await AudioManager.FadeBgs(v, d),
                filePath => AudioManager.PlayBgs(filePath),
                vol => AudioManager.BgsVolume = vol
            );

        /// <summary>
        /// 播放音效命令
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="volume">音量</param>
        public static void ParseSeCommand(string param, float volume = 1.0f)
            => ParseAudioCommand(
                param,
                volume,
                VisualNoveCore.VocalPath,
                vol => AudioManager.SeVolume = vol,
                filePath => AudioManager.PlaySe(filePath)
            );

        /// <summary>
        /// 播放语音命令
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="volume">音量</param>
        public static void ParseVoiceCommand(string param, float volume = 1.0f)
            => ParseAudioCommand(
                param,
                volume,
                VisualNoveCore.VocalPath,
                vol => AudioManager.VoiceVolume = vol,
                filePath => AudioManager.PlayVoice(filePath)
            );
                

        private static void ParseAudioCommand(
            string param,
            float volume,
            string pathPrefix,
            Action<float> setVolume,
            Action<string> playAudio
        )
        {
            string path = param;
#if UNITY_5_3_OR_NEWER
#if ENABLE_ADDRESSABLES == false
            path = string.Join("/", pathPrefix, param); // 拼接路径
#endif
#endif
            setVolume(volume);
            playAudio(path);
        }

        private static void ParseAudioCommand(
            string param, 
            float volume, 
            float duration, 
            string pathPrefix, 
            Action<float, float> fadeAudio,
            Action<string> playAudio,
            Action<float> setVolume
        )
        {
            
            if (param == "none")
            {
                if (AudioManager.Fading)
                {
                    AudioManager.StopFade();
                }
                // 淡出音乐
                fadeAudio(0.0f, duration);
                return;
            }

            string path = param;
#if UNITY_5_3_OR_NEWER
#if ENABLE_ADDRESSABLES == false
            path = string.Join("/", pathPrefix, param); // 拼接路径
#endif
#endif

            if (duration > 0.0f)
            {
                // 淡入音乐
                setVolume(0.0f); // 初始音量设为0
                playAudio(path); // 播放音乐
                if (AudioManager.Fading)
                {
                    AudioManager.StopFade();
                }
                fadeAudio(volume, duration); // 开始淡入
            }
            else
            {
                // 直接播放音乐
                setVolume(volume); // 设置音量
                playAudio(path);
            }
            
        }

    }
}