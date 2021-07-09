using System.Collections;
using GMTK2021;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const int SpawnInterval = 1;
    private const int DestroyInterval = 4;

    [SerializeField]
    private Block _spawnee = null;
    public Block Spawnee => _spawnee;

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

            var rigid = new GameObject("rigido corpo");
            
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
