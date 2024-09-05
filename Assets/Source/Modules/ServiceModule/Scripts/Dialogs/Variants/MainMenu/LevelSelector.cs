using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.ServiceModule.Scripts.Player;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private LevelSelectButton _levelSelectButtonPrefab;
        [SerializeField] private LevelsConfig _levelsConfig;
        private PlayerDataService _playerDataService;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(ServicesHolder servicesHolder, SignalBus signalBus)
        {
            _playerDataService = servicesHolder.GetService<PlayerDataService>();
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            var lastOpenedLevel = _playerDataService.LevelProgressHandler.GetLastCompletedLevel();
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
