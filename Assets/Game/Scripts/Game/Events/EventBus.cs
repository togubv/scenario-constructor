using System;
using System.Collections.Generic;

namespace SC
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new ();

        public static void Subscribe<T>(Action<T> callback)
        {
            if (_events.TryGetValue(typeof(T), out var existing))
            {
                _events[typeof(T)] = (Action<T>)Delegate.Combine(existing, callback);
            }
            else
            {
                _events[typeof(T)] = callback;
            }
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            if (_events.TryGetValue(typeof(T), out var existing))
            {
                var updated = (Action<T>)Delegate.Remove(existing, callback);

                if (updated == null)
                {
                    _events.Remove(typeof(T));
                }
                else
                {
                    _events[typeof(T)] = updated;
                }
            }
        }

        public static void Raise<T>(T eventData)
        {
            if (_events.TryGetValue(typeof(T), out var callback))
            {
                ((Action<T>)callback)?.Invoke(eventData);
            }
        }
    }
}
