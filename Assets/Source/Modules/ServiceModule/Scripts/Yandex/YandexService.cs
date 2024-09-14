using System.Threading;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Source.Modules.GameLogicModule.Scripts.Utils;
using Source.Modules.ServiceModule.Scripts.Interfaces;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Yandex
{
    public class YandexService : IService
    {
        private const string HELPER_RESOURCE_PATH = "Services/Yandex/YandexServiceMonoBehaviourHelper";
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public async UniTask Initialize(CancellationTokenSource cts)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            await UniTask.Delay(1000, cancellationToken:cts.Token).SuppressCancellationThrow();
#else
            YandexGamesSdk.CallbackLogging = false;
            await YandexGamesSdk.Initialize().ToUniTask(cancellationToken:cts.Token).SuppressCancellationThrow();
            
#endif
            _signalBus.Subscribe<MainMenuInitializedSignal>(OnGameplayStarted);
        }

        private void OnGameplayStarted()
        {
            _signalBus.TryUnsubscribe<MainMenuInitializedSignal>(OnGameplayStarted);
            YandexGamesSdk.GameReady();
            Object.Instantiate(Resources.Load<YandexServiceMonoBehaviourHelper>(HELPER_RESOURCE_PATH)).Initialize();
        }
        
        public void Dispose()
        {
        }
    }
}
