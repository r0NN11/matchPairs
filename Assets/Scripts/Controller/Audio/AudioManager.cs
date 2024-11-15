using Zenject;
using UnityEngine;
using AudioSettings = Model.AudioSettings;

namespace Controller.Audio
{
    public class AudioManager : IAudioManager
    {
        private readonly AudioSettings _audioSettings;
        private AudioSource _audioSource;
        [Inject]
        public AudioManager(AudioSettings audioSettings)
        {
            _audioSettings = audioSettings;
            InitializeAudioSource();
        }
        public void InitializeAudioSource()
        {
            GameObject audioObject = new GameObject("AudioSourceObject");
            _audioSource = audioObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            Object.DontDestroyOnLoad(audioObject);
        }
        public void PlayFlipSound()
        {
            _audioSource.PlayOneShot(_audioSettings.GetFlipSound);
        }

        public void PlayMatchSound()
        {
            _audioSource.PlayOneShot(_audioSettings.GetMatchSound);
        }

        public void PlayMismatchSound()
        {
            _audioSource.PlayOneShot(_audioSettings.GetMisMatchSound);
        }

        public void PlayLevelEndSound()
        {
            _audioSource.PlayOneShot(_audioSettings.GetLevelEndSound);
        }
    }
}
