using CalcWcfService;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Threading.Tasks;

namespace CalculatorClient
{
    public class WcfClient : ServicePartitionClient<WcfCommunicationClient<ICalcWcfService>>, ICalcWcfService
    {
        public WcfClient(WcfCommunicationClientFactory<ICalcWcfService> clientFactory, Uri serviceName)
            : base(clientFactory, serviceName, ServicePartitionKey.Singleton)
        { }

        public Task<string> Add(int a, int b)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.Add(a, b));
        }

        public Task<string> Substract(int a, int b)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.Substract(a, b));
        }
    }
}
