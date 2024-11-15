using System;
using Controller;
using Model.Scores;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
	public class GameView : MonoBehaviour
	{
		public event Action OnNextButtonClick;
		public event Action OnExitButtonClick;
		public event Action OnResetButtonClick;
		[SerializeField] private Button _next;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _resetButton;
		[SerializeField] private TMP_Text _score;
		[SerializeField] private TMP_Text _turnsCount;

		[Inject]
		public void Constructor(IScoresManager scoresManager)
		{
			scoresManager.OnScoresChange += UpdateScoreText;
			scoresManager.OnTurnsCountChanges += UpdateTurnsCountText;
		}
		private void Awake()
		{
			_next.onClick.AddListener(OnNextButton);
			_closeButton.onClick.AddListener(OnExitButton);
			_resetButton.onClick.AddListener(OnResetButton);
		}

		public void ShowNextButton()
		{
			_next.gameObject.SetActive(true);
		}
		public void ShowResetButton()
		{
			_resetButton.gameObject.SetActive(true);
		}

		private void OnNextButton()
		{
			OnNextButtonClick?.Invoke();
			_next.gameObject.SetActive(false);
		}

		private void OnExitButton()
		{
			OnExitButtonClick?.Invoke();
		}

		private void OnResetButton()
		{
			_resetButton.gameObject.SetActive(false);
			OnResetButtonClick?.Invoke();
		}

		private void OnDestroy()
		{
			_next.onClick.RemoveListener(OnNextButton);
			_closeButton.onClick.RemoveListener(OnExitButton);
			_resetButton.onClick.RemoveListener(OnResetButton);
		}
		private void UpdateScoreText(int score)
		{
			_score.text = "Score" + score;
		}

		private void UpdateTurnsCountText(int turnsCount)
		{
			_turnsCount.text = "Turns" + turnsCount;
		}
	}
}