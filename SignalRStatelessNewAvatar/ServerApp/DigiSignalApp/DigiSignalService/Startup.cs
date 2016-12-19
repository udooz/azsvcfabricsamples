using System.Web.Http;
using Owin;
using Microsoft.AspNet.SignalR;

using Microsoft.Owin.Cors;
using Microsoft.ServiceBus;
using System;

namespace DigiSignalService
{
    public static class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);

            string connectionString = "Endpoint=sb://shksignal1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=0++up2RtkmYCP22Ywj9fVm1yG1yGjpdsQT30tXKA74s=";
            GlobalHost.DependencyResolver.UseServiceBus(connectionString, "shksignal");
            
            appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.Map("/echo", map =>
            {
                map.UseCors(CorsOptions.AllowAll).RunSignalR<DigiSignalConnection>();
            });
        }
    }
}
