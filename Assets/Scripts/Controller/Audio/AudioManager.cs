using UnityEngine;
using Zenject;

namespace Controller.Audio
{
    public class AudioManager : IAudioManager
    {
        [Inject]
        public AudioManager()
        {
            
        }
        public void PlayFlipSound()
        {
            throw new System.NotImplementedException();
        }

        public void PlayMatchSound()
        {
            throw new System.NotImplementedException();
        }

        public void PlayMismatchSound()
        {
            throw new System.NotImplementedException();
        }

        public void PlayGameOverSound()
        {
            throw new System.NotImplementedException();
        }
    }
}
