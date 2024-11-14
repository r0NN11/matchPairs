using System;
using Sirenix.OdinInspector;
using UnityEngine;
using View.Card;

namespace View.Level
{
	[Serializable]
	public class LevelConfig
	{
		[ReadOnly] 
		[SerializeField] private Vector2Int _gridSize;
		[SerializeField] private CardModel[] _cardModels;
		[SerializeField] private Sprite[] _sprites;
		[SerializeField] private int _emptySlotsCount;

		public LevelConfig(Vector2Int gridSize, CardModel[] cardModels, Sprite[] sprites, int emptySlotsCount)
		{
			_gridSize = gridSize;
			_sprites = sprites;
			_cardModels = cardModels;
			_emptySlotsCount = emptySlotsCount;
		}

		public Vector2Int GetGridSize => _gridSize;
		public CardModel[] GetCardModels => _cardModels;
		public Sprite[] GetSprites => _sprites;
		public int GetEmptySlotsCount => _emptySlotsCount;
	}
}
