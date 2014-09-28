using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using StructureMap;

namespace Todo.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
        const string QueueName = "ProcessingQueue";

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        private SubscriptionClient _client;
        readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);

        private readonly IContainer _container = new Container();
        private CommandReceiver _receiver;

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");
            _receiver.Start();

            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            //_client.OnMessage((receivedMessage) =>
            //    {
            //        try
            //        {
            //            // Process the message
            //            Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
            //        }
            //        catch
            //        {
            //            // Handle any message processing specific exceptions here
            //        }
            //    });

            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            Bootstrapper.Bootstrap(_container);
            _receiver = new CommandReceiver(RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBus.ConnectionString"), _container);

            //// Create the queue if it does not exist already
            //string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            //var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            //if (!namespaceManager.QueueExists(QueueName))
            //{
            //    namespaceManager.CreateQueue(QueueName);
            //}

            // Initialize the connection to Service Bus Queue
            //_client = SubscriptionClient.CreateFromConnectionString(connectionString, QueueName, "Test");
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            //_client.Close();
            _completedEvent.Set();
            base.OnStop();
        }
    }
}
