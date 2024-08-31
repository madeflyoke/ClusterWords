using System;
using Sirenix.OdinInspector;
using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants
{
    public class GameplayDialog : Dialog
    {
        [SerializeField] private LevelLauncher _levelLauncher;
        [SerializeField] private Button _nextLevelButton;
        private SignalBus _signalBus;
        private DialogService _dialogService;
        
        [Inject]
        public void Construct(SignalBus signalBus, ServicesHolder servicesHolder)
        {
            _signalBus = signalBus;
            _dialogService = servicesHolder.GetService<DialogService>();
            _signalBus.Subscribe<LevelStartSignal>(NextLevelStarted);
        }
        
        protected override void Start()
        {
            base.Start();
            #if UNITY_EDITOR
            gameObject.name += Guid.NewGuid();
            #endif
            _levelLauncher.LaunchLevel();
        }

        public override void Show(Action onComplete = null)
        {
            DialogCanvas.TransitionAnimationComponent.Open();
            base.Show(onComplete);
        }

        [Button]
        private void NextLevelStarted()
        {
            _signalBus.Unsubscribe<LevelStartSignal>(NextLevelStarted);
            DialogCanvas.TransitionAnimationComponent.Close(() =>
            {
                _dialogService.HideDialog<GameplayDialog>();
                _dialogService.ShowDialog<GameplayDialog>(asFirstChild: true, asSingle: true);
            });
        }
    }
}
