using System;
using System.Collections.Generic;
using Level;
using StateMachine.State;

namespace StateMachine
{
    public class StateMachineManager : IStateMachineManager
    {
        private Stack<IState> StateStack { get; }
        private LevelController LevelController { get; }

        public IState CurrentState => StateStack.Count > 0 ? StateStack.Peek() : null;

        public StateMachineManager(LevelController levelController)
        {
            LevelController = levelController;
            StateStack = new Stack<IState>();
        }

        public void PushState(IState newState)
        {
            CurrentState?.Suspend();
            StateStack.Push(newState);
            newState.Init(this);
        }

        public void PopState()
        {
            if (StateStack.Count == 0)
                throw new InvalidOperationException("Trying to PopState with empty Stack!");
            
            CurrentState.Finish();
            StateStack.Pop();
            CurrentState.Resume();
        }

        public void SwapState(IState newState)
        {
            CurrentState.Finish();
            StateStack.Pop();
            StateStack.Push(newState);
            CurrentState.Init(this);
        }

        public void ShowPauseOverlay()
        {
            LevelController.PauseGame();
        }

        public void HidePauseOverlay()
        {
            LevelController.UnpauseGame();
        }

        public void OnPauseGameKeyPressed()
        {
            CurrentState.OnPauseGameKeyPressed();
        }

        public void OnEnterBlockReorderKeyPressed()
        {
            CurrentState.OnEnterBlockReorderKeyPressed();
        }
    }
}