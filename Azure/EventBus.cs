using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Todo.Domain;
using Todo.Domain.Infrastructure;

namespace Todo.Infrastructure.Azure
{
    public class EventBus : IEventBus
    {
        private readonly string _connectionString;
        private const string TopicName = "eventTopic";

        public EventBus(string connectionString)
        {
            _connectionString = connectionString;
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.TopicExists(TopicName))
            {
                namespaceManager.CreateTopic(TopicName);
            }
        }

        public void Publish(ICollection<IEvent> events)
        {   
            var topicClient = TopicClient.CreateFromConnectionString(_connectionString, TopicName);
            
            topicClient.SendBatch(events.Select(e =>
            {
                var message = new BrokeredMessage(e);
                message.Properties["messageType"] = e.GetType().AssemblyQualifiedName;
                return message;
            }));
        }
    }
}