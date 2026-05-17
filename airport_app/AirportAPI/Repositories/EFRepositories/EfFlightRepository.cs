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
            flight.Id = 0;
            flight.Route = databaseContext.Routes.Find(flight.Route.Id)
                ?? throw new InvalidOperationException($"Route with id {flight.Route.Id} does not exist.");
            flight.Runway = databaseContext.Runways.Find(flight.Runway.Id)
                ?? throw new InvalidOperationException($"Runway with id {flight.Runway.Id} does not exist.");
            flight.Gate = databaseContext.Gates.Find(flight.Gate.Id)
                ?? throw new InvalidOperationException($"Gate with id {flight.Gate.Id} does not exist.");

            databaseContext.Flights.Add(flight);
            databaseContext.SaveChanges();

            return flight.Id;
        }

        public void UpdateFlight(Flight flight)
        {
            Flight? existingFlight = databaseContext.Flights.Find(flight.Id);
            if (existingFlight == null)
            {
                return;
            }

            AirportAPI.Domain.Route route = databaseContext.Routes.Find(flight.Route.Id)
                ?? throw new InvalidOperationException($"Route with id {flight.Route.Id} does not exist.");
            Runway runway = databaseContext.Runways.Find(flight.Runway.Id)
                ?? throw new InvalidOperationException($"Runway with id {flight.Runway.Id} does not exist.");
            Gate gate = databaseContext.Gates.Find(flight.Gate.Id)
                ?? throw new InvalidOperationException($"Gate with id {flight.Gate.Id} does not exist.");

            existingFlight.Date = flight.Date;
            existingFlight.FlightNumber = flight.FlightNumber;
            existingFlight.Route = route;
            existingFlight.Runway = runway;
            existingFlight.Gate = gate;
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
