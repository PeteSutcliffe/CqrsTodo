using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Domain.Events;
using Todo.Domain.Infrastructure;

namespace Todo.Domain
{
    public abstract class BaseAggregateRoute : IEventProvider
    {
        private readonly Dictionary<Type, Action<IEvent>> _events = new Dictionary<Type, Action<IEvent>>();
        private int _version;
        private readonly ICollection<IEvent> _eventsRaised;
        private readonly List<IEventProvider> _childEventProviders;

        protected BaseAggregateRoute()
        {
            _eventsRaised = new List<IEvent>();
            _childEventProviders = new List<IEventProvider>();
            RegisterEvents();
        }
               
        public Guid Id { get; protected set; }
        
        public IEnumerable<IEvent> GetChanges()
        {
            return _eventsRaised.Concat(_childEventProviders.SelectMany(p => p.GetChanges()));
        }

        protected void RaiseEvent(IEvent ev)
        {
            ev.Version = GetNewVersion();
            _eventsRaised.Add(ev);
            Apply(ev);
        }

        protected void RegisterChildEventProvider(ISubEventProvider childEntity)
        {
            childEntity.HookupVersionProvider(GetNewVersion);
            _childEventProviders.Add(childEntity);
        }

        private int GetNewVersion()
        {
            return ++_version;
        }

        public void LoadFromEvents(IEnumerable<IEvent> events)
        {
            var list = events.OrderBy(e => e.Version).ToList();
            list.ForEach(Apply);
            _version = list.Max(e => e.Version);
        }

        private void Apply(IEvent ev)
        {
            if (ev.Id != ev.AggregateId)
            {
                var entity = _childEventProviders.SingleOrDefault(p => p.Id == ev.Id);
                if (entity == null)
                {
                    throw new InvalidOperationException("Child entity was not properly initialized");
                }

                entity.LoadFromEvents(new[]{ev});
                return;
            }

            var handler = _events[ev.GetType()];
            handler(ev);
        }

        protected void RegisterEvent<TEvent>(Action<TEvent> eventHandler) where TEvent : class, IEvent
        {
            //todo: auto registration / routing
            _events.Add(typeof(TEvent), theEvent => eventHandler(theEvent as TEvent));
        }

        protected abstract void RegisterEvents();
    }
}