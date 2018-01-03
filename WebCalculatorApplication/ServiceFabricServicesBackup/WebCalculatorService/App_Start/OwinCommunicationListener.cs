using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace WebCalculatorService
{
    public class OwinCommunicationListener : ICommunicationListener
    {
        private readonly IowinAppBuilder startup;
        private IDisposable serviceHandle;
        private string listeningAddress;
        private ServiceContext context;

        public OwinCommunicationListener(IowinAppBuilder startup, ServiceContext context)
        {
            this.startup = startup;
            this.context = context;
        }
        public void Abort()
        {
            this.StopWebServer();
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            this.StopWebServer();
            return Task.FromResult(true);
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var serviceEndpoint = this.context.CodePackageActivationContext.GetEndpoint("ServiceEndPoint");
            int port = serviceEndpoint.Port;
            this.listeningAddress = $"http://+:{port}/";
            this.serviceHandle = WebApp.Start(this.listeningAddress,
                appBuilder => this.startup.Configuration(appBuilder));
            var resultAddress = listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
            ServiceEventSource.Current.Message($"Listening on {resultAddress}");
            return Task.FromResult(resultAddress);
        }

        private void StopWebServer()
        {
            if(this.serviceHandle != null)
            {
                try
                {
                    this.serviceHandle.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    
                }
            }
        }
    }
}
