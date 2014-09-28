using System;
using Todo.Domain;

namespace Todo.Infrastructure.Azure
{
    public class TableStorageRepository : IRepository
    {
        public T Load<T>(Guid id) where T : new()
        {
            throw new NotImplementedException();
        }

        public void Save(IEventProvider entity)
        {
            throw new NotImplementedException();
        }
    }
}