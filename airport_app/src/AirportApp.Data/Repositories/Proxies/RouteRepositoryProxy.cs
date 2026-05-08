namespace AirportApp.Data.Repositories.Proxies;

public class RouteRepositoryProxy : RepositoryProxyBase, IRouteRepository
{
    public RouteRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Route> GetAllRoutes()
    {
        return this.GetList<RouteDto>("api/routes")
            .Select(MapRoute)
            .ToList();
    }

    public Route? GetRouteById(int routeId)
    {
        RouteDto? dto = this.GetOptional<RouteDto>($"api/routes/{routeId}");
        return dto == null ? null : MapRoute(dto);
    }

    public int AddRoute(Route newRoute)
    {
        return this.PostForResult<RouteRequest, int>("api/routes", ToRequest(newRoute));
    }

    public void UpdateRoute(Route updatedRoute)
    {
        this.Put($"api/routes/{updatedRoute.Id}", ToRequest(updatedRoute));
    }

    private static RouteRequest ToRequest(Route route)
    {
        return new RouteRequest(
            route.Id,
            route.RouteType,
            route.RecurrenceInterval,
            route.StartDate,
            route.EndDate,
            route.DepartureTime,
            route.ArrivalTime,
            route.Capacity,
            route.Company?.Id ?? 0,
            route.Airport?.Id ?? 0);
    }

    public void DeleteRoute(int routeId)
    {
        this.Delete($"api/routes/{routeId}");
    }

    private static Route MapRoute(RouteDto dto)
    {
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

    private sealed record RouteRequest(
        int Id,
        string RouteType,
        int RecurrenceInterval,
        DateOnly StartDate,
        DateOnly EndDate,
        TimeOnly DepartureTime,
        TimeOnly ArrivalTime,
        int Capacity,
        int CompanyId,
        int AirportId);
}
