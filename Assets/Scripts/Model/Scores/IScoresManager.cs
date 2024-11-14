using System;

namespace Model.Scores
{
	public interface IScoresManager
	{
		public event Action<int> OnScoresChange;
		public event Action<int> OnTurnsCountChanges;
		void Load();
		void LoadTurn();
		void UpdateTurn(int value);
		void LoadScore();
		void IncreaseScore();
		public void ResetCombo();
		void Reset();
	}
}
