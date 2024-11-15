using Save;
using View.Card;

namespace Model.Level
{
	public class StandardLevel : AbstractLevel
	{
		public StandardLevel(int index, ISaveSystem saveSystem, AbstractCardView[] cards) : base(index, saveSystem, cards)
		{
		}
	}
}
