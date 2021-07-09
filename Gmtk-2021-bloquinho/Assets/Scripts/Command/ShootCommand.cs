using Block;

namespace Command
{
    public class ShootCommand : BaseCommand
    {
        public ShootCommand()
        {
        }

        public override void Execute(BaseBlock block)
        {
            switch (block)
            {
                case PlayerBlock playerBlock:
                    playerBlock.Shoot();
                    break;
                case ShootingBlock shootingBlock:
                    shootingBlock.DoAction();
                    break;
            }
        }
    }
}