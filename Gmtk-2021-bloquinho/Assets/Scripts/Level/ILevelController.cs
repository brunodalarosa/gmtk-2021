using Block;
using StateMachine.State;

namespace Level
{
    public interface ILevelController
    {
        IState CurrentGameState { get; }
        LeaderBlock FocusedLeaderBlock { get; }

        void StopLeaderBlocksWalk();
        void OnPauseGameKeyPressed();
        void ResetCurrentLevel();
        void ChangeFocusedLeader();
    }
}