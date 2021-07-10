using System.Collections;
using UnityEngine;

namespace Level
{
    public class Spawner : MonoBehaviour
    {
        private const int SpawnInterval = 1;
        private const int DestroyInterval = 4;

        [SerializeField]
        private Block.BaseBlock _spawnee = null;
        private Block.BaseBlock Spawnee => _spawnee;

        [SerializeField]
        private GameObject _spawnPoint = null;
        private GameObject SpawnPoint => _spawnPoint;
    
        private bool IsRunning { get; set; }

        void Start()
        {
            IsRunning = true;
            StartCoroutine(SpawnNewBlock());
        }
    
        private IEnumerator SpawnNewBlock()
        {
            while (IsRunning)
            {
                yield return new WaitForSeconds(SpawnInterval);

                var rigid = new GameObject("spawnee");
            
                rigid.AddComponent<Rigidbody2D>();

                Instantiate(Spawnee, SpawnPoint.transform.position, Quaternion.identity, rigid.transform);
            
                StartCoroutine(DestroyAfterInterval(DestroyInterval, rigid.gameObject));
            }
        }

        private static IEnumerator DestroyAfterInterval(float seconds, GameObject obj)
        {
            yield return new WaitForSeconds(seconds);
        
            Destroy(obj);
        }

        private void OnDestroy()
        {
            IsRunning = false;
        }
    }
}
