namespace StateMachine.State
{
    public class DefaultGameplayState : BaseState
    {
        public override void OnPauseGameKeyPressed()
        {
            base.OnPauseGameKeyPressed();
            Manager.PushState(new GamePausedState());
        }

        public override void OnEnterBlockReorderKeyPressed()
        {
            base.OnEnterBlockReorderKeyPressed();
            Manager.PushState(new BlockReorderState());
        }
    }
}