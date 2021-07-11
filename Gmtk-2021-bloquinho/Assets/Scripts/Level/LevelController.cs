using System;
using Block;
using Manager;
using Observer;
using StateMachine;
using StateMachine.State;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Level
{
    [RequireComponent(typeof(BlockInputHandler))]
    public class LevelController : MonoBehaviour, IGameObserver<IEvent>
    {
        [SerializeField]
        private Canvas _gamePausedCanvas = null;
        private Canvas GamePausedCanvas => _gamePausedCanvas;
        
        
        [FormerlySerializedAs("_focusedBlock")]
        [SerializeField]
        private LeaderBlock _startingLeaderBlock = null;
        private LeaderBlock StartingLeaderBlock => _startingLeaderBlock;

        [SerializeField]
        private LeaderBlock _otherLeaderBlock = null;
        private LeaderBlock OtherLeaderBlock => _otherLeaderBlock;

        private LeaderBlock FocusedLeaderBlock { get; set; }

        private BlockInputHandler BlockInputHandler { get; set; }
        
        private IStateMachineManager StateMachine { get; set; }
        // private IGameInputListener GameInputListener { get; set; }

        private void Start()
        {
            GamePausedCanvas.gameObject.SetActive(false);
            BlockInputHandler = GetComponent<BlockInputHandler>();

            InitializeStateMachine();

            FocusedLeaderBlock = StartingLeaderBlock;
            FocusedLeaderBlock.AddObserver(this);

            GameplayCamera.Instance.SetCamera(FocusedLeaderBlock.transform);
        }

        private void InitializeStateMachine()
        {
            StateMachine = new StateMachineManager(this);
            StateMachine.PushState(new DefaultGameplayState());
        }

        private void Update()
        {
            //necessário para o bloco que anda parar de andar depois de receber um input de andar
            StartingLeaderBlock?.StopWalking();
            OtherLeaderBlock?.StopWalking();
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StateMachine.OnPauseGameKeyPressed();
                return;
            }
            
            if (!(StateMachine.CurrentState is DefaultGameplayState))
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetCurrentLevel();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchFocusedLeaderBlock();
                GameplayCamera.Instance.SetCamera(FocusedLeaderBlock.transform);
                return;
            }

            BlockInputHandler.ReadInputs();

            if (BlockInputHandler.Commands.Count > 0)
            {
                foreach (var command in BlockInputHandler.Commands) command.Execute(FocusedLeaderBlock);
                BlockInputHandler.Commands.Clear();
            }
        }
        
        private void SwitchFocusedLeaderBlock()
        {
            if (OtherLeaderBlock == null) return;

            FocusedLeaderBlock = FocusedLeaderBlock == StartingLeaderBlock ? OtherLeaderBlock : StartingLeaderBlock;
        }

        private void ResetCurrentLevel()
        {
            //TODO: animação de transição
            HeadsUpDisplay.Instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void FinishLevel()
        {
            AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.LevelComplete);
            AdventureModeManager.Instance.GoToNextLevel();
        }

        public void ReceiveEvent(object subject, IEvent data)
        {
            switch (data)
            {
                case PlayerDiedEvent _:
                    ResetCurrentLevel();
                    break;
                case PlayerReachedEndOfLevelEvent _:
                    FinishLevel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data));
            }
        }

        public void ShowPauseOverlay()
        {
            HeadsUpDisplay.Instance.Hide();
            GamePausedCanvas.gameObject.SetActive(true);
        }

        public void HidePauseOverlay()
        {
            HeadsUpDisplay.Instance.Show();
            GamePausedCanvas.gameObject.SetActive(false);
        }
    }
}