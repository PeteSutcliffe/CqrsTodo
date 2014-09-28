using System.Web.Http;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace Todo.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IContainer container)
        {
            // Web API configuration and services
            config.Services.Replace(typeof(IHttpControllerActivator), new ServiceActivator(config, container));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
