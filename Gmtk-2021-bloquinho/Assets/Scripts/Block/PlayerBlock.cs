using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Level;
using Manager;
using UnityEngine;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace Block
{
    public class PlayerBlock : BaseBlock
    {
        private const int BlockAddMode = 1; // 0 para clockwise, 1 para direção do bloco que encostou
        private const float WalkSfxSecondsInterval = 0.4f;

        [SerializeField]
        private Rigidbody2D _rigidbody2D;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [Header("Feet")]
        [SerializeField]
        private Transform _feetPos;
        private Transform FeetPos => _feetPos;

        [SerializeField]
        private float _feetRadius;
        private float FeetRadius => _feetRadius;

        [SerializeField]
        private LayerMask _groundLayer;
        private LayerMask GroundLayer => _groundLayer;

        private Dictionary<Vector2, BaseBlock> BlockGrid { get; set; }

        private Vector3 _velocity = Vector3.zero;
        private float Smoothness => 0.05f;
        public bool Grounded { get; private set; }
        public bool FacingRight { get; private set; }

        private bool PlayingWalkSound { get; set; }

        protected override void DidAwake()
        {
            BlockGrid = new Dictionary<Vector2, BaseBlock>();
            PositionFromPlayer = Vector2.zero;
            PlayerBlock = this;
            IsConnected = true;
            PlayingWalkSound = false;

            tag = "Player";
            FacingRight = true;
        }

        private void FixedUpdate()
        {
            HandleAnimation();
            Grounded = Physics2D.OverlapCircle(FeetPos.position, FeetRadius, GroundLayer);
        }

        public void HandleMovement(float horizontalInput)
        {
            var velocity = Rigidbody2D.velocity;
            Vector3 targetVelocity = new Vector2(horizontalInput, velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, Smoothness);

            if (Rigidbody2D.velocity.x != 0 && !PlayingWalkSound)
            {
                StartCoroutine(PlayWalkSfx());
            }
        }

        private IEnumerator PlayWalkSfx()
        {
            PlayingWalkSound = true;
            yield return new WaitForSeconds(WalkSfxSecondsInterval);
            AudioManager.Instance?.PlaySfx(AudioManager.SoundEffects.WalkStep);
            PlayingWalkSound = false;
        }

        private void HandleAnimation()
        {
            Animator.SetFloat("MoveX", Rigidbody2D.velocity.x);

            foreach (var block in BlockGrid.Values)
            {
                block.Animator.SetFloat("MoveX", Rigidbody2D.velocity.x);
            }

            if (Rigidbody2D.velocity.x > 0.3)
            {
                SpriteRenderer.flipX = false;
                FacingRight = true;
            }
            else if (Rigidbody2D.velocity.x < -0.3)
            {
                SpriteRenderer.flipX = true;
                FacingRight = false;
            }
        }

        public void Jump()
        {
            var qtdJumps = PlayerBlock.BlockGrid.Values.Count(b => b is JumpBlock);
            if (qtdJumps == 0)
                return;
            
            var force = 0f;
            var initialForce = (PlayerBlock.BlockGrid.Values.FirstOrDefault(b => b is JumpBlock) as JumpBlock).JumpForce;
            
            for (int i = 0; i < qtdJumps; i++)
            {
                force += initialForce;
                initialForce /= 2;
            }

            if (PlayerBlock.Grounded)
            {
                PlayerBlock.Rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                AudioManager.Instance?.PlaySfx(AudioManager.SoundEffects.Jump);
            }
        }

        public void Dash()
        {
            foreach (var block in BlockGrid.Values.Where(block => block is DashBlock))
            {
                block.DoAction();
            }
        }

        public void Shoot()
        {
            foreach (var block in BlockGrid.Values.Where(block => block is ShootingBlock))
            {
                block.DoAction();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var go = other.gameObject;

            if (!go.CompareTag("Block"))
                return;

            var block = go.GetComponent<BaseBlock>();

            if (block.IsConnected)
                return;

            Debug.Log($"Trigger: from:{gameObject.name}, to:{go.name}");

            var realCollidedBlock = GetNearestBlockFromCollision(go);

            if (BlockAddMode == 0)
                realCollidedBlock.AddBlockClockwise(block);
            realCollidedBlock.AddBlockFromCollision(block, go);
        }

        //Encontra o bloco onde aconteceu de fato a colisão calculando pela distancia do bloco colidido e os blocos já associados ao Player
        private BaseBlock GetNearestBlockFromCollision(GameObject collidedBlock)
        {
            BaseBlock nearest = this;
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

        public void AddBlock(BaseBlock baseBlock, Vector2 position)
        {
            BlockGrid.Add(position, baseBlock);

            int quantity = baseBlock switch
            {
                DashBlock _ => BlockGrid.Values.Count(b => b is DashBlock),
                JumpBlock _ => BlockGrid.Values.Count(b => b is JumpBlock),
                ShootingBlock _ => BlockGrid.Values.Count(b => b is ShootingBlock),
                _ => throw new ArgumentOutOfRangeException(nameof(baseBlock))
            };

            HeadsUpDisplay.Instance.UpdateBlockStatus(baseBlock, quantity);
        }

        public BaseBlock GetBlock(Vector2 position)
        {
            return BlockGrid[position];
        }

        public BaseBlock GetBlockSafe(Vector2 position)
        {
            return BlockGrid.ContainsKey(position) ? GetBlock(position) : null;
        }

        public BaseBlock GetNeighbourBlock(Vector2 position, Direction direction)
        {
            return GetBlock(position + direction.AsVector2());
        }

        protected override void Action()
        {
            // O player faz nada
        }
    }
}
