using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace Todo.Infrastructure.Azure.TableEntities
{
    public class Event : TableEntity
    {
        public Event()
        {
            
        }

        public Event(Guid aggregateId, int version)
        {
            PartitionKey = aggregateId.ToString();
            RowKey = version.ToString(CultureInfo.InvariantCulture);
            Timestamp = Timestamp.DateTime;
        }
        
        public string EventData { get; set; }
        public string EventType { get; set; }
    }
}