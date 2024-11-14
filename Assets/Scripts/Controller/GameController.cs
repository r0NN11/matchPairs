using System;
using System.Collections.Generic;
using Controller.Audio;
using Model;
using Model.Scores;
using Save;
using UnityEngine.SceneManagement;
using View.Card;
using View.Level;
using Zenject;

namespace Controller
{
	public class GameController : IGameController
	{
		public event Action OnLevelEnd;
		private readonly GameSettings _gameSettings;
		private readonly IScoresManager _scoresManager;
		private readonly CardViewSpawner _cardViewSpawner;
		private readonly IAudioManager _audioManager;
		private readonly ISaveSystem _saveSystem;
		private List<LevelConfig> _levelsConfigs;
		private int _currentLevelId;
		private AbstractCardView _currentCardView;
		private const string CURRENT_LEVEL_SAVE_KEY = "current_level";

		[Inject]
		public GameController(GameSettings gameSettings, IScoresManager scoresManager, CardViewSpawner cardViewSpawner, IAudioManager audioManager,
			ISaveSystem saveSystem)
		{
			_gameSettings = gameSettings;
			_levelsConfigs = _gameSettings.GetLevelsConfigs;
			_scoresManager = scoresManager;
			_scoresManager.Load();
			_cardViewSpawner = cardViewSpawner;
			_audioManager = audioManager;
			_saveSystem = saveSystem;
			Initialize();
			LoadGame();
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
				_saveSystem.SaveValue(CURRENT_LEVEL_SAVE_KEY, _currentLevelId);
			}
			

		}

		public void LoadGame()
		{
			AbstractCardView[] levelCards = _cardViewSpawner.GetCardsForLevelConfig(_levelsConfigs[_currentLevelId]);
			AbstractLevel level = new StandardLevel(_currentLevelId,_saveSystem,levelCards);
			level.LoadState();
		}
		
		public void CheckClick(AbstractCardView cardView)
		{
			if (_currentCardView == null)
			{
				_currentCardView = cardView;
				return;
			}

			if (_currentCardView == cardView)
			{
				_currentCardView = null;
				return;
			}

			cardView.Flip(true);
			_scoresManager.UpdateTurn(1);
			CheckForMatch(cardView);
		}
		
		public void EndGame()
		{
			GoToMainMenu();
		}
		public void GoToMainMenu()
		{
			SceneManager.LoadScene(0);
		}

		private void CheckForMatch(AbstractCardView cardView)
		{
			if (_currentCardView.Id != cardView.Id)
			{
				FlipCards(cardView, false);
				return;
			}

			_currentCardView.gameObject.SetActive(false);
			cardView.gameObject.SetActive(false);
			_currentCardView = null;
			_scoresManager.IncreaseScore();
			CheckLevelWin();
		}

		private void CheckLevelWin()
		{
			if (_levelsConfigs[_currentLevelId + 1] != null)
			{
				OnLevelEnd?.Invoke();
			}
		}

		private void FlipCards(AbstractCardView cardView, bool up)
		{
			_currentCardView.Flip(up, _gameSettings.GetCardsFlipTime);
			cardView.Flip(up, _gameSettings.GetCardsFlipTime);
		}

	}
}
