using System;
using Agava.YandexGames;
using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants
{
    public class GameplayDialog : Dialog
    {
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private LevelLauncher _levelLauncher;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private ParticleSystem _winEffectPrefab;
        [SerializeField] private Transform _vfxPivot;
        private SignalBus _signalBus;
        private DialogService _dialogService;
        private LevelContainer _levelContainer;
        
        [Inject]
        public void Construct(SignalBus signalBus, ServicesHolder servicesHolder, LevelContainer levelContainer)
        {
            _signalBus = signalBus;
            _levelContainer = levelContainer;
            _dialogService = servicesHolder.GetService<DialogService>();
            _signalBus.Subscribe<LevelCompleteSignal>(OnLevelComplete);
        }
        
        protected override void Start()
        {
            base.Start();
            _nextLevelButton.gameObject.SetActive(false);
            
#if UNITY_EDITOR
            gameObject.name += Guid.NewGuid();
#endif
            
            _levelLauncher.LaunchLevel();
        }


        private void OnLevelComplete()
        {
            _signalBus.TryUnsubscribe<LevelCompleteSignal>(OnLevelComplete);

            Instantiate(_winEffectPrefab, _vfxPivot.position, Quaternion.identity).Play();
            
            if (_levelsConfig.GetLevelData(_levelContainer.CurrentLevelId+1)!=null)
            {
                _nextLevelButton.gameObject.SetActive(true);
                _nextLevelButton.onClick.AddListener(() =>
                {
                    _nextLevelButton.onClick.RemoveAllListeners();
                    if ((_levelContainer.CurrentLevelId+1)%5==0)
                    {
                        InterstitialAd.Show(onCloseCallback:x=> Invoke(nameof(NextLevelSet), 1f));
                        return;
                    }

                    NextLevelSet();
                });
            }
        }
        
        private void NextLevelSet()
        {
            _nextLevelButton.gameObject.SetActive(false);
            LoadNextLevel();
        }
        
        public override void Show(Action onComplete = null)
        {
            DialogCanvas.TransitionAnimationComponent.Open();
            base.Show(onComplete);
        }

        private void LoadNextLevel()
        {
            DialogCanvas.TransitionAnimationComponent.Close(() =>
            {
                _signalBus.Fire(new LevelStartSignal(_levelContainer.CurrentLevelId+1));
                _dialogService.HideDialog<GameplayDialog>();
                _dialogService.ShowDialog<GameplayDialog>(asFirstChild: true, asSingle: true);
            });
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<LevelCompleteSignal>(OnLevelComplete);
        }
    }
}
