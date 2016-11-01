using System;

namespace SignalRStatelessService
{
    using System.Web.Http;

    public static class RouteConfig
    {
        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "NotificationApi",
                routeTemplate: "api/{action}",
                defaults: new { controller = "Default" });
        }
    }
}
