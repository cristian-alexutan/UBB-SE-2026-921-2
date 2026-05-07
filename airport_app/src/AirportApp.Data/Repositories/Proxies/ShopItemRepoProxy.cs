namespace AirportApp.Data.Repositories.Proxies;

public class ShopItemRepoProxy : RepositoryProxyBase, IShopItemRepo
{
    public ShopItemRepoProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<ShopItem> GetAll()
    {
        return this.GetList<ShopItemDto>("api/shop-items")
            .Select(MapShopItem)
            .ToList();
    }

    public ShopItem? GetById(int shopItemId)
    {
        ShopItemDto? dto = this.GetOptional<ShopItemDto>($"api/shop-items/{shopItemId}");
        return dto == null ? null : MapShopItem(dto);
    }

    public void Add(ShopItem shopItem)
    {
        this.Post("api/shop-items", shopItem);
    }

    public void Delete(int shopItemId)
    {
        this.Delete($"api/shop-items/{shopItemId}");
    }

    public void Update(ShopItem shopItem)
    {
        this.Put("api/shop-items", shopItem);
    }

    private static ShopItem MapShopItem(ShopItemDto dto)
    {
        return new ShopItem(dto.Id, dto.Quantity, dto.Price, MapShop(dto.Shop), dto.Photo ?? string.Empty, dto.Name ?? string.Empty, dto.Description ?? string.Empty);
    }

    private static Shop MapShop(ShopDto? dto)
    {
        if (dto == null)
        {
            return new Shop(string.Empty, string.Empty, MapManager(null));
        }

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
}
