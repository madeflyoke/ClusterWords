using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Modules.ServiceModule.Scripts.Audio;
using Source.Modules.ServiceModule.Scripts.Dialogs;
using Source.Modules.ServiceModule.Scripts.Interfaces;
using Source.Modules.ServiceModule.Scripts.Player;
using Source.Modules.ServiceModule.Scripts.Yandex;
using UnityEngine;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts
{
    public class ServicesHolder : IDisposable
    {
        private Dictionary<Type,IService> _services;
        private CancellationTokenSource _cts;
        private bool _isInitialized;
        private readonly DiContainer _container;

        public ServicesHolder(DiContainer container)
        {
            _container = container;
        }

        public async UniTask InitializeServices(Action onInitialized = null)
        {
            if (_isInitialized)
            {
                return;
            }
            
            TService AddService<TService>() where TService: IService
            {
                var instance = _container.Instantiate<TService>();
                _services.Add(typeof(TService), instance);
                return instance;
            }
            
            _services = new Dictionary<Type, IService>();
            
            //add all services below
            AddService<YandexService>();
            AddService<PlayerDataService>();
            AddService<DialogService>();
            AddService<AudioService>();

            _cts = new CancellationTokenSource();

            foreach (var service in _services)
            {
                Debug.Log($"Service {service.Value} started initialization...");
                await service.Value.Initialize(_cts);
                Debug.Log($"Service {service.Value} initialized");
            }
            
            onInitialized?.Invoke();
            _isInitialized = true;
        }
        
        public TService GetService<TService>() where TService: IService
        {
            return (TService) _services[typeof(TService)];
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}
