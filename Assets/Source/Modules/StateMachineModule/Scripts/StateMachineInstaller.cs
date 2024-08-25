using UnityEngine;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts
{
    public class StateMachineInstaller : MonoInstaller
    {
        [SerializeField] private StateMachine _stateMachine;
        
        public override void InstallBindings()
        {
            Container.Bind<StateFactory>().AsSingle().NonLazy();
            Container.Bind<StateMachine>().FromInstance(_stateMachine).AsSingle().NonLazy();
        }
    }
}