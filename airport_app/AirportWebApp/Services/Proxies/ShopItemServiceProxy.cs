using AirportWebApp.Services.Proxies;

namespace AirportWebApp.Services.Proxies;

public class ShopItemServiceProxy : RepositoryProxyBase, IShopItemService
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
        this.Post("api/shop-items", shopItem);
    }

    public void UpdateShopItem(ShopItem shopItem)
    {
        this.Put($"api/shop-items/{shopItem.Id}", shopItem);
    }

    public void RemoveShopItem(int shopItemId)
    {
        this.Delete($"api/shop-items/{shopItemId}");
    }
}
