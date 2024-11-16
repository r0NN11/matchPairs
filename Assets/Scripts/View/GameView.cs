using System;
using DG.Tweening;
using Model;
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
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Button _nextButton;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _resetButton;
		[SerializeField] private TMP_Text _score;
		[SerializeField] private TMP_Text _comboCount;
		[SerializeField] private TMP_Text _turnsCount;
		private float _animationTime;
		private Tween _moveTween;

		[Inject]
		public void Constructor(GameSettings gameSettings, IScoresManager scoresManager)
		{
			scoresManager.OnScoresChange += UpdateScoreText;
			scoresManager.OnTurnsCountChanges += UpdateTurnsCountText;
			scoresManager.OnComboCountChange += UpdateComboCountText;
			_animationTime = gameSettings.GetButtonsAnimationTime;
		}

		private void Awake()
		{
			_nextButton.onClick.AddListener(OnNextButton);
			_closeButton.onClick.AddListener(OnExitButton);
			_resetButton.onClick.AddListener(OnResetButton);
		}

		public void ShowNextButton()
		{
			MakeButtonAnimation(_nextButton.transform);
		}

		public void ShowResetButton()
		{
			MakeButtonAnimation(_resetButton.transform);
		}

		private void MakeButtonAnimation(Transform buttonTransform)
		{
			buttonTransform.gameObject.SetActive(true);
			float canvasRectHeight = ((RectTransform) _canvas.transform).rect.height;
			buttonTransform.localPosition = new Vector2(buttonTransform.localPosition.x, canvasRectHeight);
			_moveTween = buttonTransform.DOLocalMoveY(0, _animationTime);
		}

		private void OnNextButton()
		{
			OnNextButtonClick?.Invoke();
			_nextButton.gameObject.SetActive(false);
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
			_moveTween.Kill();
			OnNextButtonClick = null;
			OnExitButtonClick = null;
			OnResetButtonClick = null;
			_nextButton.onClick.RemoveListener(OnNextButton);
			_closeButton.onClick.RemoveListener(OnExitButton);
			_resetButton.onClick.RemoveListener(OnResetButton);
		}

		private void UpdateScoreText(int score)
		{
			_score.text = "Score " + score;
		}

		private void UpdateTurnsCountText(int turnsCount)
		{
			_turnsCount.text = "Turns " + turnsCount;
		}

		private void UpdateComboCountText(int comboCount)
		{
			_comboCount.text = "Combo " + comboCount;
		}
	}
}