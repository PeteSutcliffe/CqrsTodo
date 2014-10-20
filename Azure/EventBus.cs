using System.Collections.Generic;
using System.Linq;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Todo.Domain.Events;
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

        public void Publish(IEnumerable<IEvent> events)
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