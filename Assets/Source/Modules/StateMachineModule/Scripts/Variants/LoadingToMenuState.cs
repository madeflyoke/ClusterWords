using Source.Modules.GameLogicModule.Scripts.Utils;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Modules.StateMachineModule.Scripts.Variants
{
    public class LoadingToMenuState : IState
    {
        private StateMachine _stateMachine;
        private AsyncOperation _asyncOperation;
        
        public void Enter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        private void OnLoadedScene(AsyncOperation obj)
        {
            obj.completed -= OnLoadedScene;
            _stateMachine.SwitchState<MainMenuState>();
        }
    }
}
