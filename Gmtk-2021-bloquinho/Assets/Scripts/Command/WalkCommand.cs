using Block;
using UnityEngine;

namespace Command
{
    public class WalkCommand : BaseCommand
    {
        private float HorizontalInput { get; }

        public WalkCommand(float horizontalInput)
        {
            HorizontalInput = horizontalInput;
        }

        public override void Execute(BaseBlock block)
        {
            if (block is PlayerBlock playerBlock)
            {
                playerBlock.HandleMovement(HorizontalInput);
            }
        }
    }
}