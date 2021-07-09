using Manager;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BaseEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            AdventureModeManager.Instance.ResetCurrentLevel();
    }

    public void Die()
    {
        AudioManager.Instance?.PlaySfx(AudioManager.SoundEffects.EnemyPoof);
        Destroy(gameObject);
    }
}
