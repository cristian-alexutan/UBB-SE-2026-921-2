namespace AirportAPI.Repositories.Interfaces;

using Route = AirportAPI.Domain.Route;

public interface IRouteRepository
{
    List<Route> GetAllRoutes();
    int AddRoute(Route newRoute);
    void DeleteRoute(int routeId);
    void UpdateRoute(Route updatedRoute);
    Route? GetRouteById(int routeId);
}



