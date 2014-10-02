using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Domain.Events;
using Todo.Domain.Infrastructure;

namespace Todo.Domain
{
    public abstract class BaseEntity : ISubEventProvider
    {
        private Func<int> _versionProvider;
        public Guid Id { get; protected set; }
        private readonly ICollection<IEvent> _eventsRaised;
        private readonly Dictionary<Type, Action<IEvent>> _events = new Dictionary<Type, Action<IEvent>>();

        IEnumerable<IEvent> IEventProvider.EventsRaised()
        {
            return _eventsRaised;
        }

        protected BaseEntity()
        {
            _eventsRaised = new List<IEvent>();
            RegisterEvents();
        }

        protected void RaiseEvent(IEvent ev)
        {
            Apply(ev);
            ev.Version = _versionProvider();
            _eventsRaised.Add(ev);            
        }

        void ISubEventProvider.HookupVersionProvider(Func<int> versionProvider)
        {
            _versionProvider = versionProvider;
        }

        private void Apply(IEvent ev)
        {
            var handler = _events[ev.GetType()];
            handler(ev);
        }

        protected void RegisterEvent<TEvent>(Action<TEvent> eventHandler) where TEvent : class, IEvent
        {
            //todo: auto registration / routing
            _events.Add(typeof(TEvent), theEvent => eventHandler(theEvent as TEvent));
        }

        protected abstract void RegisterEvents();

        public void LoadFromEvents(IEnumerable<IEvent> events)
        {
            var list = events.ToList();
            list.ForEach(Apply);
        }
    }
}