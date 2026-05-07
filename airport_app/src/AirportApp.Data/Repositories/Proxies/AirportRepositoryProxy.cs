namespace AirportApp.Data.Repositories.Proxies;

public class AirportRepositoryProxy : RepositoryProxyBase, IAirportRepository
{
    public AirportRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Airport> GetAllAirports()
    {
        return this.GetList<AirportDto>("api/airports")
            .Select(MapAirport)
            .ToList();
    }

    public Airport? GetAirportById(int airportId)
    {
        AirportDto? airport = this.GetOptional<AirportDto>($"api/airports/{airportId}");
        return airport == null ? null : MapAirport(airport);
    }

    public int AddAirport(Airport newAirport)
    {
        AirportDto airport = this.PostForResult<Airport, AirportDto>("api/airports", newAirport);
        return airport.Id;
    }

    public void DeleteAirportUsingId(int airportId)
    {
        this.Delete($"api/airports/{airportId}");
    }

    public void UpdateAirport(Airport updatedAirport)
    {
        this.Put($"api/airports/{updatedAirport.Id}", updatedAirport);
    }

    private static Airport MapAirport(AirportDto airport)
    {
        return new Airport
        {
            Id = airport.Id,
            Code = airport.Code ?? string.Empty,
            Name = airport.Name ?? string.Empty,
            City = airport.City ?? string.Empty
        };
    }

    private sealed class AirportDto
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? City { get; set; }
    }
}
