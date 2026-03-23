using System;
using System.Collections.Generic;
using System.Linq;

namespace Bill.Mutant.Core
{
    public static class EventBus
    {
        private class Listener
        {
            public Delegate Callback;
            public int Priority;
            public bool Once;
        }

        private static readonly Dictionary<Type, List<Listener>> _listeners = new();

        public static void Subscribe<T>(Action<T> callback, int priority = 0, bool once = false)
        {
            var type = typeof(T);

            if (!_listeners.TryGetValue(type, out var list))
            {
                list = new List<Listener>();
                _listeners[type] = list;
            }

            list.Add(new Listener
            {
                Callback = callback,
                Priority = priority,
                Once = once
            });

            list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            if (!_listeners.TryGetValue(type, out var list)) return;

            list.RemoveAll(l => l.Callback.Equals(callback));
        }

        public static void Publish<T>(T evt)
        {
            var type = typeof(T);

            if (!_listeners.TryGetValue(type, out var list)) return;

            var toRemove = new List<Listener>();

            foreach (var listener in list)
            {
                if (listener.Callback is Action<T> cb)
                {
                    cb.Invoke(evt);

                    if (listener.Once)
                        toRemove.Add(listener);
                }
            }

            foreach (var l in toRemove)
            {
                list.Remove(l);
            }
        }

        public static void Clear()
        {
            _listeners.Clear();
        }
    }
}
