using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Domain.Infrastructure;

namespace Todo.CommandHandlers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepository _repository;
        private readonly IEventBus _eventBus;
        private List<IEventProvider> _eventProviders = new List<IEventProvider>();

        public UnitOfWork(IRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public T Load<T>(Guid id) where T : IEventProvider, new()
        {
            var aggregate = _repository.Load<T>(id);
            this.RegisterForTracking(aggregate);
            return aggregate;
        }

        public void Add(IEventProvider aggregate)
        {
            RegisterForTracking(aggregate);
        }

        public void Commit()
        {
            try
            {
                _eventProviders.ForEach(e => _repository.Save(e));
                _eventBus.Publish(_eventProviders.SelectMany(e => e.GetChanges()));
            }
            catch (Exception)
            {
                //uh oh... do something here
                throw;
            }            
        }

        private void RegisterForTracking(IEventProvider aggregate)
        {
            _eventProviders.Add(aggregate);
        }
    }
}