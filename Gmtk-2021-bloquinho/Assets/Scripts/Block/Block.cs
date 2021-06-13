using System;
using Unity.Mathematics;
using UnityEngine;

namespace GMTK2021
{
    /*
     * Se os blocos precisam alterar algo que impacte todos os blocos, é necessário que o componente esteja no player
     * e então acessado a partir do bloco que vai alterar ele. O jump é um exemplo, porque ele altera a posição do player
     * e todos os outros blocos seguem a posição do player (por serem direta ou indiretamente filhos dele).
     * Todos os blocos tem uma referência para bloco player.
     */
    public abstract class Block : MonoBehaviour
    {

        [SerializeField]
        private Collider2D _collider;
        private Collider2D Collider => _collider;

        [SerializeField]
        protected Animator _animator;
        public Animator Animator => _animator;
        
        protected Vector2 PositionFromPlayer { get; set; }
        protected PlayerBlock PlayerBlock { get; set; }
        public bool IsConnected { get; set; }

        private Transform Transform { get; set; }
        
        private void Awake()
        {
            Transform = transform;
            IsConnected = false;
            tag = "Block";
            
            DidAwake();
        }
        
        public void DoAction()
        {
            Action();
        }

        private void ConnectBlock(Block newBlock, Direction direction)
        {
            AddNeighbour(newBlock, direction);
            newBlock.ConnectBlockFrom(this, direction, PlayerBlock);
        }
        
        private void ConnectBlockFrom(Block parentBlock, Direction direction, PlayerBlock playerBlock)
        {
            PlayerBlock = playerBlock;
            
            Transform.SetParent(parentBlock.Transform);
            Transform.localPosition = direction.AsVector3();
            Transform.rotation = PlayerBlock.Transform.rotation;

            switch (direction)
            {
                case Direction.Left:
                    Transform.localPosition = new Vector3(Transform.localPosition.x + 0.2f, Transform.localPosition.y);
                    break;
                case Direction.Up:
                    Transform.localPosition = new Vector3(Transform.localPosition.x, Transform.localPosition.y - 0.2f);
                    break;
                case Direction.Right:
                    Transform.localPosition = new Vector3(Transform.localPosition.x - 0.2f, Transform.localPosition.y);
                    break;
                case Direction.Down:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            PositionFromPlayer = parentBlock.PositionFromPlayer + direction.AsVector2();
            gameObject.name = $"{direction.ToString()}, x:{PositionFromPlayer.x}, y:{PositionFromPlayer.y}";

            Collider.isTrigger = false;
            Collider.usedByComposite = true;
            
            IsConnected = true;
        }

        public void AddBlockClockwise(Block block)
        {
            var direction = Direction.Down;
            
            // Ordem de adição dos blocos. Para alterar, só alterar a ordem da validação. Pode ser até aleatório.
            
            if (!HasNeighbour(Direction.Up))
                direction = Direction.Up;
            else if (!HasNeighbour(Direction.Right))
                direction = Direction.Right;
            else if (!HasNeighbour(Direction.Left))
                direction = Direction.Left;
            
            if (direction != Direction.Down)
                ConnectBlock(block, direction);
            else // Todas as direções estão ocupadas, vai para a próxima. Está priorizando left mas pode ser alterado
                PlayerBlock.GetBlock(PositionFromPlayer + Direction.Left.AsVector2()).AddBlockClockwise(block);
        }

        public void AddBlockFromCollision(Block block, GameObject go)
        {
            var direction = go.transform.position - transform.position;
            // Debug.Log("[OnTriggerEnter2D] direction -> " + direction);

            if (math.abs(direction.x) > math.abs(direction.y)) //É uma conexão horizontal
                ConnectBlock(block, direction.x < 0 ? Direction.Left : Direction.Right);
            else //É uma conexão vertical
                ConnectBlock(block, Direction.Up);
            
            // ConnectBlock(block, direction.y < 0 ? Direction.Down : Direction.Up); substituir aqui se for voltar a ter conexão por baixo
        }

        private void AddNeighbour(Block block, Direction direction)
        {
            PlayerBlock.AddBlock(block, PositionFromPlayer + direction.AsVector2());
        }

        private Block GetNeighbour(Direction direction)
        {
            return PlayerBlock.GetBlock(PositionFromPlayer + direction.AsVector2());
        }
        
        private Block GetNeighbourSafe(Direction direction)
        {
            return PlayerBlock.GetBlockSafe(PositionFromPlayer + direction.AsVector2());
        }

        private bool HasNeighbour(Direction direction)
        {
            return GetNeighbourSafe(direction) != null;
        }

        protected virtual void DidAwake() { }
        protected abstract void Action();
    }

    public enum Direction
    {
        Left = 0,
        Up,
        Right,
        Down
    }

    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid direction")
            };
        }

        public static Direction Clockwise(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Up,
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid direction")
            };
        }

        public static Vector2 AsVector2(this Direction direction)
        {
            return AsVector3(direction);
        }
        
        public static Vector3 AsVector3(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Vector3.left,
                Direction.Up => Vector3.up,
                Direction.Right => Vector3.right,
                Direction.Down => Vector3.down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid direction")
            };
        }
    }
}
