using AirportLib.Services.Services.Interfaces;

namespace AirportLib.Services.Services.Proxies;

public class ShopItemServiceProxy : ServiceProxyBase, IShopItemService
{
    public ShopItemServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public IEnumerable<ShopItem> GetAll()
    {
        return this.GetList<ShopItem>("api/shop-items");
    }

    public ShopItem GetById(int shopItemId)
    {
        return this.GetRequired<ShopItem>($"api/shop-items/{shopItemId}");
    }

    public IEnumerable<ShopItem> GetItemsByShopId(int shopId)
    {
        return this.GetList<ShopItem>($"api/shop-items/shop/{shopId}");
    }

    public IEnumerable<ShopItem> SearchItemsByName(int shopId, string searchText)
    {
        return this.GetList<ShopItem>($"api/shop-items/shop/{shopId}/search?searchText={Uri.EscapeDataString(searchText)}");
    }

    public IEnumerable<ShopItem> GetItemsSortedByPrice(Shop currentShop)
    {
        return this.GetList<ShopItem>($"api/shop-items/shop/{currentShop.Id}/sorted-by-price");
    }

    public IEnumerable<ShopItem> GetItemsSortedAlphabetically(Shop currentShop)
    {
        return this.GetList<ShopItem>($"api/shop-items/shop/{currentShop.Id}/sorted-alphabetically");
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

    private sealed record ShopItemRequest(
        int Id,
        int Quantity,
        float Price,
        int ShopId,
        string Photo,
        string Name,
        string Description);
}
