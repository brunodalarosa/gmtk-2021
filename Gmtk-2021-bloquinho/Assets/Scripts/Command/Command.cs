using Block;

namespace Command
{
    public abstract class BaseCommand
    {
        protected BaseCommand()
        {
        }

        public abstract void Execute(BaseBlock block);
    }
}