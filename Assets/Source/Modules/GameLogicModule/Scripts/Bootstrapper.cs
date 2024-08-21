using Source.Modules.ServiceModule.Scripts;
using Source.Modules.SignalsModule.Scripts;
using Source.Modules.StateMachineModule.Scripts;
using UnityEngine;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts
{
    public class Bootstrapper : MonoBehaviour
    {
        [Inject] private ServicesHolder _servicesHolder;
        [Inject] private SignalBus _signalBus;
        
        private async void Start()
        {
            Application.targetFrameRate = 60;
            await _servicesHolder.InitializeServices();
            
            _signalBus.Fire(new BootstrapperInitializedSignal());
        }
        
        private void OnDestroy()
        {
            _servicesHolder?.Dispose();
        }
    }
}
