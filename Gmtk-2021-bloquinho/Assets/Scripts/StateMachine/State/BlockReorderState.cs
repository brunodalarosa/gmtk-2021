namespace StateMachine.State
{
    public class BlockReorderState : BaseState
    {
        public override void OnPauseGameKeyPressed()
        {
            Manager.PushState(new GamePausedState());
        }

        public override void OnEnterBlockReorderKeyPressed()
        {
            Manager.PopState();
        }
    }
}