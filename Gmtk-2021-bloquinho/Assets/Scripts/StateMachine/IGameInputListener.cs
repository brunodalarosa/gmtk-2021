namespace StateMachine
{
    public interface IGameInputListener
    {
        void OnPauseGameKeyPressed();
        void OnEnterBlockReorderKeyPressed();
    }
}