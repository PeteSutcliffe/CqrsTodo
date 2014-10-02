using System;
using System.Diagnostics;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using StructureMap;
using Todo.Domain.Events;

namespace Todo.Worker
{
    public class EventSubscriber
    {
        private readonly string _connectionString;
        private readonly string _subscriptionName;
        private readonly IContainer _container;
        private const string TopicName = "eventTopic";
        private SubscriptionClient _client;

        public EventSubscriber(string connectionString, string subscriptionName, IContainer container)
        {
            _connectionString = connectionString;
            _subscriptionName = subscriptionName;
            _container = container;            
        }

        public void Start()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(_connectionString);

            if (!namespaceManager.TopicExists(TopicName))
            {
                namespaceManager.CreateTopic(TopicName);
            }

            if (!namespaceManager.SubscriptionExists(TopicName, _subscriptionName))
            {
                namespaceManager.CreateSubscription(TopicName, _subscriptionName);
            }

            _client = SubscriptionClient.CreateFromConnectionString(_connectionString, TopicName, _subscriptionName, ReceiveMode.ReceiveAndDelete);

            var options = new OnMessageOptions();
            options.AutoComplete = true; // Indicates if the message-pump should call complete on messages after the callback has completed processing.

            options.MaxConcurrentCalls = 1; // Indicates the maximum number of concurrent calls to the callback the pump should initiate 

            options.ExceptionReceived += LogErrors; // Enables you to be notified of any errors encountered by the message pump

            // Start receiveing messages
            _client.OnMessage(receivedMessage => // Initiates the message pump and callback is invoked for each message that is received. Calling Close() on the client will stop the pump.
            {
                //var command = receivedMessage.GetBody<ICommand>();
                var messageBodyType = Type.GetType(receivedMessage.Properties["messageType"].ToString());
                var method = typeof(BrokeredMessage).GetMethod("GetBody", new Type[] { });
                var generic = method.MakeGenericMethod(messageBodyType);
                var messageBody = generic.Invoke(receivedMessage, null);
                var @event = (dynamic) messageBody;

                var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
                dynamic handler = _container.GetInstance(handlerType);
                handler.Handle(@event);
            }, options);
        }

        private void LogErrors(object sender, ExceptionReceivedEventArgs e)
        {
            Trace.WriteLine("error");
        }
    }
}