using AirportLib.Services.Services.Interfaces;

namespace AirportLib.Services.Services.Proxies;

public class FlightServiceProxy : ServiceProxyBase, IFlightService
{
    public FlightServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public List<Flight> GetAllFlights()
    {
        return this.GetList<Flight>("api/flights");
    }

    public Flight? GetFlightById(int flightId)
    {
        return this.GetOptional<Flight>($"api/flights/{flightId}");
    }

    public List<Flight> GetFlightsByRouteId(int routeId)
    {
        return this.GetList<Flight>($"api/flights/by-route/{routeId}");
    }

    public int AddFlight(string flightNumber, int routeId, DateTime date, int runwayId, int gateId)
    {
        var flight = new Flight
        {
            FlightNumber = flightNumber,
            Route = new Route { Id = routeId },
            Date = date,
            Runway = new Runway { Id = runwayId },
            Gate = new Gate { Id = gateId }
        };

        return this.PostForResult<Flight, int>("api/flights", flight);
    }

    public void UpdateFlight(int id, DateTime? date = null, string? flightNumber = null,
        int? runwayId = null, int? gateId = null)
    {
        var flight = new Flight
        {
            Id = id,
            Date = date ?? DateTime.MinValue,
            FlightNumber = flightNumber ?? string.Empty,
            Route = new Route { Id = 0 },
            Runway = new Runway { Id = runwayId ?? 0 },
            Gate = new Gate { Id = gateId ?? 0 }
        };

        this.Put($"api/flights/{id}", flight);
    }

    public void DeleteFlightUsingId(int flightId)
    {
        this.Delete($"api/flights/{flightId}");
    }
}