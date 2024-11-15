using System;
using System.Collections.Generic;
using Controller.Audio;
using Model;
using Model.Scores;
using Save;
using UnityEngine.SceneManagement;
using View;
using View.Card;
using View.Level;
using Zenject;

namespace Controller
{
	public class GameController : IGameController
	{
		private readonly GameSettings _gameSettings;
		private readonly IScoresManager _scoresManager;
		private readonly CardViewSpawner _cardViewSpawner;
		private readonly IAudioManager _audioManager;
		private readonly ISaveSystem _saveSystem;
		private readonly GameView _gameView;
		private List<LevelConfig> _levelsConfigs;
		private int _currentLevelId;
		private AbstractLevel _currentLevel;
		private AbstractCardView _firstCardView;
		private AbstractCardView _secondCardView;
		private const string CURRENT_LEVEL_SAVE_KEY = "current_level";

		[Inject]
		public GameController(GameSettings gameSettings, IScoresManager scoresManager, CardViewSpawner cardViewSpawner,
			IAudioManager audioManager, ISaveSystem saveSystem, GameView gameView)
		{
			_gameSettings = gameSettings;
			_levelsConfigs = _gameSettings.GetLevelsConfigs;
			_scoresManager = scoresManager;
			_cardViewSpawner = cardViewSpawner;
			_audioManager = audioManager;
			_saveSystem = saveSystem;
			_gameView = gameView;
			SubscribeGameViewButtons();
			Initialize();
		}

		private void Initialize()
		{
			_cardViewSpawner.CreateCards();
			foreach (CardViewInject card in _cardViewSpawner.GetCardViewsInjects)
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

			LoadGame();
		}

		public void LoadGame()
		{
			AbstractCardView[] levelCards = _cardViewSpawner.GetCardsForLevelConfig(_levelsConfigs[_currentLevelId]);
			_currentLevel = new StandardLevel(_currentLevelId, _saveSystem, levelCards);
			_currentLevel.LoadState();
			foreach (AbstractCardView card in levelCards)
			{
				card.Flip(true, true, _gameSettings.GetCardsFlipTime);
			}
		}

		private void SubscribeGameViewButtons()
		{
			_gameView.OnNextButtonClick += LoadGame;
			_gameView.OnExitButtonClick += GoToMainMenu;
			_gameView.OnResetButtonClick += ResetGame;
		}


		public void CheckClick(AbstractCardView cardView)
		{
			if (_firstCardView == null)
			{
				_firstCardView = cardView;
				cardView.Flip(true);
				return;
			}

			if (_secondCardView != null || _firstCardView == cardView || _secondCardView == cardView)
			{
				return;
			}

			_secondCardView = cardView;
			_secondCardView.Flip(true, false, _gameSettings.GetCardsFlipTime, (() =>
			{
				_scoresManager.UpdateTurn(1);
				CheckForMatch();
			}));
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
			LoadGame();
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
				FlipCards(false);
				_firstCardView = null;
				_secondCardView = null;
				return;
			}

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

			if (_currentLevelId + 1 != _levelsConfigs.Count)
			{
				_currentLevelId++;
				SaveLevelId();
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