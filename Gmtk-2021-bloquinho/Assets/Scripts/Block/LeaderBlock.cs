using System;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using JetBrains.Annotations;
using Level;
using Manager;
using Observer;
using UnityEngine;

namespace Block
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class LeaderBlock : BaseBlock
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private const float FeetRadius = 0.2f;

        [Header("Feet")]
        [SerializeField]
        private Transform _feetPos;
        private Transform FeetPos => _feetPos;

        [SerializeField]
        private LayerMask _groundLayer;
        private LayerMask GroundLayer => _groundLayer;

        [Header("Inner Block")]
        [SerializeField]
        private BaseBlock _innerBlock = null;
        private BaseBlock InnerBlock => _innerBlock;
        
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody2D { get; private set; }
        public bool FacingRight { get; private set; }
        public bool Grounded { get; private set; }


        private readonly BlockSubject _blockSubject = new BlockSubject();
        private Dictionary<Vector2, BaseBlock> BlockGrid { get; set; }

        private void Start()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            BlockGrid = new Dictionary<Vector2, BaseBlock>();
            LeaderBlock = this;

            IsConnected = true;
            FacingRight = true;

            tag = "Player";
            
            LevelController.Instance.RegisterAsLeaderBlock(this);
        }
        
        private void FixedUpdate()
        {
            HandleAnimation();
            Grounded = Physics2D.OverlapCircle(FeetPos.position, FeetRadius, GroundLayer);
        }

        public void CollidedWithEnemy(BaseEnemy enemy)
        {
            //todo expandir essa lógica, considerar qual o tipo de imigo que colidiu etc
            _blockSubject.SendDeathEvent(this);
        }
        
        public void HandleMovement(float horizontalInput)
        {
            GetWalkingBlock()?.HandleMovement(horizontalInput);
        }

        public void StopWalking()
        {
            GetWalkingBlock()?.HandleMovement(0);
        }

        [CanBeNull]
        private WalkingBlock GetWalkingBlock()
        {
            if (InnerBlock is WalkingBlock) return InnerBlock as WalkingBlock;
            return BlockGrid.Values.FirstOrDefault(block => block is WalkingBlock) as WalkingBlock;
        }

        private void HandleAnimation()
        {
            Animator.SetFloat(MoveX, Rigidbody2D.velocity.x);

            foreach (var block in BlockGrid.Values)
            {
                block.Animator.SetFloat(MoveX, Rigidbody2D.velocity.x);
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
            var qtdJumps = LeaderBlock.BlockGrid.Values.Count(b => b is JumpBlock);
            if (InnerBlock is JumpBlock) qtdJumps++;
            
            if (qtdJumps == 0)
                return;
            
            var force = 0f;
            var initialForce = JumpBlock.JumpForce;
            
            for (int i = 0; i < qtdJumps; i++)
            {
                force += initialForce;
                initialForce /= 2;
            }

            if (LeaderBlock.Grounded)
            {
                LeaderBlock.Rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.Jump);
            }
        }

        public void Dash()
        {
            if (InnerBlock is DashBlock dashBlock) dashBlock.DoAction();

            foreach (var block in BlockGrid.Values.Where(block => block is DashBlock))
            {
                block.DoAction();
            }
        }

        public void Shoot()
        {
            if (InnerBlock is ShootingBlock shootingBlock) shootingBlock.DoAction();
            
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
            realCollidedBlock.AddBlockFromCollision(block, go);
        }
        
        //Encontra o bloco onde aconteceu de fato a colisão calculando pela distancia do bloco colidido e os blocos já
        //associados ao Lider
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

        public void ReachedEndOfLevel()
        {
            _blockSubject.SendReachedEndOfLevelEvent(this);
        }

        public void AddObserver(IGameObserver<IEvent> observer)
        {
            _blockSubject.AddObserver(observer);
        }
        
        private void OnDestroy()
        {
            _blockSubject.ClearObservers();
        }
        
        protected override void Action()
        {
        }
    }
}