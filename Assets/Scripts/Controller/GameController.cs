using System.Collections.Generic;
using Controller.Audio;
using Model;
using Model.Level;
using Model.Scores;
using Save;
using UnityEngine.SceneManagement;
using View;
using View.Card;
using Zenject;

namespace Controller
{
	public class GameController : IGameController
	{
		private readonly GameSettings _gameSettings;
		private readonly IScoresManager _scoresManager;
		private readonly CardViewHolder _cardViewHolder;
		private readonly IAudioManager _audioManager;
		private readonly ISaveSystem _saveSystem;
		private readonly GameView _gameView;
		private readonly List<LevelConfig> _levelsConfigs;
		private int _currentLevelId;
		private AbstractLevel _currentLevel;
		private AbstractCardView _firstCardView;
		private AbstractCardView _secondCardView;
		private const string CURRENT_LEVEL_SAVE_KEY = "current_level";

		[Inject]
		public GameController(GameSettings gameSettings, IScoresManager scoresManager, CardViewHolder cardViewHolder,
			IAudioManager audioManager, ISaveSystem saveSystem, GameView gameView)
		{
			_gameSettings = gameSettings;
			_levelsConfigs = _gameSettings.GetLevelsConfigs;
			_scoresManager = scoresManager;
			_cardViewHolder = cardViewHolder;
			_audioManager = audioManager;
			_saveSystem = saveSystem;
			_gameView = gameView;
			SubscribeGameViewButtons();
			Initialize();
		}

		private void Initialize()
		{
			_cardViewHolder.CreateCards();
			foreach (CardViewInject card in _cardViewHolder.GetCardViewsInjects)
			{
				card.OnClick += CheckClick;
			}

			_currentLevelId = _saveSystem.LoadValue(CURRENT_LEVEL_SAVE_KEY, 0);
			if (_currentLevelId == 0)
			{
				SaveLevelId();
			}

			if (_currentLevelId == _levelsConfigs.Count)
			{
				_gameView.ShowResetButton();
				return;
			}

			LoadLevel();
		}

		public void LoadLevel()
		{
			AbstractCardView[] levelCards = _cardViewHolder.GetCardsForLevelConfig(_levelsConfigs[_currentLevelId]);
			_currentLevel = new StandardLevel(_currentLevelId, _saveSystem, levelCards);
			_currentLevel.LoadState();
			foreach (AbstractCardView card in levelCards)
			{
				card.Flip(true, true, _gameSettings.GetCardsStartFlipTime);
			}
		}
		public void EndGame()
		{
			GoToMainMenu();
		}

		public void ResetGame()
		{
			_currentLevelId = 0;
			_saveSystem.DeleteSave();
			_scoresManager.Load();
			_scoresManager.ResetCombo();
			LoadLevel();
		}
		public void CheckClick(AbstractCardView cardView)
		{
			if (_firstCardView == null)
			{
				_firstCardView = cardView;
				cardView.Flip(true);
				_audioManager.PlayFlipSound();
				return;
			}

			if (_secondCardView != null || _firstCardView == cardView || _secondCardView == cardView)
			{
				return;
			}

			_secondCardView = cardView;
			_audioManager.PlayFlipSound();
			_secondCardView.Flip(true, false, _gameSettings.GetCardsSFlipTime, (() =>
			{
				_scoresManager.UpdateTurn(1);
				CheckForMatch();
			}));
		}

		private void SubscribeGameViewButtons()
		{
			_gameView.OnNextButtonClick += LoadNextLevel;
			_gameView.OnExitButtonClick += GoToMainMenu;
			_gameView.OnResetButtonClick += ResetGame;
		}

		private void LoadNextLevel()
		{
			_scoresManager.ResetCombo();
			_scoresManager.UpdateTurn(0);
			LoadLevel();
		}
		
		private void GoToMainMenu()
		{
			SceneManager.LoadScene(0);
		}

		private void SaveLevelId()
		{
			_saveSystem.SaveValue(CURRENT_LEVEL_SAVE_KEY, _currentLevelId);
		}

		private void CheckForMatch()
		{
			if (_firstCardView.Id != _secondCardView.Id)
			{
				_audioManager.PlayMismatchSound();
				FlipCards(false);
				_firstCardView = null;
				_secondCardView = null;
				_scoresManager.ResetCombo();
				return;
			}
			_audioManager.PlayMatchSound();

			_firstCardView.gameObject.SetActive(false);
			_secondCardView.gameObject.SetActive(false);
			_currentLevel.SaveState(_firstCardView.Id);
			_firstCardView = null;
			_secondCardView = null;
			_scoresManager.IncreaseScore();
			CheckLevelWin();
		}

		private void CheckLevelWin()
		{
			if (!_currentLevel.CheckWin())
			{
				return;
			}

			_currentLevelId++;
			SaveLevelId();
			if (_currentLevelId != _levelsConfigs.Count)
			{
				_gameView.ShowNextButton();
			}
			else
			{
				_gameView.ShowResetButton();
			}
		}

		private void FlipCards(bool up)
		{
			_firstCardView.Flip(up);
			_secondCardView.Flip(up);
		}
	}
}