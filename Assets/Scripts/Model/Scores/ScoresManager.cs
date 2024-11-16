using System;
using Save;
using Zenject;

namespace Model.Scores
{
    public class ScoresManager : IScoresManager
    {
        public event Action<int> OnScoresChange;
        public event Action<int> OnTurnsCountChanges;
        public event Action<int> OnComboCountChange;
        private int _score;
        private int _turnsCount;
        private int _comboCount;
        private const string SCORE_SAVE_KEY = "score";
        private const string TURNS_COUNT_SAVE_KEY = "turns_count";
        private const string COMBO_COUNT_SAVE_KEY = "combo_count";
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
            LoadCombo();
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
            UpdateCombo(1);
            _score += _gameSettings.GetMatchPoints *
                      (_gameSettings.GetComboMultiplier + _comboCount / _gameSettings.GetComboIncreaseStep);
            _saveSystem.SaveValue(SCORE_SAVE_KEY, _score);
            OnScoresChange?.Invoke(_score);
        }

        public void LoadCombo()
        {
            _comboCount = _saveSystem.LoadValue(COMBO_COUNT_SAVE_KEY, 0);
            OnComboCountChange?.Invoke(_comboCount);
        }

        public void UpdateCombo(int value)
        {
            if (value == 0)
            {
                _comboCount = 0;
            }
            else
            {
                _comboCount += value;
            }

            _saveSystem.SaveValue(COMBO_COUNT_SAVE_KEY, _comboCount);
            OnComboCountChange?.Invoke(_comboCount);
        }
    }
}
