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
    [RequireComponent(typeof(InputHandler))]
    public class LevelController : MonoBehaviour, ILevelController, IGameObserver<IEvent>
    {
        public static LevelController Instance { get; private set; }
        
        [SerializeField]
        private GameplayCamera _gameplayCamera = null;
        private GameplayCamera GameplayCamera => _gameplayCamera;
        
        [SerializeField]
        private Canvas _gamePausedCanvas = null;
        private Canvas GamePausedCanvas => _gamePausedCanvas;
        
        public IState CurrentGameState => StateMachine.CurrentState;
        public LeaderBlock FocusedLeaderBlock => _leaderBlocks[_focusedLeaderBlockIndex];
        private int _focusedLeaderBlockIndex;

        private readonly List<LeaderBlock> _leaderBlocks = new List<LeaderBlock>();

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

            InitializeStateMachine();
        }
        
        private void InitializeStateMachine()
        {
            StateMachine = new StateMachineManager(this);
            StateMachine.PushState(new DefaultGameplayState());
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

        #region ILevelController Interface Methods

        public void StopLeaderBlocksWalk()
        {
            foreach (var leaderBlock in _leaderBlocks) leaderBlock.StopWalking();
        }
        
        public void OnPauseGameKeyPressed()
        {
            StateMachine.OnPauseGameKeyPressed();
        }
        
        public void ResetCurrentLevel()
        {
            //TODO: animação de transição
            HeadsUpDisplay.Instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        /// <summary>
        /// Cicla pela lista de blocos líder: Volta pro início se estiver focando no último.
        /// </summary>
        public void ChangeFocusedLeader()
        {
            _focusedLeaderBlockIndex++;
            _focusedLeaderBlockIndex %= _leaderBlocks.Count;
            
            FocusCamera();
        }

        #endregion

        private void FocusCamera()
        {
            GameplayCamera.SetCamera(FocusedLeaderBlock.transform);
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