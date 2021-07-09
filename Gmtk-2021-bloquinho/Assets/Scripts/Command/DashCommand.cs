using Block;

namespace Command
{
    public class DashCommand : BaseCommand
    {
        public DashCommand()
        {
        }

        public override void Execute(BaseBlock block)
        {
            switch (block)
            {
                case DashBlock dashBlock:
                    dashBlock.DoAction();
                    break;
                case PlayerBlock playerBlock:
                    playerBlock.Dash();
                    break;
            }
        }
    }
}