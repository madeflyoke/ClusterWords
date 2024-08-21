using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Modules.ServiceModule.Scripts.S.Interfaces
{
    public interface IService : IDisposable
    {
        public UniTask Initialize(CancellationTokenSource cts);
    }
}
