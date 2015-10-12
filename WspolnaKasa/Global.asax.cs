using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WspolnaKasa
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "DefaultResources";
            DefaultModelBinder.ResourceClassKey = "DefaultResources";
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
