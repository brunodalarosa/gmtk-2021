using System.Collections.Generic;
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
        
        private Dictionary<Vector2, Block> BlockGrid { get; set; }
        
        private Vector3 _velocity = Vector3.zero;
        private float Smoothness => 0.05f;
        
        protected override void DidAwake()
        {
            BlockGrid = new Dictionary<Vector2, Block>();
            PositionFromPlayer = Vector2.zero;
            PlayerBlock = this;
            IsConnected = true;
        }
        
        private void Update()
        {
            HandleMovement();
            
            if (Input.GetKey(KeyCode.Space)) 
                ApplyAction();
        }

        private void HandleMovement()
        {
            var velocity = Rigidbody2D.velocity;
            
            Vector3 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * 10f, velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, Smoothness);
        }

        private void ApplyAction()
        {
            foreach (var block in BlockGrid.Values)
            {
                block.DoAction();
            }
        }

        public void AddBlock(Block block, Vector2 position)
        {
            BlockGrid.Add(position, block);
        }

        public Block GetBlock(Vector2 position)
        {
            return BlockGrid[position];
        }

        public Block GetBlockSafe(Vector2 position)
        {
            return BlockGrid.ContainsKey(position) ? GetBlock(position) : null;
        }

        public Block GetNeighbourBlock(Vector2 position, Direction direction)
        {
            return GetBlock(position + direction.AsVector2());
        }

        protected override void Action()
        {
            // O player faz nada
        }
    }
}
