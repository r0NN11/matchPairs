using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View.Menu
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button _playButton;
		private const string GAME_SCENE_NAME = "GameScene";

		private void Awake()
		{
			_playButton.onClick.AddListener(LoadGameScene);
		}

		private void LoadGameScene()
		{
			SceneManager.LoadScene(GAME_SCENE_NAME);
		}
		
		private void OnDestroy()
		{
			_playButton.onClick.RemoveListener(LoadGameScene);
		}
	}
}