using UnityEngine;

namespace Model
{
	[CreateAssetMenu(fileName = "AudioSettings", menuName = "Game/AudioSettings")]
	public class AudioSettings : ScriptableObject
	{
		[SerializeField] private AudioClip _flipSound;
		[SerializeField] private AudioClip _matchSound;
		[SerializeField] private AudioClip _misMatchSound;
		[SerializeField] private AudioClip _levelEndSound;
		public AudioClip GetFlipSound => _flipSound;
		public AudioClip GetMatchSound => _matchSound;
		public AudioClip GetMisMatchSound => _misMatchSound;
		public AudioClip GetLevelEndSound => _levelEndSound;
	}
}
