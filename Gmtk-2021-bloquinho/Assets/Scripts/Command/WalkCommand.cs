using Block;

namespace Command
{
    public class WalkCommand : BaseCommand
    {
        private float HorizontalInput { get; }

        public WalkCommand(float horizontalInput)
        {
            HorizontalInput = horizontalInput;
        }

        public override void Execute(LeaderBlock leaderBlock)
        {
            leaderBlock.HandleMovement(HorizontalInput);
        }
    }
}