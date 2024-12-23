using UnityEngine;

namespace GensouLib.Unity.Audio
{
    /// <summary>
    /// 音频播放器 <br/>
    /// AudioPlayer
    /// </summary>
    /// <remarks>
    /// 用于初始化并向音频管理器提供音频源，请将其挂载到一个 GameObject 上 <br/>
    /// Used to initialize and provide audio sources to the audio manager, please attach it to a GameObject.
    /// </remarks>
    public class AudioPlayer : MonoBehaviour
    {
        /// <summary>
        /// 音频播放器的实例 <br/>
        /// The instance of the audio player.
        /// </summary>
        public static AudioPlayer Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            GetAudioSource();
        }
        private void GetAudioSource()
        {
            AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
            if (audioSources.Length == 0)
            {
                AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
                AudioManager.BGMSource = newAudioSource;
            }
            else
            {
                AudioManager.BGMSource = audioSources[0];
            }
            
            AudioManager.BGMSource.loop = true;
            AudioManager.BGMSource.playOnAwake = true;
            
            if (AudioManager.BGSSource == null && audioSources.Length < 2)
            {
                AudioManager.BGSSource = gameObject.AddComponent<AudioSource>();
            }
            else if (audioSources.Length >= 2)
            {
                AudioManager.BGSSource = audioSources[1];
            }

            AudioManager.BGSSource.loop = true;
            AudioManager.BGSSource.playOnAwake = false;

            if (AudioManager.SfxSource == null && audioSources.Length < 3)
            {
                AudioManager.SfxSource = gameObject.AddComponent<AudioSource>();
            }
            else if (audioSources.Length >= 3)
            {
                AudioManager.SfxSource = audioSources[2];
            }

            AudioManager.SfxSource.playOnAwake = false;
            AudioManager.SfxSource.loop = false;

            if (AudioManager.VoiceSource == null && audioSources.Length < 4)
            {
                AudioManager.VoiceSource = gameObject.AddComponent<AudioSource>();
            }
            else if (audioSources.Length >= 4)
            {
                AudioManager.VoiceSource = audioSources[3];
            }

            AudioManager.VoiceSource.loop = false;
            AudioManager.VoiceSource.playOnAwake = false;
        }
    }
}