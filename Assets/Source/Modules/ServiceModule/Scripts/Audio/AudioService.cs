using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Modules.ServiceModule.Scripts.Interfaces;
using Source.Modules.ServiceModule.Scripts.Player;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Audio
{
    public class AudioService : IService
    {
        private const string AUDIO_RESOURCES_PATH = "Audio";
        
        public AudioPlayer AudioPlayer { get; private set; }
        private DiContainer _diContainer;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(DiContainer diContainer, SignalBus signalBus)
        {
            _diContainer = diContainer;
            _signalBus = signalBus;
        }
        
        public UniTask Initialize(CancellationTokenSource cts)
        {
            var audioContainer = Resources.Load<AudioContainer>(AUDIO_RESOURCES_PATH+"/"+nameof(AudioContainer));
            AudioPlayer = new AudioPlayer(audioContainer.AudioClips, audioContainer.AudioSourcePrefab, 
                _diContainer.Resolve<ServicesHolder>().GetService<PlayerDataService>().SettingsHandler);
            _signalBus.Subscribe<GameplaySceneLoadedSignal>(OnGameplayStarted);
            
            return UniTask.CompletedTask;
        }

        private void OnGameplayStarted()
        {
            AudioPlayer.PlayAsMusic(SoundType.MAIN_MUSIC, .5f);
        }
        
        public void Dispose()
        {
            
        }
    }
}
