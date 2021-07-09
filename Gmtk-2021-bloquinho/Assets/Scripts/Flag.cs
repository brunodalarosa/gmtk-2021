using GMTK2021;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var go = other.gameObject;

        if (!go.CompareTag("Player"))
            return;

        AudioManager.Instance?.PlaySfx(AudioManager.SoundEffects.LevelComplete);
        LevelManager.Instance.GoToNextLevel();
    }
}
