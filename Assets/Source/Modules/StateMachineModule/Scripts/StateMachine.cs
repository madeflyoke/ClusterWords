using System;
using System.Collections.Generic;
using Source.Modules.StateMachineModule.Scripts.Interfaces;
using Source.Modules.StateMachineModule.Scripts.Variants;
using UnityEngine;
using Zenject;

namespace Source.Modules.StateMachineModule.Scripts
{
    public class StateMachine : MonoBehaviour
    {
        public IState CurrentState { get; private set; }
        
        private readonly Dictionary<Type, IState> _cashedStates = new();
        private StateFactory _stateFactory;

        [Inject]
        public void Construct(StateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void Start()
        {
            SwitchState<BootstrapperState>();
        }
        
        private void Update()
        {
            CurrentState.Update();
        }
        
        public void SwitchState<T>() where T :IState
        {
            CurrentState?.Exit();
            if (_cashedStates.ContainsKey(typeof(T)) == false)
            {
                _cashedStates.Add(typeof(T), _stateFactory.CreateState<T>());
            }
            
            CurrentState = _cashedStates[typeof(T)];
            CurrentState?.Enter(this);
        }
    }
}
