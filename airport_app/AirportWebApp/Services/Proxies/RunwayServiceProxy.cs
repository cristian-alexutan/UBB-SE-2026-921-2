using AirportWebApp.Services.Proxies;

namespace AirportWebApp.Services.Proxies;

public class RunwayServiceProxy : RepositoryProxyBase, IRunwayService
{
    public RunwayServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public List<Runway> GetAllRunways()
    {
        return this.GetList<Runway>("api/runways");
    }

    public Runway? GetRunwayById(int runwayId)
    {
        return this.GetOptional<Runway>($"api/runways/{runwayId}");
    }

    public int AddRunway(string name, int handleTime)
    {
        return this.PostForResult<RunwayDto, int>("api/runways", new RunwayDto { Name = name, HandleTime = handleTime });
    }

    public void UpdateRunway(int runwayId, string? newName = null, int? newHandleTime = null)
    {
        this.Put($"api/runways/{runwayId}", new RunwayDto { Name = newName, HandleTime = newHandleTime });
    }

    public void DeleteRunwayUsingId(int runwayId)
    {
        this.Delete($"api/runways/{runwayId}");
    }

    public void SaveRunway(int runwayId, string runwayName, string handleTimeText)
    {
        if (runwayId == 0)
        {
            this.AddRunway(runwayName, int.Parse(handleTimeText));
        }
        else
        {
            this.UpdateRunway(runwayId, runwayName, int.TryParse(handleTimeText, out int handleTime) ? handleTime : null);
        }
    }

    public bool HasFlights(int runwayId)
    {
        return this.GetRequired<bool>($"api/runways/{runwayId}/has-flights");
    }

    public string GetDeleteWarningMessage(int runwayId)
    {
        DeleteWarningDto? deleteWarningDto = this.GetOptional<DeleteWarningDto>($"api/runways/{runwayId}/delete-warning");
        return deleteWarningDto?.Message ?? string.Empty;
    }

    private sealed class RunwayDto
    {
        public string? Name { get; set; }

        public int? HandleTime { get; set; }
    }

    private sealed class DeleteWarningDto
    {
        public string? Message { get; set; }
    }
}
