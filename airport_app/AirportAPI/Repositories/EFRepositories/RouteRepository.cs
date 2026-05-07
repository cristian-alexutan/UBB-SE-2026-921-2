using AirportAPI;
using AirportAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Route = AirportAPI.Domain.Route;

namespace AirportAPI.Repositories
{
    public class RouteRepository(AppDbContext databaseContext) : IRouteRepository
    {
        public List<Route> GetAllRoutes()
        {
            return databaseContext.Routes
                .Include(route => route.Company)
                .Include(route => route.Airport)
                .ToList();
        }

        public Route? GetRouteById(int routeId)
        {
            return databaseContext.Routes
                .Include(route => route.Company)
                .Include(route => route.Airport)
                .FirstOrDefault(route => route.Id == routeId);
        }

        public int AddRoute(Route newRoute)
        {
            databaseContext.Routes.Add(newRoute);
            databaseContext.SaveChanges();
            return newRoute.Id;
        }

        public void UpdateRoute(Route routeToUpdate)
        {
            databaseContext.Routes.Update(routeToUpdate);
            databaseContext.SaveChanges();
        }

        public void DeleteRoute(int routeId)
        {
            Route? routeToRemove = this.GetRouteById(routeId);

            if (routeToRemove != null)
            {
                databaseContext.Routes.Remove(routeToRemove);
                databaseContext.SaveChanges();
            }
        }
    }
}


