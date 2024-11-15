using Controller;
using Model.Scores;
using Save;
using View;
using View.Card;
using Zenject;

namespace Installer
{
	public class UntitledInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			CreateCardsFactory();
			Container.Bind<CardViewHolder>().FromInstance(FindObjectOfType<CardViewHolder>(true)).AsSingle();
			Container.Bind<IGameController>().To<GameController>().AsSingle();
			Container.Bind<IScoresManager>().To<ScoresManager>().AsSingle();
			Container.Bind<GameView>().FromInstance(FindObjectOfType<GameView>(true)).AsSingle();
		}

		private void CreateCardsFactory()
		{
			string card = "CardViewVariant";
			Container.BindFactory<CardViewInject, CardViewInject.FactoryCardInject>()
				.FromComponentInNewPrefabResource(card);
		}

		public void Awake()
		{
			Container.Resolve<ISaveSystem>();
			Container.Resolve<IGameController>();
			IScoresManager scoresManager = Container.Resolve<IScoresManager>();
			scoresManager.Load();
		}
	}
}