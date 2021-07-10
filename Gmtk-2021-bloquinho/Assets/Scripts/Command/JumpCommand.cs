using Block;

namespace Command
{
    public class JumpCommand : BaseCommand
    {
        public JumpCommand()
        {
        }

        public override void Execute(LeaderBlock leaderBlock)
        {
            leaderBlock.Jump();
        }
    }
}