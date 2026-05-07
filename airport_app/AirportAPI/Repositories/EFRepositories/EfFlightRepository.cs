using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfFlightRepository(AppDbContext databaseContext) : IFlightRepository
    {
        public List<Flight> GetAllFlights()
        {
            return databaseContext.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .ToList();
        }

        public Flight? GetFlightById(int flightId)
        {
            return databaseContext.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .FirstOrDefault(flight => flight.Id == flightId);
        }

        public List<Flight> GetFlightsByRouteId(int routeId)
        {
            return databaseContext.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .Where(flight => flight.Route.Id == routeId)
                .ToList();
        }

        public List<Flight> GetFlightsByRunwayId(int runwayId)
        {
            return databaseContext.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .Where(flight => flight.Runway.Id == runwayId)
                .ToList();
        }

        public List<Flight> GetFlightsByGateId(int gateId)
        {
            return databaseContext.Flights
                .Where(flight => flight.Gate.Id == gateId)
                .ToList();
        }

        public List<Flight> GetFlightsByAirportId(int airportId)
        {
            return databaseContext.Flights
                .Include(flight => flight.Route)
                .Where(flight => flight.Route.Airport.Id == airportId)
                .ToList();
        }

        public int AddFlight(Flight flight)
        {
            databaseContext.Flights.Add(flight);
            databaseContext.SaveChanges();

            return flight.Id;
        }

        public void UpdateFlight(Flight flight)
        {
            databaseContext.Flights.Update(flight);
            databaseContext.SaveChanges();
        }

        public void DeleteFlightUsingId(int flightId)
        {
            Flight? flightToRemove = databaseContext.Flights
                .FirstOrDefault(flight => flight.Id == flightId);

            if (flightToRemove == null)
            {
                return;
            }

            databaseContext.Flights.Remove(flightToRemove);
            databaseContext.SaveChanges();
        }
    }
}