using Source.Modules.ServiceModule.Scripts;
using Source.Modules.ServiceModule.Scripts.Audio;
using Source.Modules.SignalsModule.Scripts;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts.Variants
{
    public class BootstrapperState : IState
    {
        private SignalBus _signalBus;
        private StateMachine _stateMachine;
        
        public BootstrapperState(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Enter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _signalBus.Subscribe<BootstrapperInitializedSignal>(OnBootstrapped);
        }

        private void OnBootstrapped()
        {
            SceneManager.LoadSceneAsync(1).completed += OnLoadedScene;
        }

        private void OnLoadedScene(AsyncOperation obj)
        {
            _signalBus.Fire<GameplaySceneLoadedSignal>();
            _stateMachine.SwitchState<MainMenuState>();
        }
    }
}
