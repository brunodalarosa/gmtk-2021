using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var go = other.gameObject;
            
        if (!go.CompareTag("Player"))
            return;

        LevelManager.Instance.GoToNextLevel();
    }
}
