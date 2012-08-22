using System.Linq;
using System.Web.Mvc;
using DotNetNuke.Web.Services;
using Microsoft.Web.Mvc;

namespace Christoc.Com.Modules.Ladder.services
{
    public class LadderRouteMapper : IServiceRouteMapper
    {
        private JsonValueProviderFactory _existingFactory;

        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapRoute("Ladder", "{controller}.ashx/{action}",
                                     new[] { "Christoc.Com.Modules.Ladder.services" });
            //Remove and JsonValueProviderFactory and add JsonDotNetValueProviderFactory
            if (ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault() == null)
            {
                ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
            }

        }
    }
}