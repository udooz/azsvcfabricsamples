using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using SignalRStatelessService.SignalR;

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

            string connectionString = "Endpoint=sb://lbassignalr.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=piW0e6ugFBdZXXiJ/zZX6GSeSyVt8IiNRlpJxDMYwEg=";
            GlobalHost.DependencyResolver.UseServiceBus(connectionString, "signalrr");
            appBuilder.MapSignalR<MyEndPoint>("/echo");

            //appBuilder.MapSignalR();


        }
    }
}
