using DG.Tweening;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;
    private float Speed => _speed;

    [SerializeField]
    private float _timeToDestroy = 2.5f;
    private float TimeToDestroy => _timeToDestroy;

    private float Direction { get; set; }

    private void Start()
    {
        Invoke("Die", TimeToDestroy);
    }

    public void Init(bool facingRight)
    {
        Direction = facingRight ? 1 : -1;
    }

    private void FixedUpdate()
    {
        transform.DOMoveX(transform.position.x + (Speed * 0.01f * Direction), 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
            col.gameObject.GetComponent<BaseEnemy>().Die();
        Die();
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
