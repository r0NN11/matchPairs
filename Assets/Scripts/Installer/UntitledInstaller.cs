using Controller;
using Controller.Audio;
using Model;
using Model.Scores;
using Save;
using UnityEngine;
using View;
using View.Card;
using Zenject;
using AudioSettings = Model.AudioSettings;

namespace Installer
{
	public class UntitledInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<GameSettings>().FromScriptableObjectResource("GameSettings").AsSingle();
			Container.Bind<AudioSettings>().FromScriptableObjectResource("AudioSettings").AsSingle();
			CreateCardsFactory();
			Container.Bind<CardViewSpawner>().FromInstance(FindObjectOfType<CardViewSpawner>(true)).AsSingle();
			Container.Bind<ISaveSystem>().To<SaveSystem>().AsSingle();
			Container.Bind<IGameController>().To<GameController>().AsSingle();
			Container.Bind<IScoresManager>().To<ScoresManager>().AsSingle();
			Container.Bind<IAudioManager>().To<AudioManager>().AsSingle();
			Container.Bind<GameView>().FromInstance(FindObjectOfType<GameView>(true)).AsSingle();

		}
		private void CreateCardsFactory()
		{
			string CARD = "CardViewVariant";
			Container.BindFactory<CardViewInject, CardViewInject.FactoryCardInject>()
				.FromComponentInNewPrefabResource(CARD);
		}
		public override void Start()
		{
			base.Start();
			Container.Resolve<ISaveSystem>();
			Container.Resolve<IGameController>();
			Container.Resolve<IAudioManager>();
			IScoresManager scoresManager = Container.Resolve<IScoresManager>();
			scoresManager.Load();
		}
	}
}
