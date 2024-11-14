using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace View.Card
{
	[Serializable]
	public class CardModel
	{
		[ReadOnly]
		[SerializeField] private int _id;

		public CardModel(int id)
		{
			_id = id;
		}

		public int GetId => _id;
	}
}
