namespace StateMachine.State
{
    public interface IState : IGameInputListener
    {
        void Suspend();
        void Init(IStateMachineManager manager);
        void Resume();
        void Finish();
    }
}