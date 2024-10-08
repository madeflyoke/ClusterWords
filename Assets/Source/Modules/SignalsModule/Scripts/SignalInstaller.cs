using Zenject;

namespace Source.Modules.SignalsModule.Scripts
{
    public class SignalInstallers : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<LevelCompleteSignal>();
            Container.DeclareSignal<MoveToMainMenuSignal>();
            Container.DeclareSignal<LevelStartSignal>();
            Container.DeclareSignal<BootstrapperInitializedSignal>();
            Container.DeclareSignal<GameplaySceneLoadedSignal>();
            Container.DeclareSignal<MainMenuInitializedSignal>().OptionalSubscriber();
            SignalBusInstaller.Install(Container);
        }
    }
}
