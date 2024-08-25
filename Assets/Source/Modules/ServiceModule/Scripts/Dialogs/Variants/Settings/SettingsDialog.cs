using Source.Modules.AudioModule.Scripts;
using Source.Modules.StateMachineModule.Scripts;
using Source.Modules.StateMachineModule.Scripts.Variants;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.Settings
{
    public class SettingsDialog : PopupDialog
    {
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Button _toMenuButton;
                
        private AudioPlayer _audioPlayer;
        private StateMachine _stateMachine;
        
        [Inject]
        private void Construct(AudioPlayer audioPlayer, StateMachine stateMachine)
        {
            _audioPlayer = audioPlayer;
            _stateMachine = stateMachine;
        }
        
        protected override void Start()
        {
            base.Start();

            switch (_stateMachine.CurrentState)
            {
                case MainMenuState:
                    _toMenuButton.gameObject.SetActive(false);
                    break;
                case GameplayState:
                    _toMenuButton.gameObject.SetActive(true);
                    break;
            }
            
            RefreshView();
            
            _soundToggle.onValueChanged.AddListener(OnSoundToggleValueChanged);
            _musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged);
        }

        private void RefreshView()
        {
            _soundToggle.isOn = !_audioPlayer.SoundActive;
            _musicToggle.isOn = !_audioPlayer.MusicActive;
        }

        private void OnMusicToggleValueChanged(bool isDeactive)
        {
            if (isDeactive)
            {
                _audioPlayer.DeActiveMusic();
            }
            else
            {
                _audioPlayer.ActiveMusic();
            }
        }

        private void OnSoundToggleValueChanged(bool isDeactive)
        {
            if (isDeactive)
            {
                _audioPlayer.DeActiveSound();
            }
            else
            {
                _audioPlayer.ActiveSound();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _soundToggle.onValueChanged.RemoveListener(OnSoundToggleValueChanged);
        }
    } 
}