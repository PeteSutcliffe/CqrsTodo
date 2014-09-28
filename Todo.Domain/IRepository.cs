using System;

namespace Todo.Domain
{
    public interface IRepository
    {
        T Load<T>(Guid id) where T:ILoadFromEvents,new();
        void Save(IEventProvider entity);
    }
}