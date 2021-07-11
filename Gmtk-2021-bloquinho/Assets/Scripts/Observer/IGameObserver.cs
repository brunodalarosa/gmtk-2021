namespace Observer
{
    public interface IGameObserver<T> where T : IEvent
    {
        void ReceiveEvent(object subject, T data);
    }
}