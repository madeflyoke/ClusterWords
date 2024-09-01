using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.ServiceModule.Scripts.Progress.Currency;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private LevelSelectButton _levelSelectButtonPrefab;
        [SerializeField] private LevelsConfig _levelsConfig;
        private ProgressService _progressService;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(ServicesHolder servicesHolder, SignalBus signalBus)
        {
            _progressService = servicesHolder.GetService<ProgressService>();
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            var lastOpenedLevel = _progressService.LevelProgressHandler.GetLastCompletedLevel();
            for (int i = 0; i < _levelsConfig.LevelDatasCount; i++)
            {
                var id = i;
                var button = Instantiate(_levelSelectButtonPrefab, transform);
                button.Initialize(id+1, i-1>lastOpenedLevel, ()=>
                {
                    _signalBus.Fire(new LevelStartSignal(id));
                });
            }
        }
        
    }
}
