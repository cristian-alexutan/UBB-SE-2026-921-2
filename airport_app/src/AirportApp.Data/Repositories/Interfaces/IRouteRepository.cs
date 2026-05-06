namespace AirportApp.Data.Repositories.Interfaces;

public interface IRouteRepository
{
    List<Route> GetAllRoutes();
    int AddRoute(Route newRoute);
    void DeleteRoute(int routeId);
    void UpdateRoute(Route updatedRoute);
    Route? GetRouteById(int routeId);
}
