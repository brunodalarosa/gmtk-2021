using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BaseEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            LevelManager.Instance.ResetCurrentLevel();
    }

    public void Die()
    {

    }
}
