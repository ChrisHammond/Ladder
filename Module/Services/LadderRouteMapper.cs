using DotNetNuke.Web.Services;

namespace Christoc.Com.Modules.Ladder.services
{
    public class LadderRouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapRoute("Ladder", "{controller}.ashx/{action}",
                                     new[] { "Christoc.Com.Modules.Ladder.services" });
        }
    }
}