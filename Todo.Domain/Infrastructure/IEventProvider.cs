using System;
using System.Collections.Generic;
using Todo.Domain.Events;

namespace Todo.Domain.Infrastructure
{
    public interface IEventProvider : ILoadFromEvents
    {
        Guid Id { get; }
        IEnumerable<IEvent> GetChanges();
    }

    public interface ISubEventProvider : IEventProvider
    {
        void HookupVersionProvider(Func<int> versionProvider);
    }
}