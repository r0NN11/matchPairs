using System;

namespace Model.Scores
{
	public interface IScoresManager
	{
		event Action<int> OnScoresChange;
		event Action<int> OnTurnsCountChanges;
		void Load();
		void LoadTurn();
		void UpdateTurn(int value);
		void LoadScore();
		void IncreaseScore();
		public void ResetCombo();
	}
}
