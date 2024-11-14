using View.Card;

namespace Controller
{
	public interface IGameController 
	{
		void LoadGame();
		public void CheckClick(AbstractCardView cardView);
		void EndGame();
	}
}
