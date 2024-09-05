using System;
using Source.Modules.SignalsModule.Scripts;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class LevelContainer : IDisposable
    {
        public int SessionLevelsCompletedCount { get; private set; }
        
        public int CurrentLevelId { get; private set; }
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<LevelStartSignal>(OnLevelStart);
            _signalBus.Subscribe<LevelCompleteSignal>(OnLevelComplete);
        }

        private void OnLevelComplete()
        {
            SessionLevelsCompletedCount++;
        }

        private void OnLevelStart(LevelStartSignal args)
        {
            CurrentLevelId = args.LevelId;
        }
        
        public void Dispose()
        {
            _signalBus?.TryUnsubscribe<LevelStartSignal>(OnLevelStart);
            _signalBus?.TryUnsubscribe<LevelCompleteSignal>(OnLevelComplete);
        }
    }
}
