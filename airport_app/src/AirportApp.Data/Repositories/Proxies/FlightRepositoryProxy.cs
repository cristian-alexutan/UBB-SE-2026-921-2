namespace AirportApp.Data.Repositories.Proxies;

public class FlightRepositoryProxy : RepositoryProxyBase, IFlightRepository
{
    public FlightRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Flight> GetAllFlights()
    {
        return this.GetList<FlightDto>("api/flights")
            .Select(MapFlight)
            .ToList();
    }

    public Flight? GetFlightById(int flightId)
    {
        FlightDto? dto = this.GetOptional<FlightDto>($"api/flights/{flightId}");
        return dto == null ? null : MapFlight(dto);
    }

    public List<Flight> GetFlightsByRouteId(int routeId)
    {
        return this.GetList<FlightDto>($"api/flights/by-route/{routeId}")
            .Select(MapFlight)
            .ToList();
    }

    public List<Flight> GetFlightsByRunwayId(int runwayId)
    {
        return this.GetList<FlightDto>($"api/flights/by-runway/{runwayId}")
            .Select(MapFlight)
            .ToList();
    }

    public List<Flight> GetFlightsByGateId(int gateId)
    {
        return this.GetList<FlightDto>($"api/flights/by-gate/{gateId}")
            .Select(MapFlight)
            .ToList();
    }

    public List<Flight> GetFlightsByAirportId(int airportId)
    {
        return this.GetList<FlightDto>($"api/flights/by-airport/{airportId}")
            .Select(MapFlight)
            .ToList();
    }

    public int AddFlight(Flight newFlight)
    {
        FlightDto result = this.PostForResult<FlightRequest, FlightDto>("api/flights", ToRequest(newFlight));
        return result.Id;
    }

    public void UpdateFlight(Flight updatedFlight)
    {
        this.Put($"api/flights/{updatedFlight.Id}", ToRequest(updatedFlight));
    }

    private static FlightRequest ToRequest(Flight flight)
    {
        return new FlightRequest(
            flight.Id,
            flight.Date,
            flight.FlightNumber,
            flight.Route?.Id ?? 0,
            flight.Runway?.Id ?? 0,
            flight.Gate?.Id ?? 0);
    }

    public void DeleteFlightUsingId(int flightId)
    {
        this.Delete($"api/flights/{flightId}");
    }

    private static Flight MapFlight(FlightDto dto)
    {
        return new Flight
        {
            Id = dto.Id,
            Date = dto.Date,
            FlightNumber = dto.FlightNumber ?? string.Empty,
            Route = MapRoute(dto.Route),
            Runway = MapRunway(dto.Runway),
            Gate = MapGate(dto.Gate)
        };
    }

    private static Route MapRoute(RouteDto? dto)
    {
        if (dto == null)
        {
            return new Route();
        }

        return new Route
        {
            Id = dto.Id,
            RouteType = dto.RouteType ?? string.Empty,
            RecurrenceInterval = dto.RecurrenceInterval,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            DepartureTime = dto.DepartureTime,
            ArrivalTime = dto.ArrivalTime,
            Capacity = dto.Capacity,
            Company = new Company { Id = dto.Company?.Id ?? 0, Name = dto.Company?.Name ?? string.Empty },
            Airport = new Airport { Id = dto.Airport?.Id ?? 0, Name = dto.Airport?.Name ?? string.Empty, Code = dto.Airport?.Code ?? string.Empty, City = dto.Airport?.City ?? string.Empty }
        };
    }

    private static Runway MapRunway(RunwayDto? dto)
    {
        if (dto == null)
        {
            return new Runway();
        }

        return new Runway { Id = dto.Id, Name = dto.Name ?? string.Empty, HandleTime = dto.HandleTime };
    }

    private static Gate MapGate(GateDto? dto)
    {
        if (dto == null)
        {
            return new Gate();
        }

        return new Gate { Id = dto.Id, Name = dto.Name ?? string.Empty };
    }

    private sealed class FlightDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string? FlightNumber { get; set; }

        public RouteDto? Route { get; set; }

        public RunwayDto? Runway { get; set; }

        public GateDto? Gate { get; set; }
    }

    private sealed class RouteDto
    {
        public int Id { get; set; }

        public string? RouteType { get; set; }

        public int RecurrenceInterval { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public TimeOnly DepartureTime { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        public int Capacity { get; set; }

        public CompanyDto? Company { get; set; }

        public AirportDto? Airport { get; set; }
    }

    private sealed class RunwayDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int HandleTime { get; set; }
    }

    private sealed class GateDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }

    private sealed class CompanyDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }

    private sealed class AirportDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? City { get; set; }
    }

    private sealed record FlightRequest(
        int Id,
        DateTime Date,
        string FlightNumber,
        int RouteId,
        int RunwayId,
        int GateId);
}
