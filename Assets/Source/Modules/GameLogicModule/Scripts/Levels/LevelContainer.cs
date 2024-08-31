using Source.Modules.SignalsModule.Scripts;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class LevelContainer
    {
        public int CurrentLevelId { get; private set; }
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<LevelStartSignal>(OnLevelStart);
        }

        private void OnLevelStart(LevelStartSignal args)
        {
            CurrentLevelId = args.LevelId;
        }
    }
}
