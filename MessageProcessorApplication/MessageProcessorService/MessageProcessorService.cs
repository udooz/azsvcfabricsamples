using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace MessageProcessorService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class MessageProcessorService : StatelessService
    {
        public MessageProcessorService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var msgReceiver = new MessageReceiver("uzevents", "Endpoint=sb://uzehub.servicebus.windows.net/;SharedAccessKeyName=AllAccess;SharedAccessKey=Yf4t4XJQPD6fZn/xg36giRrAjPYr4xk8YnflbwEB+nk=");
            msgReceiver.MessageProcessingWithPartitionDistribution(this.Context);

            return new ServiceInstanceListener[0];
        }
        
    }
}
