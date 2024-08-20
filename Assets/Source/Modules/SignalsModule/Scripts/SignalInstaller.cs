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
            SignalBusInstaller.Install(Container);
        }
    }
}
