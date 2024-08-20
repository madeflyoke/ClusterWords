using Source.Modules.GameLogicModule.Scripts.Utils;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Modules.StateMachineModule.Scripts
{
    public class LoadingToGameplayState : IState
    {
        private StateMachine _stateMachine;
        private AsyncOperation _asyncOperation;
        
        public void Enter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            SceneManager.LoadSceneAsync(GameConstants.Scenes.GAMEPLAY_SCENE_INDEX).completed += OnLoadedScene;
        }
        
        private void OnLoadedScene(AsyncOperation obj)
        {
            obj.completed -= OnLoadedScene;
            _stateMachine.SwitchState<PlayState>();
        }
    }
}