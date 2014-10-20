using System;
using Todo.Domain.Infrastructure;

namespace Todo.EventStore
{
    public interface IDomainRepository
    {
        TAggregate GetById<TAggregate>(Guid id)
            where TAggregate : class, IEventProvider, new();

        void Add<TAggregate>(TAggregate aggregateRoot)
            where TAggregate : class, IEventProvider, new();
    }
}
