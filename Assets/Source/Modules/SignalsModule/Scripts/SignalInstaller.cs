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
            SignalBusInstaller.Install(Container);
        }
    }
}
