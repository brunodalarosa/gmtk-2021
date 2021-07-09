using Block;

namespace Command
{
    public class JumpCommand : BaseCommand
    {
        public JumpCommand()
        {
        }

        public override void Execute(BaseBlock block)
        {
            //todo alterar para que seja possível chamar de JumpBlock
            if (block is PlayerBlock playerBlock)
            {
                playerBlock.Jump();
            }
        }
    }
}