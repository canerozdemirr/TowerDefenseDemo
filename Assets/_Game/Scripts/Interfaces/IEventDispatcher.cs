using System;

namespace Interfaces
{
    public interface IEventDispatcher
    {
        void Subscribe<T>(Action<T> handler) where T : IEvent;
        void Unsubscribe<T>(Action<T> handler) where T : IEvent;
        void Dispatch<T>(T payload) where T : IEvent;
        void UnsubscribeAll<T>() where T : IEvent;
        void Clear();
        void Dispatch<T>(params T[] events) where T : IEvent;
    }
}
