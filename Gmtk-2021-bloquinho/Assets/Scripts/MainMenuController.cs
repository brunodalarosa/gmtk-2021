using GMTK2021;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button _playButton = null;
    private Button PlayButton => _playButton;


    private void Awake()
    {
        PlayButton.onClick.AddListener(GoToFirstLevel);
        AudioManager.Instance.PlayMainMenuMusic();
    }

    public void GoToFirstLevel()
    {
        AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.PlayFirstLevel);
        LevelManager.Instance.GoToNextLevel();
    }
}
