using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Fabric;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRStatelessService
{
    public class OwinCommunicationListener : ICommunicationListener
    {
        private readonly IowinAppBuilder startup;
        private IDisposable serviceHandle;
        private string listeningAddress;
        private ServiceContext context;
        private string endpointInfo;
        private string publishAddress;

        public OwinCommunicationListener(IowinAppBuilder startup, ServiceContext context, string endpointInfo)
        {
            this.startup = startup;
            this.context = context;
            this.endpointInfo = endpointInfo;
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
            var serviceEndpoint = this.context.CodePackageActivationContext.GetEndpoint(this.endpointInfo);
            var protocol = serviceEndpoint.Protocol;
            int port = serviceEndpoint.Port;
            
            //this.listeningAddress = $"http://+:{port}/";
            this.listeningAddress = string.Format(
            CultureInfo.InvariantCulture,
            "{0}://+:{1}/{2}",
            protocol,
            port,"");
            ServiceEventSource.Current.Message($"Listening on {listeningAddress}");

            //this.publishAddress = this.listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
            string resultAddress = string.Empty;

            try
            {
                this.serviceHandle = WebApp.Start(this.listeningAddress, appBuilder => this.startup.Configuration(appBuilder));
                resultAddress = listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
                ServiceEventSource.Current.Message($"Listening on {resultAddress}");
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message("Web server failed to open endpoint {0}. {1}", this.endpointInfo, ex.ToString());

                this.StopWebServer();
            }
            
            return Task.FromResult(resultAddress);;
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
