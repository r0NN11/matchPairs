using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View.Menu
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button _playButton;

		private void Awake()
		{
			_playButton.onClick.AddListener(LoadGameScene);
		}

		private void LoadGameScene()
		{
			SceneManager.LoadScene(1);
		}
		
		private void OnDestroy()
		{
			_playButton.onClick.RemoveListener(LoadGameScene);
		}
	}
}