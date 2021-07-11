using DG.Tweening;
using Enemy;
using UnityEngine;

namespace Block
{
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
            Invoke(nameof(Die), TimeToDestroy);
        }

        public void Init(bool facingRight)
        {
            Direction = facingRight ? 1 : -1;
        }

        private void FixedUpdate()
        {
            transform.DOMoveX(transform.position.x + (Speed * 0.01f * Direction), 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy")) other.gameObject.GetComponent<BaseEnemy>().Die();

            Die();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player")) return;
        
            Die();
        }

        private void Die()
        {
            _speed = 0f;
            
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.zero, 0.25f));
            seq.AppendCallback(() => Destroy(gameObject));
            seq.Play();
        }
    }
}
