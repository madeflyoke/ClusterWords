using Source.Modules.ServiceModule.Scripts.Audio;
using Source.Modules.ServiceModule.Scripts.Player;
using Source.Modules.ServiceModule.Scripts.Player.Settings;
using Source.Modules.SignalsModule.Scripts;
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
        private SignalBus _signalBus;
        private SettingsHandler _settingsHandler;
        
        [Inject]
        private void Construct(StateMachine stateMachine, SignalBus signalBus, ServicesHolder servicesHolder)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            _audioPlayer = servicesHolder.GetService<AudioService>().AudioPlayer;
            _settingsHandler = servicesHolder.GetService<PlayerDataService>().SettingsHandler;
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
                    _toMenuButton.onClick.AddListener(() =>
                    {
                        _signalBus.Fire(new MoveToMainMenuSignal());
                        Hide();
                    });
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
                _audioPlayer.DeActivateMusic();
            }
            else
            {
                _audioPlayer.ActivateMusic();
            }
            
            _settingsHandler.SaveMusicStatus(!isDeactive);
        }

        private void OnSoundToggleValueChanged(bool isDeactive)
        {
            if (isDeactive)
            {
                _audioPlayer.DeActivateSound();
            }
            else
            {
                _audioPlayer.ActivateSound();
            }
            
            _settingsHandler.SaveSoundStatus(!isDeactive);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _soundToggle.onValueChanged.RemoveListener(OnSoundToggleValueChanged);
            _musicToggle.onValueChanged.RemoveListener(OnMusicToggleValueChanged);
            _toMenuButton.onClick.RemoveAllListeners();
        }
    } 
}