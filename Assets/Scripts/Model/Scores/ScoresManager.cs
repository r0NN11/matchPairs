using System;
using Save;
using Zenject;

namespace Model.Scores
{
    public class ScoresManager : IScoresManager
    {
        public event Action<int> OnScoresChange;
        public event Action<int> OnTurnsCountChanges;
        private int _score;
        private int _turnsCount;
        private int _combo;
        private const string SCORE_SAVE_KEY = "score";
        private const string TURNS_COUNT_SAVE_KEY = "turns_count";
        private readonly ISaveSystem _saveSystem;
        private readonly GameSettings _gameSettings;

        [Inject]
        public ScoresManager(GameSettings gameSettings, ISaveSystem saveSystem)
        {
            _gameSettings = gameSettings;
            _saveSystem = saveSystem;
        }

        public void Load()
        {
            LoadTurn();
            LoadScore();
        }

        public void LoadTurn()
        {
            _turnsCount = _saveSystem.LoadValue(TURNS_COUNT_SAVE_KEY, 0);
            OnTurnsCountChanges?.Invoke(_turnsCount);
        }

        public void UpdateTurn(int value)
        {
            if (value == 0)
            {
                _turnsCount = 0;
            }
            else
            {
                _turnsCount += value;
            }

            _saveSystem.SaveValue(TURNS_COUNT_SAVE_KEY, _turnsCount);
            OnTurnsCountChanges?.Invoke(_turnsCount);
        }

        public void LoadScore()
        {
            _score = _saveSystem.LoadValue(SCORE_SAVE_KEY, 0);
            OnScoresChange?.Invoke(_score);
        }

        public void IncreaseScore()
        {
            _combo++;
            _score += _gameSettings.GetMatchPoints *
                      (_gameSettings.GetComboMultiplier + _combo / _gameSettings.GetComboIncreaseStep);
            _saveSystem.SaveValue(SCORE_SAVE_KEY, _score);
            OnScoresChange?.Invoke(_score);
        }

        public void ResetCombo()
        {
            _combo = 0;
        }
    }
}
