using View.Card;

namespace Controller
{
	public interface IGameController 
	{
		void LoadGame();
		void ResetGame();
		void EndGame();
		public void CheckClick(AbstractCardView cardView);

	}
}
