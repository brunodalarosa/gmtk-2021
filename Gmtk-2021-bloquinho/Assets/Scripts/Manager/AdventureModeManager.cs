using System;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class AdventureModeManager : MonoBehaviour
    {
        private const int Finallevel = 9;

        [SerializeField]
        private UnityEngine.Object[] _scenes;
        private UnityEngine.Object[] Scenes => _scenes;

        private int CurrentIndex { get; set; }

        public static AdventureModeManager Instance { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            CurrentIndex = 0;
        }

        public void GoToNextLevel()
        {
            if (CurrentIndex == Finallevel + 1)
            {
                SceneManager.LoadScene("EndGame");
                return;
            }

            if (CurrentIndex >= Scenes.Length)
                throw new InvalidOperationException("Can't go further last level!");

            SceneManager.LoadScene($"Level{CurrentIndex}");
            HeadsUpDisplay.Instance.Reset();
            CurrentIndex++;

            AudioManager.Instance.PlayLevelMusic();
        }

        public void ResetCurrentLevel()
        {
            //TODO: animação de transição
            HeadsUpDisplay.Instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
