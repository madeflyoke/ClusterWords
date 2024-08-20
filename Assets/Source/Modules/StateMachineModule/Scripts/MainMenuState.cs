using Source.Modules.DialogModule.Scripts;
using Source.Modules.SignalsModule.Scripts;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts
{
    public class MainMenuState : IState
    {
        private readonly DialogService _dialogService;
        private readonly SignalBus _signalBus;
        private StateMachine _stateMachine;
        
        public MainMenuState(DialogService dialogService,SignalBus signalBus)
        {
            _dialogService = dialogService;
            _signalBus = signalBus;
        }

        public void Enter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _dialogService.ShowDialog<MainMenuDialog>();
            _signalBus.Subscribe<LevelStartSignal>(OnLevelStartSignal);
        }

        public void Exit()
        {
            _signalBus.Unsubscribe<LevelStartSignal>(OnLevelStartSignal);
        }

        private void OnLevelStartSignal(LevelStartSignal signal)
        {
            _stateMachine.SwitchState<LoadingToGameplayState>();
        }
    }
}