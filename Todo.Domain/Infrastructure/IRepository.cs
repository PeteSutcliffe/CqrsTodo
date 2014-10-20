using System;

namespace Todo.Domain.Infrastructure
{
    public interface IRepository
    {
        T Load<T>(Guid id) where T : IEventProvider, new();
        void Save(IEventProvider aggregate);
    }

    public interface IUnitOfWork
    {
        T Load<T>(Guid id) where T : IEventProvider, new();
        void Add(IEventProvider aggregate);
        void Commit();
    }
}