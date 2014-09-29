using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using StructureMap;
using Todo.EventHandlers.ReadModel;

namespace Todo.Worker
{
    public class WorkerRole : RoleEntryPoint
    {        
        readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);

        private readonly IContainer _container = new Container();
        private CommandReceiver _receiver;
        readonly List<Tuple<IContainer, EventSubscriber>> _eventSubscribers = new List<Tuple<IContainer, EventSubscriber>>();

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");
            _receiver.Start();           
            _eventSubscribers.ForEach(t => t.Item2.Start());

            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            Bootstrapper.Bootstrap(_container);
            var connectionString = RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBus.ConnectionString");
            _receiver = new CommandReceiver(connectionString, _container);

            var readModelContainer = new Container(new ReadModelRegistry());
            var eventSubscriber = new EventSubscriber(connectionString, "readmodelsubscription", readModelContainer);

            _eventSubscribers.Add(new Tuple<IContainer, EventSubscriber>(readModelContainer,
                eventSubscriber));            

            return base.OnStart();
        }

        public override void OnStop()
        {            
            _completedEvent.Set();
            _receiver.Stop();
            base.OnStop();
        }
    }
}
