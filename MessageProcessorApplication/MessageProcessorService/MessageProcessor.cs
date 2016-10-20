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

    public class MessageProcessor : IEventProcessor
    {
        IDictionary<string, int> map;
        PartitionContext partitionContext;
        Stopwatch checkpointStopWatch;

        public MessageProcessor()
        {
            this.map = new Dictionary<string, int>();
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

                    ServiceEventSource.Current.Message(string.Format("Message received.  Partition: '{0}', Device: '{1}', Data: '{2}'",
                        this.partitionContext.Lease.PartitionId, key, newData));
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
}