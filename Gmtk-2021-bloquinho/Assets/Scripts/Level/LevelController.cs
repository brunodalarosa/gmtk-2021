using Block;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Level
{
    [RequireComponent(typeof(BlockInputHandler))]
    public class LevelController : MonoBehaviour
    {
        [FormerlySerializedAs("_focusedBlock")]
        [SerializeField]
        private LeaderBlock _startingLeaderBlock = null;
        private LeaderBlock StartingLeaderBlock => _startingLeaderBlock;
        
        [SerializeField]
        private LeaderBlock _otherLeaderBlock = null;
        private LeaderBlock OtherLeaderBlock => _otherLeaderBlock;

        private LeaderBlock FocusedLeaderBlock { get; set; }

        private BlockInputHandler BlockInputHandler { get; set; }

        private void Start()
        {
            BlockInputHandler = GetComponent<BlockInputHandler>();

            FocusedLeaderBlock = StartingLeaderBlock;
            
            GameplayCamera.Instance.SetCamera(FocusedLeaderBlock.transform);
        }

        private void Update()
        {
            //necessário para o bloco que anda parar de andar depois de receber um input de andar
            StartingLeaderBlock?.StopWalking();
            OtherLeaderBlock?.StopWalking();
            
            bool receivedLevelInput = ReadLevelInputs();
            if (receivedLevelInput) return;
            
            BlockInputHandler.ReadInputs();
            
            if (BlockInputHandler.Commands.Count > 0)
            {
                foreach (var command in BlockInputHandler.Commands) command.Execute(FocusedLeaderBlock);
                BlockInputHandler.Commands.Clear();
            }
        }

        private bool ReadLevelInputs()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetCurrentLevel();
                return true;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchFocusedLeaderBlock();
                GameplayCamera.Instance.SetCamera(FocusedLeaderBlock.transform);
                return true;
            }

            return false;
        }

        private void SwitchFocusedLeaderBlock()
        {
            if (OtherLeaderBlock == null) return;

            FocusedLeaderBlock = FocusedLeaderBlock == StartingLeaderBlock ? OtherLeaderBlock : StartingLeaderBlock;
        }
        
        public void ResetCurrentLevel()
        {
            //TODO: animação de transição
            HeadsUpDisplay.Instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}