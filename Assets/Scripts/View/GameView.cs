using Controller;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
	public class GameView : MonoBehaviour
	{
		[SerializeField] private Button _next;
		[SerializeField] private Button _closeButton;
		
		private void Awake()
		{
			_next.onClick.AddListener(HideNextButton);
			_closeButton.onClick.AddListener(GoToMainMenu);
		}

		private void ShowNextButton()
		{
			gameObject.SetActive(true);
		}

		private void HideNextButton()
		{
			gameObject.SetActive(false);

		}

		private void GoToMainMenu()
		{

		}

		private void OnDestroy()
		{
			_next.onClick.RemoveListener(HideNextButton);
			_closeButton.onClick.RemoveListener(GoToMainMenu);
		}
	}
}