using Save;
using View.Card;

namespace View.Level
{
	public class StandardLevel : AbstractLevel
	{
		public StandardLevel(int index, ISaveSystem saveSystem, AbstractCardView[] cards) : base(index, saveSystem, cards)
		{
		}
	}
}
