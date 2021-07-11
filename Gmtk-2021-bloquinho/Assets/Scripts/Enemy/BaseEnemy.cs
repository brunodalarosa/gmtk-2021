using System.Collections;
using Block;
using Manager;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class BaseEnemy : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<LeaderBlock>().CollidedWithEnemy(this);
        }

        public void Die()
        {
            AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.EnemyPoof); 
            
            StartCoroutine(DestroyAfterEndOfFrame());

            gameObject.SetActive(false);
        }

        private IEnumerator DestroyAfterEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
    }
}
