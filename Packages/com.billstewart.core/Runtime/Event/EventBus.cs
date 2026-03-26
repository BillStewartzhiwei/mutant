using System;
using System.Collections.Generic;

namespace Bill.Mutant.Core
{
    /// <summary>
    /// 简单的进程内事件总线：支持优先级、一次性监听、批量移除与运行时调试信息。
    /// </summary>
    public static class EventBus
    {
        private sealed class Listener
        {
            public Delegate Callback;
            public int Priority;
            public bool Once;
        }

        private static readonly Dictionary<Type, List<Listener>> Listeners = new();

        /// <summary>
        /// 当前总线中的监听总数（所有事件类型累计）。
        /// </summary>
        public static int ListenerCount
        {
            get
            {
                var count = 0;
                foreach (var pair in Listeners)
                {
                    count += pair.Value.Count;
                }

                return count;
            }
        }

        /// <summary>
        /// 订阅事件，并返回一个可释放的句柄（Dispose 即取消订阅）。
        /// </summary>
        public static IDisposable Subscribe<T>(Action<T> callback, int priority = 0, bool once = false)
            where T : IEvent
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var type = typeof(T);

            if (!Listeners.TryGetValue(type, out var list))
            {
                list = new List<Listener>();
                Listeners[type] = list;
            }

            list.Add(new Listener
            {
                Callback = callback,
                Priority = priority,
                Once = once
            });

            list.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            return new Subscription(() => Unsubscribe(callback));
        }

        /// <summary>
        /// 订阅一次性事件（收到一次后自动移除）。
        /// </summary>
        public static IDisposable SubscribeOnce<T>(Action<T> callback, int priority = 0)
            where T : IEvent
        {
            return Subscribe(callback, priority, once: true);
        }

        /// <summary>
        /// 取消一个事件回调的订阅。
        /// </summary>
        public static bool Unsubscribe<T>(Action<T> callback)
            where T : IEvent
        {
            if (callback == null) return false;

            var type = typeof(T);
            if (!Listeners.TryGetValue(type, out var list)) return false;

            var removed = list.RemoveAll(l => l.Callback.Equals(callback)) > 0;

            if (list.Count == 0)
            {
                Listeners.Remove(type);
            }
            return removed;
        }

        /// <summary>
        /// 发布事件。
        /// </summary>
        public static void Publish<T>(T evt)
            where T : IEvent
        {
            var type = typeof(T);

            if (!Listeners.TryGetValue(type, out var list) || list.Count == 0) return;

            var snapshot = list.ToArray();
            var toRemove = new List<Listener>();

            foreach (var listener in snapshot)
            {
                if (listener.Callback is not Action<T> cb) continue;

                cb.Invoke(evt);

                if (listener.Once)
                {
                    toRemove.Add(listener);
                }
            }

            foreach (var listener in toRemove)
            {
                list.Remove(listener);
            }

            if (list.Count == 0)
            {
                Listeners.Remove(type);
            }
        }

        /// <summary>
        /// 清空某一类事件的全部监听。
        /// </summary>
        public static void Clear<T>()
            where T : IEvent
        {
            Listeners.Remove(typeof(T));
        }

        /// <summary>
        /// 清空所有监听。
        /// </summary>
        public static void Clear()
        {
            Listeners.Clear();
        }

        private sealed class Subscription : IDisposable
        {
            private Action _dispose;

            public Subscription(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                _dispose?.Invoke();
                _dispose = null;
            }
        }
    }
}
