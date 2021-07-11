using System.Collections.Generic;

namespace Observer
{
    public class Subject<T> where T : IEvent
    {
        private readonly List<IGameObserver<T>> _observers = new List<IGameObserver<T>>();

        public void AddObserver(IGameObserver<T> gameObserver)
        {
            _observers.Add(gameObserver);
        }

        public void ClearObservers()
        {
            _observers?.Clear();
        }

        protected void SendEvent(object subject, T data)
        {
            foreach (var observer in _observers)
            {
                observer.ReceiveEvent(subject, data);
            }
        }
    }
}