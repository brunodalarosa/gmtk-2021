using System;
using System.Collections.Generic;
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

        protected Vector2 PositionFromPlayer { get; set; }
        protected PlayerBlock PlayerBlock { get; set; }
        protected bool IsConnected { private get; set; }

        private bool DoingAction { get; set; }
        private Transform Transform { get; set; }
        private Dictionary<Direction, Block> Blocks { get; set; }
        
        private void Awake()
        {
            Transform = transform;
            IsConnected = false;
            tag = "Block";
            Blocks = new Dictionary<Direction, Block>
                {{Direction.Left, null}, {Direction.Up, null}, {Direction.Right, null}, {Direction.Down, null}};
            
            DidAwake();
        }
        
        protected void DoAction()
        {
            if (DoingAction)
                return;
            
            DoingAction = true;
            
            Action();

            foreach (var block in Blocks.Values)
            {
                if (block != null)
                    block.DoAction();
            }
            
            DoingAction = false;
        }

        private void ConnectBlock(Block block, Direction direction)
        {
            Blocks[direction] = block;
            block.ConnectBlockFrom(this, direction, PlayerBlock);
        }
        
        private void ConnectBlockFrom(Block block, Direction direction, PlayerBlock playerBlock)
        {
            PlayerBlock = playerBlock;
            Blocks[direction.Opposite()] = block;

            Transform.SetParent(block.Transform);
            Transform.localPosition = direction.AsVector3() * 1.05f; // Para dar um espaço entre os blocos

            PositionFromPlayer = block.PositionFromPlayer + direction.AsVector2();
            gameObject.name = $"{direction.ToString()}, x:{PositionFromPlayer.x}, y:{PositionFromPlayer.y}";

            SetupRigidbody();
            
            IsConnected = true;

            SetupNeighbors(direction);
            
            OnConnect();
        }

        private void SetupRigidbody()
        {
            var rg = gameObject.AddComponent<Rigidbody2D>();

            rg.bodyType = RigidbodyType2D.Kinematic;
            rg.freezeRotation = true;
            
            Collider.isTrigger = false;
        }

        // Configura todos os vizinhos do novo objeto adicionado
        private void SetupNeighbors(Direction direction)
        {
            // Cria uma lista de direções que forma um circulo a partir da posição de entrada. Lembrando que Opposite e ClockWise não alteram o objeto, então precisa chamar repetido
            var directions = new List<Direction> {direction.Opposite().Clockwise(), 
                direction.Opposite().Clockwise().Clockwise(), direction.Opposite().Clockwise().Clockwise(), 
                direction.Opposite().Clockwise().Clockwise().Clockwise(),
                direction.Opposite().Clockwise().Clockwise().Clockwise(), 
                direction.Opposite().Clockwise().Clockwise().Clockwise().Clockwise()};

            var currentBlockIndex = 0;
            var currentDirection = directions[currentBlockIndex];
            var currentBlock = Blocks[direction.Opposite()].Blocks[currentDirection];

            while (currentBlock != null)
            {
                if (currentBlockIndex % 2 != 0)
                {
                    currentBlock.Blocks[currentDirection.Clockwise()] = this;
                    Blocks[currentDirection.Clockwise().Opposite()] = currentBlock;
                }
                
                currentBlockIndex++;
                currentDirection = directions[currentBlockIndex];
                currentBlock = currentBlock.Blocks[currentDirection];
            }
        }
        
        private void AddBlock(Block block)
        {
            var direction = Direction.Down;
            
            // Ordem de adição dos blocos. Para alterar, só alterar a ordem da validação. Pode ser até aleatório.
            
            if (Blocks[Direction.Up] == null)
                direction = Direction.Up;
            else if (Blocks[Direction.Right] == null)
                direction = Direction.Right;
            else if (Blocks[Direction.Left] == null)
                direction = Direction.Left;
            
            if (direction != Direction.Down)
                ConnectBlock(block, direction);
            else // Todas as direções estão ocupadas, vai para a próxima. Está priorizando left mas pode ser alterado
                Blocks[Direction.Left].AddBlock(block);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsConnected)
                return;

            var go = other.gameObject;
            
            if (!go.CompareTag("Block"))
                return;

            var block = go.GetComponent<Block>();

            if (block.IsConnected)
                return;
            
            Debug.Log($"Trigger: from:{gameObject.name}, to:{go.name}");
            
            AddBlock(block);
        }

        protected virtual void OnConnect() { }
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
