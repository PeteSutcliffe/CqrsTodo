using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Todo.Command;
using Todo.Command.Infrastructure;

namespace Todo.Infrastructure.Azure
{
    public class CommandSender : ICommandBus
    {
        private readonly string _connectionString;
        private const string QueueName = "commandqueue";

        public CommandSender(string connectionString)
        {
            _connectionString = connectionString;
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }           
        }

        public Task SendAsync(ICommand command)
        {   
            var queueClient = QueueClient.CreateFromConnectionString(_connectionString, QueueName);
            var message = new BrokeredMessage(command);
            message.Properties["messageType"] = command.GetType().AssemblyQualifiedName;
            return queueClient.SendAsync(message);
        }
    }
}
