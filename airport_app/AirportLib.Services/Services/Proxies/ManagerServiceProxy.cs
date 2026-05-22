using System.Net;
using System.Net.Http.Json;

using AirportLib.Services.Services.Interfaces;

namespace AirportLib.Services.Services.Proxies;

public class ManagerServiceProxy : ServiceProxyBase, IManagerService
{
    public ManagerServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public void AddManager(Manager manager)
    {
        this.Post("api/managers", manager);
    }

    public Manager? DeleteManager(int managerId)
    {
        ManagerDto? manager = this.DeleteForResult<ManagerDto>($"api/managers/{managerId}");
        return manager == null ? null : MapManager(manager);
    }

    public IEnumerable<Manager> GetAllManagers()
    {
        return this.GetList<ManagerDto>("api/managers")
            .Select(MapManager)
            .ToList();
    }

    public Manager GetAnyManager()
    {
        IEnumerable<Manager> managers = this.GetAllManagers();
        Manager? firstManager = null;

        foreach (Manager manager in managers)
        {
            firstManager = manager;
            break;
        }

        if (firstManager == null)
        {
            throw new InvalidOperationException("No managers are currently registered in the system.");
        }

        return firstManager;
    }

    public Manager GetManagerById(int managerId)
    {
        Manager? manager = this.GetManagerByIdOptional(managerId);
        return manager ?? throw new InvalidOperationException("Manager not found.");
    }

    public Manager? UpdateManager(Manager manager)
    {
        ManagerDto? updatedManager = this.PostUpdate(manager);
        return updatedManager == null ? null : MapManager(updatedManager);
    }

    private Manager? GetManagerByIdOptional(int managerId)
    {
        ManagerDto? manager = this.GetOptional<ManagerDto>($"api/managers/{managerId}");
        return manager == null ? null : MapManager(manager);
    }

    private ManagerDto? PostUpdate(Manager manager)
    {
        using HttpResponseMessage response = this.HttpClient.PutAsJsonAsync($"api/managers/{manager.Id}", manager).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<ManagerDto>(JsonOptions).GetAwaiter().GetResult();
    }

    private static Manager MapManager(ManagerDto manager)
    {
        return new Manager(manager.Id, manager.Name ?? string.Empty, manager.Email ?? string.Empty, manager.Phone ?? string.Empty);
    }

    private sealed class ManagerDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
