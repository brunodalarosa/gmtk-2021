using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace GMTK2021
{
    public class PlayerBlock : Block
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
        private Transform _feetPos = null;
        private Transform FeetPos => _feetPos;

        [SerializeField]
        private float _feetRadius = 0;
        private float FeetRadius => _feetRadius;

        [SerializeField]
        private LayerMask _groundLayer;
        private LayerMask GroundLayer => _groundLayer;

        private Dictionary<Vector2, Block> BlockGrid { get; set; }

        private Vector3 _velocity = Vector3.zero;
        private float Smoothness => 0.05f;
        public bool Grounded { get; private set; }
        public bool FacingRight { get; private set; }

        private bool PlayingWalkSound { get; set; }

        protected override void DidAwake()
        {
            BlockGrid = new Dictionary<Vector2, Block>();
            PositionFromPlayer = Vector2.zero;
            PlayerBlock = this;
            IsConnected = true;
            PlayingWalkSound = false;

            tag = "Player";
            FacingRight = true;
        }

        private void Update()
        {
            HandleMovement();


            if (Input.GetKeyDown(KeyCode.Space))
                ApplyJumpAction();

            if (Input.GetKeyDown(KeyCode.J))
                ApplyDashAction();

            if (Input.GetKeyDown(KeyCode.K))
                ApplyShootAction();
        }

        private void FixedUpdate()
        {
            HandleAnimation();
            Grounded = Physics2D.OverlapCircle(FeetPos.position, FeetRadius, GroundLayer);
        }

        private void HandleMovement()
        {
            var velocity = Rigidbody2D.velocity;
            Vector3 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * 10f, velocity.y);
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

        private void ApplyJumpAction()
        {
            var qtdJumps = PlayerBlock.BlockGrid.Values.Count(b => b is JumpBlock);
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

        private void ApplyDashAction()
        {
            foreach (var block in BlockGrid.Values.Where(block => block is DashBlock))
            {
                block.DoAction();
            }
        }

        private void ApplyShootAction()
        {
            foreach (var block in BlockGrid.Values.Where(block => block is LagolasBlock))
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

            switch (block)
            {
                case DashBlock dashBlock:
                    var qtdDashes = BlockGrid.Values.Count(b => b is DashBlock);
                    HeadsUpDisplay.Instance?.UpdateDash(true, qtdDashes);
                    break;

                case JumpBlock jumpBlock:
                    var qtdJumps = BlockGrid.Values.Count(b => b is JumpBlock);
                    HeadsUpDisplay.Instance?.UpdateJump(true, qtdJumps);
                    break;

                case LagolasBlock lagolasBlock:
                    var qtdLagolas = BlockGrid.Values.Count(b => b is JumpBlock);
                    HeadsUpDisplay.Instance?.UpdateLagolas(true, qtdLagolas);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(block));
            }
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
