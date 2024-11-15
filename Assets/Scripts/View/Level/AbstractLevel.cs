using System;
using Save;
using View.Card;

namespace View.Level
{
	public abstract class AbstractLevel
	{
		private AbstractCardView[] _cards;
		private int _index;
		private bool[] _state;
		private ISaveSystem _saveSystem;
		private const string LEVEL_STATE_KEY_PREFIX = "level_";
		public int Index => _index;

		protected AbstractLevel(int index, ISaveSystem saveSystem, AbstractCardView[] cards)
		{
			_cards = cards;
			_index = index;
			_saveSystem = saveSystem;
		}

		public virtual bool CheckWin()
		{
			return !Array.Exists(_state, x => !x);
		}
		public virtual void SaveState(int cardIndex)
		{
			_state[cardIndex] = true;
			_saveSystem.SaveValue($"{LEVEL_STATE_KEY_PREFIX}{_index}", _state);
		}

		public virtual void LoadState()
		{
			_state = _saveSystem.LoadValue($"{LEVEL_STATE_KEY_PREFIX}{_index}", new bool[_cards.Length/2]);
			
			foreach (AbstractCardView card in _cards)
			{
				bool active = _state[card.Id];
				card.gameObject.SetActive(!active);
			}
		}
		
	}
}