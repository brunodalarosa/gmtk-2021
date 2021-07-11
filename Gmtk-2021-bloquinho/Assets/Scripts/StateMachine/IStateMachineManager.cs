using StateMachine.State;

namespace StateMachine
{
    public interface IStateMachineManager : IGameInputListener
    {
        IState CurrentState { get; }
        
        void PushState(IState newState);
        void PopState();
        void SwapState(IState newState);

        void ShowPauseOverlay();
        void HidePauseOverlay();
    }
}