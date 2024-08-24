using Source.Modules.ServiceModule.Scripts;
using Source.Modules.ServiceModule.Scripts.Dialogs;
using Source.Modules.ServiceModule.Scripts.Dialogs.Variants;
using Source.Modules.SignalsModule.Scripts;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts.Variants
{
    public class GameplayState : IState
    {
        private readonly DialogService _dialogService;
        private readonly SignalBus _signalBus;
        private StateMachine _stateMachine;
        
        public GameplayState(ServicesHolder servicesHolder, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _dialogService = servicesHolder.GetService<DialogService>();
        }

        public void Enter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _signalBus.Subscribe<LevelCompleteSignal>(OnLevelCompleteSignal);
            _dialogService.ShowSingleDialog<GameplayDialog>();
        }

        private void OnLevelCompleteSignal()
        {
            _signalBus.Unsubscribe<LevelCompleteSignal>(OnLevelCompleteSignal);
           _stateMachine.SwitchState<MainMenuState>();
        }
    }
}