using Source.Modules.StateMachineModule.Scripts.Variants;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts
{
    public class StateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StateFactory>().AsSingle().NonLazy();
        //    Container.Bind<LoadingToGameplayState>().AsSingle().NonLazy();
          //  Container.Bind<GameplayState>().AsSingle().NonLazy();
          //  Container.Bind<EndGameState>().AsSingle().NonLazy();
        //    Container.Bind<MainMenuState>().AsSingle().NonLazy();
         //   Container.Bind<BootstrapperState>().AsSingle().NonLazy();
        }
    }
}