namespace StateMachine.State
{
    public class DefaultGameplayState : BaseState
    {
        public override void OnPauseGameKeyPressed()
        {
            Manager.PushState(new GamePausedState());
        }

        public override void OnEnterBlockReorderKeyPressed()
        {
            Manager.PushState(new BlockReorderState());
        }
    }
}