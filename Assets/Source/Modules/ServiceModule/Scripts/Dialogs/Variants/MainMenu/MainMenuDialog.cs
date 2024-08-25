using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu
{
    public class MainMenuDialog : Dialog
    {
        [SerializeField] private LevelSelector _levelSelector;
        
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
            _levelSelector.Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}