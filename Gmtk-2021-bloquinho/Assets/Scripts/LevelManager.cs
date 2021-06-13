using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Object[] _scenes;
    private UnityEngine.Object[] Scenes => _scenes;

    private int CurrentIndex { get; set; }

    public static LevelManager Instance { get; private set; }

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
        if (CurrentIndex >= Scenes.Length - 1)
            throw new InvalidOperationException("Não tem mais fase mas mesmo assim algum otário tá tentando o impossível");

        CurrentIndex++;
        SceneManager.LoadScene(Scenes[CurrentIndex].name);
    }

    public void ResetCurrentLevel()
    {
        //TODO: animação de transição
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
