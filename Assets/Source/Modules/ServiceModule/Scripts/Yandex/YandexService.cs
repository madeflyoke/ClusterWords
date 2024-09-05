using System.Threading;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Source.Modules.ServiceModule.Scripts.Interfaces;

namespace Source.Modules.ServiceModule.Scripts.Yandex
{
    public class YandexService : IService
    {
        public async UniTask Initialize(CancellationTokenSource cts)
        {
            YandexGamesSdk.CallbackLogging = false;
            
#if !UNITY_WEBGL || UNITY_EDITOR
            await UniTask.Delay(1000, cancellationToken:cts.Token).SuppressCancellationThrow();
#else
            await YandexGamesSdk.Initialize().ToUniTask(cancellationToken:cts.Token).SuppressCancellationThrow();
#endif
            
        }

        public void Dispose()
        {
        }
    }
}
