using AirportWebApp.Services.Proxies;

namespace AirportWebApp.Services.Proxies;

public class GateServiceProxy : RepositoryProxyBase, IGateService
{
    public GateServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public List<Gate> GetAllGates()
    {
        return this.GetList<Gate>("api/gates");
    }

    public Gate? GetGateById(int gateId)
    {
        return this.GetOptional<Gate>($"api/gates/{gateId}");
    }

    public int AddGate(string name)
    {
        return this.PostForResult<string, int>("api/gates", name);
    }

    public void UpdateGate(int gateId, string? newName = null)
    {
        this.Put($"api/gates/{gateId}", newName);
    }

    public void DeleteGateUsingId(int gateId)
    {
        this.Delete($"api/gates/{gateId}");
    }

    public void SaveGate(int gateId, string name)
    {
        if (gateId == 0)
        {
            this.AddGate(name);
        }
        else
        {
            this.UpdateGate(gateId, name);
        }
    }

    public bool HasFlights(int gateId)
    {
        return this.GetRequired<bool>($"api/gates/{gateId}/has-flights");
    }

    public string GetDeleteWarningMessage(int gateId)
    {
        DeleteWarningDto? deleteWarningDto = this.GetOptional<DeleteWarningDto>($"api/gates/{gateId}/delete-warning");
        return deleteWarningDto?.Message ?? string.Empty;
    }

    private sealed class DeleteWarningDto
    {
        public string? Message { get; set; }
    }
}