using System.Collections;
using System.Collections.Generic;
using GMTK2021;
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
        AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.EnemyPoof);
        Destroy(gameObject);
    }
}
