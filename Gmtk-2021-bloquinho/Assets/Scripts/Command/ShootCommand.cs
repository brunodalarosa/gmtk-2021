using Block;

namespace Command
{
    public class ShootCommand : BaseCommand
    {
        public ShootCommand()
        {
        }

        public override void Execute(LeaderBlock leaderBlock)
        {
            leaderBlock.Shoot();
        }
    }
}