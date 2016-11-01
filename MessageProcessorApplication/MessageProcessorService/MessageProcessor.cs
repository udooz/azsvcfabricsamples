namespace MessageProcessorService
{
    using System.Diagnostics;
    using System.Runtime.Serialization.Json;
    using System.Threading;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class MessageProcessor : IEventProcessor
    {
        PartitionContext partitionContext;
        Stopwatch checkpointStopWatch;
        CloudStorageAccount storageAccount;
        CloudTableClient tableClient;
        CloudTable table;

        public MessageProcessor()
        {
            storageAccount = CloudStorageAccount.Parse("AccountName=uzevttable;AccountKey=8hGJNFB0TIWtJ4koumyv1ZAIaGxkP0OhGfHlMmaGIpVNvVQChs7z006jjbvbvETo24nThu88iQshFJJmp23jpg==;DefaultEndpointsProtocol=https");

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            table = tableClient.GetTableReference("uzmessages");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
        }

        public Task OpenAsync(PartitionContext context)
        {
            var logMsg = string.Format("MessageProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset);
            ServiceEventSource.Current.Message(logMsg);
            this.partitionContext = context;
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> events)
        {
            try
            {
                foreach (EventData eventData in events)
                {                    

                    var newData = this.DeserializeEventData(eventData);
                    string key = eventData.PartitionKey;

                    var msgEntity = new MessageEntity(key);
                    msgEntity.Message = newData;

                    // Create the TableOperation object that inserts the customer entity.
                    var insertOperation = TableOperation.Insert(msgEntity);

                    // Execute the insert operation.
                    table.Execute(insertOperation);

                    ServiceEventSource.Current.Message(string.Format("Message received.  Partition: '{0}', Device: '{1}', Data: '{2}'",this.partitionContext.Lease.PartitionId, key, newData));
                }

                //Call checkpoint every 5 minutes, so that worker can resume processing from the 5 minutes back if it restarts.
                if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
                {
                    await context.CheckpointAsync();
                    this.checkpointStopWatch.Restart();
                }
            }
            catch (Exception exp)
            {
                ServiceEventSource.Current.Message("Error in processing: " + exp.Message);
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            ServiceEventSource.Current.Message(string.Format("Processor Shuting Down.  Partition '{0}', Reason: '{1}'.", this.partitionContext.Lease.PartitionId, reason.ToString()));
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        string DeserializeEventData(EventData eventData)
        {
            return JsonConvert.DeserializeObject<string>(Encoding.UTF8.GetString(eventData.GetBytes()));
        }

    }

    public class MessageEntity : TableEntity
    {
        public MessageEntity(string pk)
        {
            this.PartitionKey = pk;
            this.RowKey = Guid.NewGuid().ToString();
        }

        public MessageEntity() { }

        public string Message { get; set; }
    }
}