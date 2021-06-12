using System;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace GMTK2021
{
    public class PlayerBlock : Block
    {
        private const int BlockAddMode = 1; // 0 para clockwise, 1 para direção do bloco que encostou

        [SerializeField]
        private Rigidbody2D _rigidbody2D;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
    
        [SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;

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
        
        private void FixedUpdate()
        {
            HandleMovement();
            HandleAnimation();
            
            if (Input.GetKey(KeyCode.Space)) 
                ApplyAction();
        }

        private void HandleMovement()
        {
            var velocity = Rigidbody2D.velocity;
            
            Vector3 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * 10f, velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, Smoothness);
        }

        private void HandleAnimation()
        {
            Animator.SetFloat("MoveX", Rigidbody2D.velocity.x);
            if (Rigidbody2D.velocity.x > 0.1)
                SpriteRenderer.flipX = false;
            else if (Rigidbody2D.velocity.x < -0.1)
                SpriteRenderer.flipX = true;


        }

        private void ApplyAction()
        {
            foreach (var block in BlockGrid.Values)
            {
                block.DoAction();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var go = other.gameObject;
            
            if (!go.CompareTag("Block"))
                return;

            var block = go.GetComponent<Block>();

            if (block.IsConnected)
                return;
            
            Debug.Log($"Trigger: from:{gameObject.name}, to:{go.name}");

            var realCollidedBlock = GetNearestBlockFromCollision(go);

            if (BlockAddMode == 0)
                realCollidedBlock.AddBlockClockwise(block);
            else
                realCollidedBlock.AddBlockFromCollision(block, go);
        }

        //Encontra o bloco onde aconteceu de fato a colisão calculando pela distancia do bloco colidido e os blocos já associados ao Player
        private Block GetNearestBlockFromCollision(GameObject collidedBlock)
        {
            Block nearest = this;
            float minDistance = (collidedBlock.transform.position - transform.position).magnitude;

            foreach (var block in BlockGrid.Values)
            {
                var distanceVector = collidedBlock.transform.position - block.gameObject.transform.position;

                if (distanceVector.magnitude < minDistance)
                {
                    nearest = block;
                    minDistance = distanceVector.magnitude;
                }
            }

            return nearest;
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
