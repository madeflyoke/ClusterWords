using Source.Modules.SignalsModule.Scripts;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts
{
    public class PlayState : IState
    {
        private readonly SignalBus _signalBus;
        private StateMachine _stateMachine;
        
        [Inject]
        public PlayState(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Enter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _signalBus.Subscribe<LevelCompleteSignal>(OnLevelCompleteSignal);
        }

        private void OnLevelCompleteSignal()
        {
            _signalBus.Unsubscribe<LevelCompleteSignal>(OnLevelCompleteSignal);
           _stateMachine.SwitchState<LoadingToMenuState>();
        }
    }
}