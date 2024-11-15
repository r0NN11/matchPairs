namespace Controller.Audio
{
	public interface IAudioManager
	{
		void InitializeAudioSource();
		void PlayFlipSound();
		void PlayMatchSound();
		void PlayMismatchSound();
		void PlayLevelEndSound();
	}
}

