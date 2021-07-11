namespace StateMachine.State
{
    public class BlockReorderState : BaseState
    {
        public override void OnPauseGameKeyPressed()
        {
            base.OnPauseGameKeyPressed();
            Manager.PushState(new GamePausedState());
        }

        public override void OnEnterBlockReorderKeyPressed()
        {
            base.OnEnterBlockReorderKeyPressed();
            Manager.PopState();
        }
    }
}