using UnityEngine;

namespace GMTK2021
{
    public class PlayerBlock : Block
    {
        [SerializeField]
        private Rigidbody2D _rigidbody2D;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        private Vector3 _velocity = Vector3.zero;
        private float Smoothness => 0.05f;
        
        protected override void DidAwake()
        {
            PositionFromPlayer = Vector2.zero;
            PlayerBlock = this;
            IsConnected = true;
        }
        
        private void Update()
        {
            HandleMovement();
            
            if (Input.GetKey(KeyCode.Space)) 
                DoAction();
        }

        private void HandleMovement()
        {
            var velocity = Rigidbody2D.velocity;
            
            Vector3 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * 10f, velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, Smoothness);
        }

        protected override void Action()
        {
            // O player faz nada
        }
    }
}
