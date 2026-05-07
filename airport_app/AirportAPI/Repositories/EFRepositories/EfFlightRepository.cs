using AirportAPI;
using AirportAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfFlightRepository : IFlightRepository
    {
        private readonly AppDbContext context;

        public EfFlightRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Flight> GetAllFlights()
        {
            return context.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .ToList();
        }

        public Flight? GetFlightById(int flightId)
        {
            return context.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .FirstOrDefault(flight => flight.Id == flightId);
        }

        public List<Flight> GetFlightsByRouteId(int routeId)
        {
            return context.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .Where(flight => flight.Route.Id == routeId)
                .ToList();
        }

        public List<Flight> GetFlightsByRunwayId(int runwayId)
        {
            return context.Flights
                .Include(flight => flight.Route)
                .Include(flight => flight.Runway)
                .Include(flight => flight.Gate)
                .Where(flight => flight.Runway.Id == runwayId)
                .ToList();
        }

        public List<Flight> GetFlightsByGateId(int gateId)
        {
            return context.Flights
                .Where(flight => flight.Gate.Id == gateId)
                .ToList();
        }

        public List<Flight> GetFlightsByAirportId(int airportId)
        {
            return context.Flights
                .Include(flight => flight.Route)
                .Where(flight => flight.Route.Airport.Id == airportId)
                .ToList();
        }

        public int AddFlight(Flight flight)
        {
            context.Flights.Add(flight);
            context.SaveChanges();

            return flight.Id;
        }

        public void UpdateFlight(Flight flight)
        {
            context.Flights.Update(flight);
            context.SaveChanges();
        }

        public void DeleteFlightUsingId(int flightId)
        {
            Flight? flightToDelete = context.Flights
                .FirstOrDefault(flight => flight.Id == flightId);

            if (flightToDelete != null)
            {
                context.Flights.Remove(flightToDelete);
                context.SaveChanges();
            }
        }
    }
}


