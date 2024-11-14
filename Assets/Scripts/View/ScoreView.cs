using Model.Scores;
using TMPro;
using UnityEngine;
using Zenject;

namespace View
{
	public class ScoreView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _score;
		[SerializeField] private TMP_Text _turnsCount;

		[Inject]
		public ScoreView(IScoresManager scoresManager)
		{
			scoresManager.OnScoresChange += UpdateScoreText;
			scoresManager.OnTurnsCountChanges += UpdateTurnsCountText;
		}

		private void UpdateScoreText(int score)
		{
			_score.text = score.ToString();
		}

		private void UpdateTurnsCountText(int turnsCount)
		{
			_turnsCount.text = turnsCount.ToString();
		}
	}
}
