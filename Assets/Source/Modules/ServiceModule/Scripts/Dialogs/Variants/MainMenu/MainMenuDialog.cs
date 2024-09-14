using System;
using Source.Modules.ServiceModule.Scripts.Dialogs.Variants.Settings;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu
{
    public class MainMenuDialog : Dialog
    {
        [SerializeField] private SettingsButton _settingsButton;
        [SerializeField] private LevelSelector _levelSelector;

        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public override void Show(Action onComplete = null)
        {
            _levelSelector.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
            _levelSelector.Initialize();
            
            DialogCanvas.TransitionAnimationComponent.Close(()=>
            {
                _levelSelector.gameObject.SetActive(true);
                _settingsButton.gameObject.SetActive(true);
                _signalBus.Fire<MainMenuInitializedSignal>();
                onComplete?.Invoke();
            });
        }

        public override void Hide()
        {
            _levelSelector.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
            base.Hide();
        }
    }
}