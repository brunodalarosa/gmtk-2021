namespace StateMachine.State
{
    public class GamePausedState : BaseState
    {
        public override void Init(IStateMachineManager manager)
        {
            base.Init(manager);
            Manager.ShowPauseOverlay();
        }

        public override void Finish()
        {
            Manager.HidePauseOverlay();
        }

        public override void OnPauseGameKeyPressed()
        {
            Manager.PopState();
        }
    }
}