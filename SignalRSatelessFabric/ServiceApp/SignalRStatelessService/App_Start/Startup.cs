using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.SignalR;
using SignalRStatelessService.SignalR;
using Microsoft.Owin.Cors;

namespace SignalRStatelessService
{
    public class Startup : IowinAppBuilder
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            FormatterConfig.ConfigureFormatters(config.Formatters);
            RouteConfig.RegisterRoutes(config.Routes);
            appBuilder.UseWebApi(config);

            string connectionString = "Endpoint=sb://shksignal1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=0++up2RtkmYCP22Ywj9fVm1yG1yGjpdsQT30tXKA74s=";
            GlobalHost.DependencyResolver.UseServiceBus(connectionString, "signalrr");
            //appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.MapSignalR<MyEndPoint>("/echo");
            //appBuilder.MapSignalR<MyEndPoint>("/echo", new ConnectionConfiguration
            //{
            //    EnableJSONP = false
            //}).RunSignalR();
        }
    }
}
