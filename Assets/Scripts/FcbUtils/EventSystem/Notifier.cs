using System;
using System.Collections.Generic;

namespace FcbUtils.EventSystem
{
    public sealed class Notifier
    {
        private class BroadcastHandlerKey
        {
            private readonly Type _eventType;
            private readonly Delegate _handler;

            public BroadcastHandlerKey(Type eventType, Delegate handler)
            {
                _eventType = eventType;
                _handler = handler;
            }

            protected bool Equals(BroadcastHandlerKey other)
            {
                return _eventType == other._eventType && _handler == other._handler;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((BroadcastHandlerKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_eventType.GetHashCode() * 397) ^ _handler.GetHashCode();
                }
            }
        }

        private class SingleEventKey
        {
            private readonly int _id;
            private readonly Type _eventType;

            public SingleEventKey(int objectId, Type eventType)
            {
                _id = objectId;
                _eventType = eventType;
            }

            protected bool Equals(SingleEventKey other)
            {
                return _id == other._id && _eventType == other._eventType;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((SingleEventKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_id * 397) ^ _eventType.GetHashCode();
                }
            }
        }

        private class SingleHandlerKey
        {
            private readonly int _id;
            private readonly Type _eventType;
            private readonly Delegate _handler;

            public SingleHandlerKey(int objectId, Type eventType, Delegate handler)
            {
                _id = objectId;
                _eventType = eventType;
                _handler = handler;
            }

            protected bool Equals(SingleHandlerKey other)
            {
                return _id == other._id && _eventType == other._eventType && _handler == other._handler;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((SingleHandlerKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_id * 397) ^ _eventType.GetHashCode() ^ _handler.GetHashCode();
                }
            }
        }

        public static Notifier Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Notifier();
                }

                return _instance;
            }
        }

        private static Notifier _instance;

        private readonly Dictionary<Type, EventHandler> _delegates = new Dictionary<Type, EventHandler>();

        // To bypass private Dictionary<Type, EventHandler<T>> delegates = new Dictionary<Type, EventHandler<T>>(); generic
        // To check if specific method (listener) is already added for an event type
        private readonly Dictionary<BroadcastHandlerKey, EventHandler> _delegateLookup =
            new Dictionary<BroadcastHandlerKey, EventHandler>();

        private readonly Dictionary<SingleEventKey, EventHandler> _specificDelegates =
            new Dictionary<SingleEventKey, EventHandler>();

        private readonly Dictionary<SingleHandlerKey, EventHandler> _specificLookup =
            new Dictionary<SingleHandlerKey, EventHandler>();

        public void Reset()
        {
            _delegates.Clear();
            _delegateLookup.Clear();

            _specificDelegates.Clear();
            _specificLookup.Clear();
        }

        // TODO: Maybe instead of sourceId take object source and store object.GetHashCode()
        public void AddListener<T>(int sourceId, EventHandler<T> listener) where T : EventArgs
        {
            var eventType = typeof(T);

            var handlerKey = new SingleHandlerKey(sourceId, eventType, listener);
            if (_specificLookup.ContainsKey(handlerKey))
            {
                return;
            }

            EventHandler internalDelegate = (s, e) => listener(s, (T)e);
            _specificLookup[handlerKey] = internalDelegate;

            var eventKey = new SingleEventKey(sourceId, eventType);
            EventHandler invoker;
            if (_specificDelegates.TryGetValue(eventKey, out invoker))
            {
                invoker += internalDelegate;
                _specificDelegates[eventKey] = invoker;
            }
            else
            {
                _specificDelegates[eventKey] = internalDelegate;
            }
        }

        public void RemoveListener<T>(int sourceId, EventHandler<T> listener) where T : EventArgs
        {
            var eventType = typeof(T);
            var handlerKey = new SingleHandlerKey(sourceId, eventType, listener);

            EventHandler internalDelegate;
            if (_specificLookup.TryGetValue(handlerKey, out internalDelegate))
            {
                var eventKey = new SingleEventKey(sourceId, eventType);
                EventHandler invoker;
                if (_specificDelegates.TryGetValue(eventKey, out invoker))
                {
                    invoker -= internalDelegate;
                    if (invoker == null)
                    {
                        _specificDelegates.Remove(eventKey);
                    }
                    else
                    {
                        _specificDelegates[eventKey] = invoker;
                    }
                }

                _specificLookup.Remove(handlerKey);
            }
        }

        public void AddListener<T>(EventHandler<T> listener) where T : EventArgs
        {
            var eventType = typeof(T);

            var key = new BroadcastHandlerKey(eventType, listener);
            if (_delegateLookup.ContainsKey(key))
            {
                return;
            }

            EventHandler internalDelegate = (s, e) => listener(s, (T)e);
            _delegateLookup[key] = internalDelegate;

            EventHandler invoker;
            if (_delegates.TryGetValue(eventType, out invoker))
            {
                invoker += internalDelegate;
                _delegates[eventType] = invoker;
            }
            else
            {
                _delegates[eventType] = internalDelegate;
            }
        }

        public void RemoveListener<T>(EventHandler<T> listener) where T : EventArgs
        {
            var eventType = typeof(T);
            var key = new BroadcastHandlerKey(eventType, listener);

            EventHandler internalDelegate;
            if (_delegateLookup.TryGetValue(key, out internalDelegate))
            {
                EventHandler invoker;
                if (_delegates.TryGetValue(eventType, out invoker))
                {
                    invoker -= internalDelegate;
                    if (invoker == null)
                    {
                        _delegates.Remove(eventType);
                    }
                    else
                    {
                        _delegates[eventType] = invoker;
                    }
                }

                _delegateLookup.Remove(key);
            }
        }

        /// <summary>
        /// Remember that gameObject.GetInstanceId() is not equal to script.GetInstanceId()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sourceId"></param>
        /// <param name="e"></param>
        public void Raise(object sender, int sourceId, EventArgs e)
        {
            EventHandler invoker;
            if (_specificDelegates.TryGetValue(new SingleEventKey(sourceId, e.GetType()), out invoker))
            {
                invoker.Invoke(sender, e);
            }
        }

        public void Raise(object sender, EventArgs e)
        {
            EventHandler invoker;
            if (_delegates.TryGetValue(e.GetType(), out invoker))
            {
                invoker.Invoke(sender, e);
            }
        }
    }
}