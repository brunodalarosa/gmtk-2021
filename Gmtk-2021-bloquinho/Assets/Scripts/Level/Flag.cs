using Block;
using UnityEngine;

namespace Level
{
    public class Flag : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<LeaderBlock>().ReachedEndOfLevel();
        }
    }
}
