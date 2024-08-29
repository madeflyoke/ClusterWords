using Source.Modules.AudioModule.Scripts;
using Source.Modules.ServiceModule.Scripts.Dialogs;
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
        private AudioPlayer _audioPlayer;

        [Inject]
        private void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

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
            _audioPlayer.PlayAsMusic(SoundType.MAIN_MUSIC);
            _stateMachine.SwitchState<MainMenuState>();
        }
    }
}
