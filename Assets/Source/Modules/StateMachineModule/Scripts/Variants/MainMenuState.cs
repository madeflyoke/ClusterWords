using Source.Modules.ServiceModule.Scripts;
using Source.Modules.ServiceModule.Scripts.Dialogs;
using Source.Modules.ServiceModule.Scripts.Dialogs.Variants;
using Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu;
using Source.Modules.SignalsModule.Scripts;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts.Variants
{
    public class MainMenuState : IState
    {
        private readonly DialogService _dialogService;
        private readonly SignalBus _signalBus;
        private StateMachine _stateMachine;
        
        public MainMenuState(ServicesHolder servicesHolder,SignalBus signalBus)
        {
            _dialogService = servicesHolder.GetService<DialogService>();
            _signalBus = signalBus;
        }

        public void Enter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _dialogService.ShowSingleDialog<MainMenuDialog>();
            _signalBus.Subscribe<LevelStartSignal>(OnLevelStartSignal);
        }
        
        public void Exit()
        {
            _signalBus.Unsubscribe<LevelStartSignal>(OnLevelStartSignal);
        }

        private void OnLevelStartSignal(LevelStartSignal signal)
        {
            _stateMachine.SwitchState<GameplayState>();
        }
    }
}