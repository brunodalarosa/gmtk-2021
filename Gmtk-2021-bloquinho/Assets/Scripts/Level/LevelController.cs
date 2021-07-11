using System;
using System.Collections.Generic;
using Block;
using Manager;
using Observer;
using StateMachine;
using StateMachine.State;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    [RequireComponent(typeof(BlocksInputHandler))]
    public class LevelController : MonoBehaviour, IGameObserver<IEvent>
    {
        public static LevelController Instance { get; private set; }
        public IState CurrentGameState => StateMachine.CurrentState;

        [SerializeField]
        private GameplayCamera _gameplayCamera = null;
        private GameplayCamera GameplayCamera => _gameplayCamera;
        
        [SerializeField]
        private Canvas _gamePausedCanvas = null;
        private Canvas GamePausedCanvas => _gamePausedCanvas;

        private readonly List<LeaderBlock> _leaderBlocks = new List<LeaderBlock>();
        private int _focusedLeaderBlockIndex;
        private LeaderBlock FocusedLeaderBlock => _leaderBlocks[_focusedLeaderBlockIndex];

        private BlocksInputHandler BlocksInputHandler { get; set; }
        
        private IStateMachineManager StateMachine { get; set; }
        

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        
        private void Start()
        {
            GamePausedCanvas.gameObject.SetActive(false);
            BlocksInputHandler = GetComponent<BlocksInputHandler>();

            InitializeStateMachine();
        }

        public void RegisterAsLeaderBlock(LeaderBlock leaderBlock)
        {
            if (_leaderBlocks.Contains(leaderBlock))
                throw new InvalidOperationException(
                    "Trying to register a leaderblock that has already been registered");
            
            leaderBlock.AddObserver(this);
            _leaderBlocks.Add(leaderBlock);

            _focusedLeaderBlockIndex = _leaderBlocks.Count - 1;

            FocusCamera();
        }

        private void InitializeStateMachine()
        {
            StateMachine = new StateMachineManager(this);
            StateMachine.PushState(new DefaultGameplayState());
        }

        private void Update()
        {
            StopLeaderBlocksWalk();
            
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
                ChangeFocusedLeaderBlock();
                FocusCamera();
                return;
            }

            BlocksInputHandler.ReadInputs();

            if (BlocksInputHandler.Commands.Count > 0)
            {
                foreach (var command in BlocksInputHandler.Commands) command.Execute(FocusedLeaderBlock);
                BlocksInputHandler.Commands.Clear();
            }
        }
        
        /// <summary>
        /// Cicla pela lista de blocos líder: Volta pro início se estiver focando no último.
        /// </summary>
        private void ChangeFocusedLeaderBlock()
        {
            _focusedLeaderBlockIndex++;
            _focusedLeaderBlockIndex %= _leaderBlocks.Count;
        }
        
        private void FocusCamera()
        {
            GameplayCamera.SetCamera(FocusedLeaderBlock.transform);
        }
        
        private void StopLeaderBlocksWalk()
        {
            foreach (var leaderBlock in _leaderBlocks) leaderBlock.StopWalking();
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

        public void PauseGame()
        {
            Time.timeScale = 0;
            HeadsUpDisplay.Instance.Hide();
            GamePausedCanvas.gameObject.SetActive(true);
        }

        public void UnpauseGame()
        {
            HeadsUpDisplay.Instance.Show();
            GamePausedCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        #region EventsHandling

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

        #endregion
    }
}