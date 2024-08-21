using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants
{
    public class MainMenuDialog : Dialog
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _settingButton;
        
        private SignalBus _signalBus;
        private DialogService _dialogService;
        
        [Inject]
        public void Construct(SignalBus signalBus, ServicesHolder servicesHolder)
        {
            _signalBus = signalBus;
            _dialogService = servicesHolder.GetService<DialogService>();
        }

        protected override void Start()
        {
            base.Start();
            _startGameButton.onClick.AddListener(OnStartGameButtonClick);
            _settingButton.onClick.AddListener(OnSettingButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _settingButton.onClick.RemoveListener(OnSettingButtonClick);
            _startGameButton.onClick.RemoveListener(OnStartGameButtonClick);
        }

        private void OnSettingButtonClick()
        {
            _dialogService.ShowDialog<SettingsDialog>();
        }

        private void OnStartGameButtonClick()
        {
            _signalBus.TryFire(new LevelStartSignal(0));
            Hide();
        }
    }
}