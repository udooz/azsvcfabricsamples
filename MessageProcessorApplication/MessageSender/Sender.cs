using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSender
{
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;

    public class Sender
    {
        const int numberOfDevices = 1000;
        string eventHubName;
        int numberOfMessages;
        public Sender(string eventHubName, int numberOfMessages)
        {
            this.eventHubName = eventHubName;
            this.numberOfMessages = numberOfMessages;
        }

        public void SendEvents()
        {
            // Create EventHubClient 
            EventHubClient client = EventHubClient.Create(this.eventHubName);

            try
            {
                List<Task> tasks = new List<Task>();
                // Send messages to Event Hub 
                Console.WriteLine("Sending messages to Event Hub {0}", client.Path);
                Random random = new Random();
                for (int i = 0; i < this.numberOfMessages; ++i)
                {
                    // Create the device/temperature metric                     
                    var deviceId = random.Next(numberOfDevices).ToString();
                    var message = "DeviceId: " + deviceId;
                    var serializedString = JsonConvert.SerializeObject(message);
                    EventData data = new EventData(Encoding.UTF8.GetBytes(serializedString))
                    {
                        PartitionKey = deviceId
                    };

                    // Set user properties if needed 
                    data.Properties.Add("Type", "Telemetry_" + DateTime.Now.ToLongTimeString());
                    Console.WriteLine("Sending: {0}", serializedString);

                    // Send the metric to Event Hub 
                    tasks.Add(client.SendAsync(data));
                }
                ;

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error on send: " + exp.Message);
            }

            client.CloseAsync().Wait();
        }
    }
}