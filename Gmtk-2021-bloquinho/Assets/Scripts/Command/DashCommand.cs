using Block;

namespace Command
{
    public class DashCommand : BaseCommand
    {
        public DashCommand()
        {
        }

        public override void Execute(LeaderBlock leaderBlock)
        {
            leaderBlock.Dash();
        }
    }
}