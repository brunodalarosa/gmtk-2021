using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button _playAdventureModeButton = null;
    private Button PlayAdventureModeButton => _playAdventureModeButton;


    private void Start()
    {
        PlayAdventureModeButton.onClick.AddListener(GoToTransitionScene);
        AudioManager.Instance.PlayMainMenuMusic();
    }

    private void GoToTransitionScene()
    {
        AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.PlayFirstLevel);
        SceneManager.LoadScene("TransitionScene");
    }
}
