using System.Collections.Generic;
using Model.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Model
{
	[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
	public class GameSettings : ScriptableObject
	{
		[Title("General settings")] [SerializeField]
		private List<LevelConfig> _levelsConfigs = new List<LevelConfig>();

		[SerializeField] private int _matchPoints = 1;
		[SerializeField] private int _comboMultiplier = 1;
		[SerializeField] private int _comboIncreaseStep = 3;

		[SerializeField] private Vector2Int _maximumGridSize = new Vector2Int(6, 6);

		[Title("Animation settings")] 
		[SerializeField] private float _cardsStartFlipTime = 0.6f;
		[SerializeField] private float _cardsFlipTime = 0.3f;
		[SerializeField] private float _buttonsAnimationTime = 0.5f;
		public List<LevelConfig> GetLevelsConfigs => _levelsConfigs;
		public void AddLevelConfig(LevelConfig levelConfig) => _levelsConfigs.Add(levelConfig);
		public int GetMatchPoints => _matchPoints;
		public int GetComboMultiplier => _comboMultiplier;
		public int GetComboIncreaseStep => _comboIncreaseStep;
		public int GetMaximumCardsCount => _maximumGridSize.x * _maximumGridSize.y;
		public float GetCardsStartFlipTime => _cardsStartFlipTime;
		public float GetCardsSFlipTime => _cardsFlipTime;
		public float GetButtonsAnimationTime => _buttonsAnimationTime;
	}
}