using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using StructureMap;
using Todo.Command;
using Todo.CommandHandlers;

namespace Todo.Worker
{
    public class CommandReceiver
    {
        private readonly string _connectionString;
        private readonly IContainer _container;
        private MessagingFactory _factory;
        private const string QueueName = "commandqueue";
        private QueueClient _client;

        public CommandReceiver(string connectionString, IContainer container)
        {
            _connectionString = connectionString;
            _container = container;            
        }

        public void Start()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(_connectionString);

            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            _client = QueueClient.CreateFromConnectionString(_connectionString, QueueName, ReceiveMode.ReceiveAndDelete);

            var options = new OnMessageOptions();
            options.AutoComplete = true; // Indicates if the message-pump should call complete on messages after the callback has completed processing.

            options.MaxConcurrentCalls = 1; // Indicates the maximum number of concurrent calls to the callback the pump should initiate 

            options.ExceptionReceived += LogErrors; // Enables you to be notified of any errors encountered by the message pump

            // Start receiveing messages
            _client.OnMessage((receivedMessage) => // Initiates the message pump and callback is invoked for each message that is received. Calling Close() on the client will stop the pump.
            {
                //var command = receivedMessage.GetBody<ICommand>();
                var messageBodyType = Type.GetType(receivedMessage.Properties["messageType"].ToString());
                var method = typeof(BrokeredMessage).GetMethod("GetBody", new Type[] { });
                var generic = method.MakeGenericMethod(messageBodyType);
                var messageBody = generic.Invoke(receivedMessage, null);
                var command = (dynamic) messageBody;

                var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
                dynamic handler = _container.GetInstance(handlerType);
                handler.Handle(command);
            }, options);
        }

        private void LogErrors(object sender, ExceptionReceivedEventArgs e)
        {
            Trace.WriteLine("error");
        }
    }
}