using System;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class AdventureModeManager : MonoBehaviour
    {
        public const int Finallevel = 9;

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

        public void GoToNextLevel()
        {
            if (CurrentIndex == Finallevel + 1)
            {
                HeadsUpDisplay.Instance.Hide();
                SceneManager.LoadScene("EndGame");
                return;
            }

            if (CurrentIndex >= Scenes.Length)
                throw new InvalidOperationException("Can't go further last level!");

            SceneManager.LoadScene($"Level{CurrentIndex}");
            HeadsUpDisplay.Instance.Reset();
            CurrentIndex++;
        }

        public void SetStartingLevel(int levelIndex)
        {
            if (levelIndex >= Finallevel + 1)
                throw new InvalidOperationException($"Can't Start from level {levelIndex}");

            CurrentIndex = levelIndex;
        }
    }
}