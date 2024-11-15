using View.Card;

namespace Controller
{
	public interface IGameController 
	{
		void LoadLevel();
		void ResetGame();
		void EndGame();
		public void CheckClick(AbstractCardView cardView);

	}
}
