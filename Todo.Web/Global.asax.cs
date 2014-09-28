using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StructureMap;

namespace Todo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IContainer _container = new Container();

        protected void Application_Start()
        {
            Bootstrapper.Bootstrap(_container);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(config => WebApiConfig.Register(config, _container));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            

            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;            
        }
    }
}
