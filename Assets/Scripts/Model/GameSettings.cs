using System.Collections.Generic;
using UnityEngine;
using View.Level;

namespace Model
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levelsConfigs = new List<LevelConfig>();
        [SerializeField] private int _matchPoints = 1;
        [SerializeField] private int _comboMultiplier = 1;
        [SerializeField] private int _comboIncreaseStep = 3;
        [SerializeField] private float _cardsFlipTime = 0.5f;
        [SerializeField] private Vector2Int _maximumGridSize = new Vector2Int(6, 6);
        public List<LevelConfig> GetLevelsConfigs => _levelsConfigs;
        public void AddLevelConfig(LevelConfig levelConfig) => _levelsConfigs.Add(levelConfig);
        public int GetMatchPoints => _matchPoints;
        public int GetComboMultiplier => _comboMultiplier;
        public int GetComboIncreaseStep => _comboIncreaseStep;
        public float GetCardsFlipTime => _cardsFlipTime;
        public int GetMaximumCardsCount => _maximumGridSize.x * _maximumGridSize.y;
    }
}
