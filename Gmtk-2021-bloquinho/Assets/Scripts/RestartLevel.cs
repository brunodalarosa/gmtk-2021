using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class RestartLevel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            AdventureModeManager.Instance.ResetCurrentLevel();
    }
}
