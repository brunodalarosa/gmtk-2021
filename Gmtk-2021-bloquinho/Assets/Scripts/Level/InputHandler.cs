using StateMachine.State;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(BlocksInputHandler))]
    public class InputHandler : MonoBehaviour
    {
        private ILevelController LevelController { get; set; }
        
        private BlocksInputHandler BlocksInputHandler { get; set; }

        private void Start()
        {
            LevelController = GetComponent<ILevelController>();
            BlocksInputHandler = GetComponent<BlocksInputHandler>();
        }

        private void Update()
        {
            LevelController.StopLeaderBlocksWalk();
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelController.OnPauseGameKeyPressed();
                return;
            }
            
            if (!(LevelController.CurrentGameState is DefaultGameplayState))
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelController.ResetCurrentLevel();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                LevelController.ChangeFocusedLeader();
                return;
            }

            BlocksInputHandler.ReadInputs();

            if (BlocksInputHandler.Commands.Count > 0)
            {
                foreach (var command in BlocksInputHandler.Commands) command.Execute(LevelController.FocusedLeaderBlock);
                BlocksInputHandler.Commands.Clear();
            }
        }
    }
}