﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Todo.Domain
{
    public abstract class EntityBase : IEventProvider
    {
        private readonly Dictionary<Type, Action<IEvent>> _events = new Dictionary<Type, Action<IEvent>>();

        protected EntityBase()
        {
            EventsRaised = new List<IEvent>();
            RegisterEvents();
        }

        public ICollection<IEvent> EventsRaised { get; private set; }

        protected void RaiseEvent(IEvent ev)
        {
            EventsRaised.Add(ev);
            Apply(ev);
        }

        public void LoadFromEvents(IEnumerable<IEvent> events)
        {
            events.ToList().ForEach(Apply);
        }

        private void Apply(IEvent ev)
        {
            var handler = _events[ev.GetType()];
            handler(ev);
        }

        protected void RegisterEvent<TEvent>(Action<TEvent> eventHandler) where TEvent : class, IEvent
        {
            _events.Add(typeof(TEvent), theEvent => eventHandler(theEvent as TEvent));
        }

        protected abstract void RegisterEvents();
    }
}