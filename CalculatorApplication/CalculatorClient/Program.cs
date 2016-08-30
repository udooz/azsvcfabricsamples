using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorService;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using System.ServiceModel;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using CalcWcfService;

namespace CalculatorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculatorClient = ServiceProxy.Create<ICalculatorService>
                (new Uri("fabric:/CalculatorApplication/CalculatorService"));
            var result = calculatorClient.Add(4, 2).Result;
            Console.WriteLine("Result {0}", result);

            Console.WriteLine("======= WCF =========");
            Uri serviceUri = new Uri("fabric:/CalculatorApplication/CalcWcfService");
            var serviceResolver = ServicePartitionResolver.GetDefault();
            var binding = CreateClientConnectionBinding();
            var wcfFactory = new WcfCommunicationClientFactory<ICalcWcfService>(clientBinding: binding,
                servicePartitionResolver: serviceResolver);
            var wcfCalcClient = new WcfClient(wcfFactory, serviceUri);
            var wcfResult = wcfCalcClient.Add(3, 3);
            Console.WriteLine("WCF Result {0}", wcfResult.Result);

            //Console.ReadKey();
        }

        private static NetTcpBinding CreateClientConnectionBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.None)
            {
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.FromSeconds(5),
                CloseTimeout = TimeSpan.FromSeconds(5),
                MaxReceivedMessageSize = 1024 * 1024,
            };
            binding.MaxBufferSize = (int)binding.MaxReceivedMessageSize;
            binding.MaxBufferPoolSize = Environment.ProcessorCount * binding.MaxReceivedMessageSize;
            return binding;
        }
    }
}
