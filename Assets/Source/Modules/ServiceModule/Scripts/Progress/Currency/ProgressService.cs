using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Modules.ServiceModule.Scripts.Interfaces;

namespace Source.Modules.ServiceModule.Scripts.Progress.Currency
{
    public class ProgressService : IService
    {
        public LevelProgressHandler LevelProgressHandler { get; private set; }
        
        public UniTask Initialize(CancellationTokenSource cts)
        {
            LevelProgressHandler = new LevelProgressHandler();
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
