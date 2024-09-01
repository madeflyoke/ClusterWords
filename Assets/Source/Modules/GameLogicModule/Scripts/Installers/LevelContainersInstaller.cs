using Source.Modules.GameLogicModule.Scripts.Levels;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Installers
{
    public class LevelContainersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelContainer>().AsSingle().NonLazy();
        }
    }
}
