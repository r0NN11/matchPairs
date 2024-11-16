using System.Collections.Generic;
using Model;
using Model.Level;
using UnityEngine;
using View.Card;
using Zenject;

namespace Controller
{
	public class CardViewHolder : MonoBehaviour
	{
		[SerializeField] private RectTransform _cardsContainer;
		[SerializeField] private float _cardsPadding = 5f;
		private readonly List<CardViewInject> _cardViewInjects = new List<CardViewInject>();
		private Vector2 _cardSize;
		private GameSettings _gameSettings;
		[Inject] private CardViewInject.FactoryCardInject _factoryCardInject;

		[Inject]
		public void Constructor(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		public List<CardViewInject> GetCardViewsInjects => _cardViewInjects;

		public void CreateCards()
		{
			for (int i = 0; i < _gameSettings.GetMaximumCardsCount; i++)
			{
				CardViewInject cardView = _factoryCardInject.Create();
				cardView.gameObject.SetActive(false);
				_cardViewInjects.Add(cardView);
				cardView.transform.SetParent(transform);
			}

			_cardSize = ((RectTransform) _cardViewInjects[0].transform).sizeDelta;
		}

		public AbstractCardView[] GetCardsForLevelConfig(LevelConfig levelConfig)
		{
			Vector2Int gridSize = levelConfig.GetGridSize;
			CardModel[] cardModels = levelConfig.GetCardModels;
			Sprite[] sprites = levelConfig.GetSprites;
			int emptySlotsCount = levelConfig.GetEmptySlotsCount;
			int cardsCount = gridSize.x * gridSize.y - emptySlotsCount;
			AbstractCardView[] cardViews = new AbstractCardView[cardsCount];

			int index = 0;
			int cardViewIndex = 0;
			float cardSize = Mathf.Min(_cardSize.x, _cardSize.y);
			Vector2 start = GetStartPosition(gridSize, cardSize);
			for (int row = 0; row < gridSize.x; row++)
			{
				for (int col = 0; col < gridSize.y; col++)
				{
					if (cardViewIndex == cardsCount)
					{
						break;
					}

					if (cardModels[index].GetId == -1)
					{
						index++;
						continue;
					}

					AbstractCardView cardView = cardViews[cardViewIndex] = _cardViewInjects[cardViewIndex];
					cardView.Setup(cardModels[index], sprites[index]);
					cardView.gameObject.SetActive(true);
					Transform cardViewTransform = cardView.transform;
					cardViewTransform.localPosition =
						(Vector3) start + new Vector3(row, -col, 0) * (cardSize + _cardsPadding);
					cardViewTransform.localScale = new Vector2(cardSize / _cardSize.x, cardSize / _cardSize.y);
					cardViewIndex++;
					index++;
				}
			}

			for (int i = cardViews.Length - 1; i < _cardViewInjects.Count; i++)
			{
				_cardViewInjects[i].gameObject.SetActive(false);
			}

			return cardViews;
		}
		
		private Vector2 GetStartPosition(Vector2Int gridSize,float cardSize)
		{
			float gridWidth = gridSize.x * cardSize + (gridSize.x - 1) * _cardsPadding;
			float gridHeight = gridSize.y * cardSize + (gridSize.y - 1) * _cardsPadding;
			float xPos = -(gridWidth - cardSize) / 2;
			float yPos = (gridHeight - cardSize) / 2;
			return new Vector2(xPos, yPos);
		}
	}
}