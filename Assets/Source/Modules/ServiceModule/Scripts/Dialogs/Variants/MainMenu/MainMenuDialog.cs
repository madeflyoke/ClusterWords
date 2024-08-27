using System;
using Source.Modules.ServiceModule.Scripts.Dialogs.Variants.Settings;
using Source.Modules.ServiceModule.Scripts.Dialogs.Visual;
using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu
{
    public class MainMenuDialog : Dialog
    {
        [SerializeField] private SettingsButton _settingsButton;
        [SerializeField] private LevelSelector _levelSelector;
        [SerializeField] private TransitionAnimation _transitionAnimation;

        private SignalBus _signalBus;
        private DialogService _dialogService;
        
        [Inject]
        public void Construct(SignalBus signalBus, ServicesHolder servicesHolder)
        {
            _signalBus = signalBus;
            _dialogService = servicesHolder.GetService<DialogService>();
        }
        
        public override void Show(Action onComplete = null)
        {
            _levelSelector.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);

            _transitionAnimation.Initialize();
            _transitionAnimation.InstantOpen();
            
            _transitionAnimation.Close(()=>
            {
                _levelSelector.Initialize();
                _levelSelector.gameObject.SetActive(true);
                _settingsButton.gameObject.SetActive(true);
                onComplete?.Invoke();
            });
        }

        public override void Hide()
        {
            _levelSelector.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
            _transitionAnimation.Open(()=>base.Hide());
        }
    }
}