using Observer;

namespace Block
{
    public class BlockSubject : Subject<IEvent>
    {
        public void SendDeathEvent(object subject)
        {
            SendEvent(subject, new PlayerDiedEvent());
        }

        public void SendReachedEndOfLevelEvent(object subject)
        {
            SendEvent(subject, new PlayerReachedEndOfLevelEvent());
        }
    }
}