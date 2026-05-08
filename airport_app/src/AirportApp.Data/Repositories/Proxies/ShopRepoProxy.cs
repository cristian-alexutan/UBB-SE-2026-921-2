using System.Net;
using System.Net.Http.Json;

namespace AirportApp.Data.Repositories.Proxies;

public class ShopRepoProxy : RepositoryProxyBase, IShopRepository
{
    public ShopRepoProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Shop> GetAll()
    {
        return this.GetList<ShopDto>("api/shop")
            .Select(MapShop)
            .ToList();
    }

    public Shop? GetById(int shopId)
    {
        ShopDto? dto = this.GetOptional<ShopDto>($"api/shop/{shopId}");
        return dto == null ? null : MapShop(dto);
    }

    public void Add(Shop shop)
    {
        this.Post("api/shop", ToRequest(shop));
    }

    public Shop? Delete(int shopId)
    {
        ShopDto? dto = this.DeleteForResult<ShopDto>($"api/shop/{shopId}");
        return dto == null ? null : MapShop(dto);
    }

    public Shop? Update(Shop shop)
    {
        using HttpResponseMessage response = this.HttpClient.PutAsJsonAsync("api/shop", ToRequest(shop), JsonOptions).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        ShopDto? dto = response.Content.ReadFromJsonAsync<ShopDto>(JsonOptions).GetAwaiter().GetResult();
        return dto == null ? null : MapShop(dto);
    }

    private static ShopRequest ToRequest(Shop shop)
    {
        return new ShopRequest(shop.Id, shop.Name, shop.Type, shop.Manager?.Id ?? 0);
    }

    private static Shop MapShop(ShopDto dto)
    {
        return new Shop(dto.Id, dto.Name ?? string.Empty, dto.Type ?? string.Empty, MapManager(dto.Manager));
    }

    private static Manager MapManager(ManagerDto? dto)
    {
        if (dto == null)
        {
            return new Manager(0, string.Empty, string.Empty, string.Empty);
        }

        return new Manager(dto.Id, dto.Name ?? string.Empty, dto.Email ?? string.Empty, dto.Phone ?? string.Empty);
    }

    private sealed class ShopDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public ManagerDto? Manager { get; set; }
    }

    private sealed class ManagerDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }

    private sealed record ShopRequest(int Id, string Name, string Type, int ManagerId);
}
