using System;

namespace Model.Scores
{
	public interface IScoresManager
	{
		event Action<int> OnScoresChange;
		event Action<int> OnTurnsCountChanges;
		public event Action<int> OnComboCountChange;
		void Load();
		void LoadTurn();
		void UpdateTurn(int value);
		void LoadScore();
		void IncreaseScore();
		void LoadCombo();
		void UpdateCombo(int value);
	}
}
