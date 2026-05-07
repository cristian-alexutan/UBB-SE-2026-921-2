using System.Net;
using System.Net.Http.Json;

namespace AirportApp.Data.Repositories.Proxies;

public class ManagerRepoProxy : RepositoryProxyBase, IManagerRepo
{
    public ManagerRepoProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Manager> GetAll()
    {
        return this.GetList<ManagerDto>("api/managers")
            .Select(MapManager)
            .ToList();
    }

    public Manager? GetById(int managerId)
    {
        ManagerDto? dto = this.GetOptional<ManagerDto>($"api/managers/{managerId}");
        return dto == null ? null : MapManager(dto);
    }

    public void Add(Manager manager)
    {
        this.Post("api/managers", manager);
    }

    public Manager? Delete(int managerId)
    {
        ManagerDto? dto = this.DeleteForResult<ManagerDto>($"api/managers/{managerId}");
        return dto == null ? null : MapManager(dto);
    }

    public Manager? Update(Manager manager)
    {
        using HttpResponseMessage response = this.HttpClient.PutAsJsonAsync($"api/managers/{manager.Id}", manager, JsonOptions).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        ManagerDto? dto = response.Content.ReadFromJsonAsync<ManagerDto>(JsonOptions).GetAwaiter().GetResult();
        return dto == null ? null : MapManager(dto);
    }

    private static Manager MapManager(ManagerDto dto)
    {
        return new Manager(dto.Id, dto.Name ?? string.Empty, dto.Email ?? string.Empty, dto.Phone ?? string.Empty);
    }

    private sealed class ManagerDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
