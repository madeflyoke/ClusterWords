namespace Source.Modules.StateMachineModule.Scripts.Interfaces
{
    public interface IState
    {
        public void Enter(StateMachine stateMachine);
        public void Update(){}
        public void Exit(){}
    }
}