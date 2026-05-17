namespace AirportApp.Data.Repositories.Proxies;

public class RunwayRepositoryProxy : RepositoryProxyBase, IRunwayRepository
{
    public RunwayRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Runway> GetAllRunways()
    {
        return this.GetList<RunwayDto>("api/runways")
            .Select(MapRunway)
            .ToList();
    }

    public Runway? GetRunwayById(int runwayId)
    {
        RunwayDto? dto = this.GetOptional<RunwayDto>($"api/runways/{runwayId}");
        return dto == null ? null : MapRunway(dto);
    }

    public int AddRunway(Runway newRunway)
    {
        RunwayDto result = this.PostForResult<Runway, RunwayDto>("api/runways", newRunway);
        return result.Id;
    }

    public void UpdateRunway(Runway updatedRunway)
    {
        this.Put($"api/runways/{updatedRunway.Id}", updatedRunway);
    }

    public void DeleteRunwayUsingId(int runwayId)
    {
        this.Delete($"api/runways/{runwayId}");
    }

    private static Runway MapRunway(RunwayDto dto)
    {
        return new Runway { Id = dto.Id, Name = dto.Name ?? string.Empty, HandleTime = dto.HandleTime };
    }

    private sealed class RunwayDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int HandleTime { get; set; }
    }
}
