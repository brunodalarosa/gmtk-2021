using System.Collections;
using System.Collections.Generic;
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
    }

    public void GoToFirstLevel()
    {
        LevelManager.Instance.GoToNextLevel();
    }
}
