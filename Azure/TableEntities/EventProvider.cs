using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Todo.Infrastructure.Azure.TableEntities
{
    public class EventProvider : TableEntity
    {
        public EventProvider(Guid id, string type)
        {
            PartitionKey = type;
            RowKey = id.ToString();
            Timestamp = Timestamp.DateTime;
        }

        public EventProvider()
        {
            
        }
    }
}