using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LaunchMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private AdventureModeManager _adventureModeManagerPrefab = null;
        private AdventureModeManager AdventureModeManagerPrefab => _adventureModeManagerPrefab;
        
        [SerializeField]
        private Button _playAdventureModeButton = null;
        private Button PlayAdventureModeButton => _playAdventureModeButton;
    
        [SerializeField]
        private Button _sandboxButton = null;
        private Button SandboxButton => _sandboxButton;

        private void Start()
        {
            PlayAdventureModeButton.onClick.AddListener(() => StartAdventureMode());
            SandboxButton.onClick.AddListener(EnterSandbox);
            AudioManager.Instance.PlayMainMenuMusic();
        }

        public void StartAdventureMode(int startingLevelIndex = 0)
        {
            Instantiate(AdventureModeManagerPrefab);
            AdventureModeManager.Instance.SetStartingLevel(startingLevelIndex);
            AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.PlayFirstLevel);
            SceneManager.LoadScene("TransitionScene");
        }
    
        private void EnterSandbox()
        {
            SceneManager.LoadScene("Sandbox");
        }
    }
}
