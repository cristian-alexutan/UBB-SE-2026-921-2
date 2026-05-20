using AirportApp.Data.Repositories.Proxies;

namespace AirportApp.Data.Services.Proxies;

public class ShopServiceProxy : RepositoryProxyBase, IShopService
{
    public ShopServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Shop> GetAllAvailableShops()
    {
        return this.GetList<ShopDto>("api/shops")
            .Select(MapShop)
            .ToList();
    }

    public void AddShop(Shop shop)
    {
        this.Post("api/shops", ToRequest(shop));
    }

    public void UpdateShop(Shop shop)
    {
        this.Put($"api/shops/{shop.Id}", ToRequest(shop));
    }

    public void DeleteShop(int shopId)
    {
        this.Delete($"api/shops/{shopId}");
    }

    public IEnumerable<Shop> SearchByName(string input)
    {
        return this.GetList<ShopDto>($"api/shops/search?input={Uri.EscapeDataString(input)}")
            .Select(MapShop)
            .ToList();
    }

    public IEnumerable<Shop> SortAlphabetically(IEnumerable<Shop> shops)
    {
        return this.GetList<ShopDto>("api/shops/sorted")
            .Select(MapShop)
            .ToList();
    }

    private static ShopRequest ToRequest(Shop shop)
    {
        return new ShopRequest(shop.Id, shop.Name, shop.Type, shop.Manager?.Id ?? 0);
    }

    private static Shop MapShop(ShopDto shop)
    {
        return new Shop(
            shop.Id,
            shop.Name ?? string.Empty,
            shop.Type ?? string.Empty,
            MapManager(shop.Manager));
    }

    private static Manager MapManager(ManagerDto? manager)
    {
        if (manager == null)
        {
            return new Manager(0, string.Empty, string.Empty, string.Empty);
        }

        return new Manager(
            manager.Id,
            manager.Name ?? string.Empty,
            manager.Email ?? string.Empty,
            manager.Phone ?? string.Empty);
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
