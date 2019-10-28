using System.Data.Services;
using System.ServiceModel.Activation;
using System.Web.Routing;
using NuGet.Server;
using NuGet.Server.DataServices;
using NuGet.Server.Publishing;
using RouteMagic;
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HY.NuGetServer.NuGetRoutes), "Start")]

namespace HY.NuGetServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class NuGetRoutes
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            ServiceResolver.SetServiceResolver(new DefaultServiceResolver());
            MapRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        private static void MapRoutes(RouteCollection routes)
        {
            // Route to create a new package(http://{root}/nuget)
            routes.MapDelegate("CreatePackageNuGet",
                               "nuget",
                               new { httpMethod = new HttpMethodConstraint("PUT") },
                               context => CreatePackageService().CreatePackage(context.HttpContext));

            // The default route is http://{root}/nuget/Packages
            var factory = new DataServiceHostFactory();
            var serviceRoute = new ServiceRoute("nuget", factory, typeof(Packages));
            serviceRoute.Defaults = new RouteValueDictionary { { "serviceType", "odata" } };
            serviceRoute.Constraints = new RouteValueDictionary { { "serviceType", "odata" } };
            routes.Add("nuget", serviceRoute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IPackageService CreatePackageService()
        {
            return ServiceResolver.Resolve<IPackageService>();
        }
    }
}