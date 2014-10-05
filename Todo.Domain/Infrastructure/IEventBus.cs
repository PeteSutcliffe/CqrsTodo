using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Domain.Events;

namespace Todo.Domain.Infrastructure
{
    public interface IEventBus
    {
        void Publish(IEnumerable<IEvent> events);
    }
}