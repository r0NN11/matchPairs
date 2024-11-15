using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Level;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using View.Card;
using Random = UnityEngine.Random;

namespace Editor
{
	public class LevelGenerator : OdinEditorWindow
	{
		[Tooltip("GridSize e.g.: 2x2, 2x3")] [SerializeField]
		Vector2Int _gridSize;

		[SerializeField] private bool _randomGeneration;
		[SerializeField] private Sprite[] _sprites;
		[Title("Generation")] private const string GAME_SETTINGS_PATH = "Assets/Resources/GameSettings.asset";
		private Sprite _defaultSprite;

		[MenuItem("Game/LevelGenerator")]
		private static void OpenWindow()
		{
			GetWindow<LevelGenerator>().Show();
		}

		[ShowInInspector] [TableMatrix(SquareCells = true)]
		public Sprite[,] GridData = new Sprite[4, 4];

		[Button(ButtonSizes.Medium), GUIColor(0.5f, 0.5f, 0.8f)]
		public void RedrawGrid()
		{
			int rowsCount = _gridSize.x;
			int columnsCount = _gridSize.y;
			GridData = new Sprite[rowsCount, columnsCount];
			if (_randomGeneration)
			{
				if (_sprites.Length != rowsCount * columnsCount / 2)
				{
					Debug.Log("Sprites count does not correspond to grid size");
					return;
				}

				List<Sprite> spriteList = new List<Sprite>();
				foreach (Sprite sprite in _sprites)
				{
					spriteList.Add(sprite);
					spriteList.Add(sprite);
				}

				ShuffleList(spriteList);
				int index = 0;
				for (int i = 0; i < rowsCount; i++)
				{
					for (int j = 0; j < columnsCount; j++)
					{
						GridData[i, j] = spriteList[index++];
					}
				}
			}
		}

		[Button(ButtonSizes.Medium), GUIColor(0.5f, 0.5f, 0.8f)]
		public void GenerateLevelConfig()
		{
			if (_gridSize.x * _gridSize.y % 2 != 0 && !IsGridDataCorrect())
			{
				Debug.Log("Cards count should be even");
				return;
			}

			(CardModel[] cardModels, Sprite[] cardSprites, int emptySlotsCount) generatedSlots =
				GenerateSlots();
			LevelConfig levelConfig = new LevelConfig(_gridSize, generatedSlots.cardModels,
				generatedSlots.cardSprites, generatedSlots.emptySlotsCount);
			GameSettings gameSettings = Resources.Load<GameSettings>("GameSettings");
			if (gameSettings == null)
			{
				gameSettings = CreateInstance<GameSettings>();
				AssetDatabase.CreateAsset(gameSettings, GAME_SETTINGS_PATH);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			gameSettings.AddLevelConfig(levelConfig);
			EditorUtility.SetDirty(gameSettings);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private bool IsGridDataCorrect()
		{
			int emptySlotsCount = GridData.Cast<Sprite>().Count(sprite => sprite == null);
			return (GridData.Length - emptySlotsCount) % 2 == 0;
		}

		private (CardModel[] cardModels, Sprite[] sprites, int emptySlots) GenerateSlots()
		{
			int rowsCount = _gridSize.x;
			int columnsCount = _gridSize.y;
			(int[,] indexes, int emptyCount) slotsInfo = GetSlotsInfo(rowsCount, columnsCount);
			int emptySlots = slotsInfo.emptyCount;
			int[,] cardIndexes = slotsInfo.indexes;
			int slotsCount = rowsCount * columnsCount;
			CardModel[] cardModels = new CardModel[slotsCount];
			Sprite[] sprites = new Sprite[slotsCount];

			int index = 0;
			for (int row = 0; row < rowsCount; row++)
			{
				for (int col = 0; col < columnsCount; col++)
				{
					int cardId = cardIndexes[row, col];
					cardModels[index] = new CardModel(cardId);
					sprites[index] = GridData[row, col];
					index++;
				}
			}

			return (cardModels, sprites, emptySlots);
		}

		private Sprite GridCell(Rect rect, Sprite sprite)
		{
			EditorGUI.DrawRect(rect, Color.black);
			if (sprite != null)
			{
				GUI.DrawTexture(rect, sprite.texture);
			}

			return sprite;
		}

		private void ShuffleList(List<Sprite> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Sprite temp = list[i];
				int randomIndex = Random.Range(i, list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		private (int[,] indexes, int emptyCount) GetSlotsInfo(int rowsCount, int columnsCount)
		{
			int[,] indexes = new int[rowsCount, columnsCount];
			int emptyCount = 0;
			Dictionary<string, int> spriteIndexMap = new Dictionary<string, int>();
			int nextIndex = 0;

			for (int i = 0; i < rowsCount; i++)
			{
				for (int j = 0; j < columnsCount; j++)
				{
					Sprite gridCell = GridData[i, j];
					if (gridCell == null || string.IsNullOrEmpty(gridCell.name))
					{
						indexes[i, j] = -1;
						emptyCount++;
						continue;
					}

					if (!spriteIndexMap.TryGetValue(gridCell.name, out int index))
					{
						index = nextIndex;
						spriteIndexMap[gridCell.name] = nextIndex;
						nextIndex++;
					}

					indexes[i, j] = index;
				}
			}

			return (indexes, emptyCount);
		}
	}
}