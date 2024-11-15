using Controller.Audio;
using Model;
using Save;
using Zenject;

namespace Installer
{
	public class ProjectInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<GameSettings>().FromScriptableObjectResource("GameSettings").AsSingle();
			Container.Bind<AudioSettings>().FromScriptableObjectResource("AudioSettings").AsSingle();
			Container.Bind<ISaveSystem>().To<SaveSystem>().AsSingle();
			Container.Bind<IAudioManager>().To<AudioManager>().AsSingle();
		}
		public void Awake()
		{
			Container.Resolve<ISaveSystem>();
			Container.Resolve<IAudioManager>();
		}
	}
}
