using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Todo.Domain;
using Todo.Infrastructure.Azure.TableEntities;

namespace Todo.Infrastructure.Azure
{
    public class TableStorageRepository : IRepository
    {
        private readonly CloudTable _eventProviders;
        private readonly CloudTable _events;

        static TableStorageRepository()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist.
            var table = tableClient.GetTableReference("eventproviders");
            table.CreateIfNotExists();
            table = tableClient.GetTableReference("events");
            table.CreateIfNotExists();
        }

        public TableStorageRepository()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();
            _eventProviders = tableClient.GetTableReference("eventproviders");
            _events = tableClient.GetTableReference("events");
        }

        public T Load<T>(Guid id) where T : ILoadFromEvents, new()
        {
            var query = new TableQuery<Event>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.ToString()));
            var existing = _events.ExecuteQuery(query);
            var events = new List<IEvent>();

            foreach (var ev in existing)
            {
                var type = Type.GetType(ev.EventType);
                events.Add((IEvent)JsonConvert.DeserializeObject(ev.EventData, type));
            }

            var entity = new T();
            entity.LoadFromEvents(events);

            return entity;
        }

        public void Save(IEventProvider entity)
        {
            var events = entity.EventsRaised;

            var retrieveOperation = TableOperation.Retrieve<EventProvider>(entity.GetType().AssemblyQualifiedName, entity.Id.ToString());

            var retrievedResult = _eventProviders.Execute(retrieveOperation);

            if (retrievedResult.Result == null)
            {
                var provider = new EventProvider(entity.Id, entity.GetType().AssemblyQualifiedName);
                var insertOperation = TableOperation.Insert(provider);
                _eventProviders.Execute(insertOperation);
            }

            var query = new TableQuery<Event>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, entity.Id.ToString()));
            var existing = _events.ExecuteQuery(query).ToList();
            
            var currentVersion = existing.Any() ? existing.Max(e => int.Parse(e.RowKey)) : 0;
            var batchOperation = new TableBatchOperation();

            //todo: concurrency

            foreach (var ev in events)
            {
                var type = ev.GetType().AssemblyQualifiedName;
                var json = JsonConvert.SerializeObject(ev);
                var tableEvent = new Event(entity.Id, ++currentVersion)
                {
                    EventData = json,
                    EventType = type
                };
                batchOperation.Insert(tableEvent);
            }
            
            _events.ExecuteBatch(batchOperation);
        }
    }
}