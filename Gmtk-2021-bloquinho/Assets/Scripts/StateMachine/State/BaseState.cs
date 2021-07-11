namespace StateMachine.State
{
    public class BaseState : IState
    {
        protected IStateMachineManager Manager { get; private set; }

        protected BaseState()
        {
        }
        
        public virtual void Init(IStateMachineManager manager)
        {
            Manager = manager;
        }
        
        public virtual void OnPauseGameKeyPressed()
        {
        }

        public virtual void OnEnterBlockReorderKeyPressed()
        {
        }

        public virtual void Suspend()
        {
        }

        public virtual void Resume()
        {
        }

        public virtual void Finish()
        {
        }
    }
}