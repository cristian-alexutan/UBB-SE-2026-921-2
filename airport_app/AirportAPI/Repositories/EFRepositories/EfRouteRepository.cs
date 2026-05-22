using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using Route = AirportLib.Domain.Domain.Route;

namespace AirportAPI.Repositories
{
    public class EfRouteRepository(AppDbContext databaseContext) : IRouteRepository
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
            newRoute.Id = 0;
            newRoute.Company = databaseContext.Companies.Find(newRoute.Company.Id)
                ?? throw new InvalidOperationException($"Company with id {newRoute.Company.Id} does not exist.");
            newRoute.Airport = databaseContext.Airports.Find(newRoute.Airport.Id)
                ?? throw new InvalidOperationException($"Airport with id {newRoute.Airport.Id} does not exist.");

            databaseContext.Routes.Add(newRoute);
            databaseContext.SaveChanges();

            return newRoute.Id;
        }

        public void UpdateRoute(Route routeToUpdate)
        {
            Route? existingRoute = databaseContext.Routes.Find(routeToUpdate.Id);
            if (existingRoute == null)
            {
                return;
            }

            Company company = databaseContext.Companies.Find(routeToUpdate.Company.Id)
                ?? throw new InvalidOperationException($"Company with id {routeToUpdate.Company.Id} does not exist.");
            Airport airport = databaseContext.Airports.Find(routeToUpdate.Airport.Id)
                ?? throw new InvalidOperationException($"Airport with id {routeToUpdate.Airport.Id} does not exist.");

            existingRoute.RouteType = routeToUpdate.RouteType;
            existingRoute.RecurrenceInterval = routeToUpdate.RecurrenceInterval;
            existingRoute.StartDate = routeToUpdate.StartDate;
            existingRoute.EndDate = routeToUpdate.EndDate;
            existingRoute.DepartureTime = routeToUpdate.DepartureTime;
            existingRoute.ArrivalTime = routeToUpdate.ArrivalTime;
            existingRoute.Capacity = routeToUpdate.Capacity;
            existingRoute.Company = company;
            existingRoute.Airport = airport;
            databaseContext.SaveChanges();
        }

        public void DeleteRoute(int routeId)
        {
            Route? routeToRemove = this.GetRouteById(routeId);

            if (routeToRemove == null)
            {
                return;
            }

            databaseContext.Routes.Remove(routeToRemove);
            databaseContext.SaveChanges();
        }
    }
}
