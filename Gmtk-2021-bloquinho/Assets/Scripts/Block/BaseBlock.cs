using System;
using Manager;
using Unity.Mathematics;
using UnityEngine;

namespace Block
{
    /*
     * Se os blocos precisam alterar algo que impacte todos os blocos, é necessário que o componente esteja no player
     * e então acessado a partir do bloco que vai alterar ele. O jump é um exemplo, porque ele altera a posição do player
     * e todos os outros blocos seguem a posição do player (por serem direta ou indiretamente filhos dele).
     * Todos os blocos tem uma referência para bloco player.
     */
    public abstract class BaseBlock : MonoBehaviour
    {
        [SerializeField]
        private Collider2D _collider;
        private Collider2D Collider => _collider;

        [SerializeField]
        protected Animator _animator;
        public Animator Animator => _animator;

        protected Vector2 PositionFromPlayer { get; set; }
        protected LeaderBlock LeaderBlock { get; set; }
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

        private void ConnectBlock(BaseBlock newBaseBlock, Direction direction)
        {
            AddNeighbour(newBaseBlock, direction);
            newBaseBlock.ConnectBlockFrom(this, direction, LeaderBlock);
            AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.BlockConnect);
        }

        private void ConnectBlockFrom(BaseBlock parentBaseBlock, Direction direction, LeaderBlock playerBlock)
        {
            LeaderBlock = playerBlock;

            Transform.SetParent(parentBaseBlock.Transform);
            Transform.localPosition = direction.AsVector3();
            Transform.rotation = LeaderBlock.Transform.rotation;

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

            PositionFromPlayer = parentBaseBlock.PositionFromPlayer + direction.AsVector2();
            gameObject.name = $"{direction.ToString()}, x:{PositionFromPlayer.x}, y:{PositionFromPlayer.y}";

            Collider.isTrigger = false;
            Collider.usedByComposite = true;

            IsConnected = true;
        }

        public void AddBlockClockwise(BaseBlock baseBlock)
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
                ConnectBlock(baseBlock, direction);
            else // Todas as direções estão ocupadas, vai para a próxima. Está priorizando left mas pode ser alterado
                LeaderBlock.GetBlock(PositionFromPlayer + Direction.Left.AsVector2()).AddBlockClockwise(baseBlock);
        }

        public void AddBlockFromCollision(BaseBlock baseBlock, GameObject go)
        {
            var direction = go.transform.position - transform.position;
            // Debug.Log("[OnTriggerEnter2D] direction -> " + direction);

            if (math.abs(direction.x) > math.abs(direction.y)) //É uma conexão horizontal
                ConnectBlock(baseBlock, direction.x < 0 ? Direction.Left : Direction.Right);
            else //É uma conexão vertical
                ConnectBlock(baseBlock, Direction.Up);

            // ConnectBlock(block, direction.y < 0 ? Direction.Down : Direction.Up); substituir aqui se for voltar a ter conexão por baixo
        }

        private void AddNeighbour(BaseBlock baseBlock, Direction direction)
        {
            LeaderBlock.AddBlock(baseBlock, PositionFromPlayer + direction.AsVector2());
        }

        private BaseBlock GetNeighbour(Direction direction)
        {
            return LeaderBlock.GetBlock(PositionFromPlayer + direction.AsVector2());
        }

        private BaseBlock GetNeighbourSafe(Direction direction)
        {
            return LeaderBlock.GetBlockSafe(PositionFromPlayer + direction.AsVector2());
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