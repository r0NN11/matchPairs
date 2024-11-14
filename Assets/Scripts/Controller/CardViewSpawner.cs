using System.Collections.Generic;
using Model;
using UnityEngine;
using View.Card;
using View.Level;
using Zenject;

namespace Controller
{
	public class CardViewSpawner : MonoBehaviour
	{
		private List<CardViewInject> _cardViewInjects = new List<CardViewInject>();
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
				_cardViewInjects.Add(_factoryCardInject.Create());
				_cardViewInjects[i].transform.SetParent(transform);
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
			Vector2 start = GetStartPosition(gridSize);
			float step = _cardSize.x;
			int index = 0;
			int cardViewIndex = 0;
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
					cardView.transform.localPosition = (Vector3) start + new Vector3(row, -col, 0) * step;
					cardView.transform.localScale=Vector3.one;
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

		private Vector2 GetStartPosition(Vector2Int gridSize)
		{
			Vector2 size = GetLevelSize(gridSize);
			float xPos = -(size.x - _cardSize.x) / 2;
			float yPos = (size.y - _cardSize.y) / 2;
			return new Vector2(xPos, yPos);
		}

		private Vector2 GetLevelSize(Vector2Int gridSize)
		{
			float levelWidth = _cardSize.x * gridSize.x;
			float levelHeight = _cardSize.y * gridSize.y;
			return new Vector2(levelWidth, levelHeight);
		}
	}
}