namespace AirportApp.Data.Repositories.Proxies;

public class GateRepositoryProxy : RepositoryProxyBase, IGateRepository
{
    public GateRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Gate> GetAllGates()
    {
        return this.GetList<GateDto>("api/gates")
            .Select(MapGate)
            .ToList();
    }

    public Gate? GetGateById(int gateId)
    {
        GateDto? dto = this.GetOptional<GateDto>($"api/gates/{gateId}");
        return dto == null ? null : MapGate(dto);
    }

    public int AddGate(Gate newGate)
    {
        GateDto result = this.PostForResult<Gate, GateDto>("api/gates", newGate);
        return result.Id;
    }

    public void DeleteGateUsingId(int gateId)
    {
        this.Delete($"api/gates/{gateId}");
    }

    public void UpdateGate(Gate updatedGate)
    {
        this.Put($"api/gates/{updatedGate.Id}", updatedGate);
    }

    private static Gate MapGate(GateDto dto)
    {
        return new Gate { Id = dto.Id, Name = dto.Name ?? string.Empty };
    }

    private sealed class GateDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
