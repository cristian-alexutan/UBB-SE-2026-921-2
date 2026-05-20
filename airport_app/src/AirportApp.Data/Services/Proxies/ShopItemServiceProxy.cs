using AirportApp.Data.Repositories.Proxies;

namespace AirportApp.Data.Services.Proxies;

public class ShopItemServiceProxy : RepositoryProxyBase, IShopItemService
{
    public ShopItemServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<ShopItem> GetAll()
    {
        return this.GetList<ShopItemDto>("api/shop-items")
            .Select(MapShopItem)
            .ToList();
    }

    public ShopItem GetById(int shopItemId)
    {
        return MapShopItem(this.GetRequired<ShopItemDto>($"api/shop-items/{shopItemId}"));
    }

    public IEnumerable<ShopItem> GetItemsByShopId(int shopId)
    {
        return this.GetList<ShopItemDto>($"api/shop-items/shop/{shopId}")
            .Select(MapShopItem)
            .ToList();
    }

    public IEnumerable<ShopItem> SearchItemsByName(int shopId, string searchText)
    {
        return this.GetList<ShopItemDto>($"api/shop-items/shop/{shopId}/search?searchText={Uri.EscapeDataString(searchText)}")
            .Select(MapShopItem)
            .ToList();
    }

    public IEnumerable<ShopItem> GetItemsSortedByPrice(Shop currentShop)
    {
        return this.GetList<ShopItemDto>($"api/shop-items/shop/{currentShop.Id}/sorted-by-price")
            .Select(MapShopItem)
            .ToList();
    }

    public IEnumerable<ShopItem> GetItemsSortedAlphabetically(Shop currentShop)
    {
        return this.GetList<ShopItemDto>($"api/shop-items/shop/{currentShop.Id}/sorted-alphabetically")
            .Select(MapShopItem)
            .ToList();
    }

    public void AddShopItem(ShopItem shopItem)
    {
        this.Post("api/shop-items", ToRequest(shopItem));
    }

    public void UpdateShopItem(ShopItem shopItem)
    {
        this.Put($"api/shop-items/{shopItem.Id}", ToRequest(shopItem));
    }

    public void RemoveShopItem(int shopItemId)
    {
        this.Delete($"api/shop-items/{shopItemId}");
    }

    private static ShopItemRequest ToRequest(ShopItem shopItem)
    {
        return new ShopItemRequest(
            shopItem.Id,
            shopItem.Quantity,
            shopItem.Price,
            shopItem.Shop?.Id ?? 0,
            shopItem.Photo,
            shopItem.Name,
            shopItem.Description);
    }

    private static ShopItem MapShopItem(ShopItemDto shopItem)
    {
        return new ShopItem(
            shopItem.Id,
            shopItem.Quantity,
            shopItem.Price,
            MapShop(shopItem.Shop),
            shopItem.Photo ?? string.Empty,
            shopItem.Name ?? string.Empty,
            shopItem.Description ?? string.Empty);
    }

    private static Shop MapShop(ShopDto? shop)
    {
        if (shop == null)
        {
            return new Shop(string.Empty, string.Empty, MapManager(null));
        }

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

    private sealed class ShopItemDto
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public ShopDto? Shop { get; set; }

        public string? Photo { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
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

    private sealed record ShopItemRequest(
        int Id,
        int Quantity,
        float Price,
        int ShopId,
        string Photo,
        string Name,
        string Description);
}
