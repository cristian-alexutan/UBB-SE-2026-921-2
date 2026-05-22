using System;
using System.Collections.Generic;
using System.Text;

namespace AirportWebApp.Services.Proxies;

using System;
using System.Collections.Generic;
using System.Text;

using AirportWebApp.Services.Proxies;

public class ShopServiceProxy : ServiceProxyBase, IShopService
{
    public ShopServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public IEnumerable<Shop> GetAllAvailableShops()
    {
        return GetList<Shop>("api/shops");
    }

    public Shop? GetShopById(int shopId)
    {
        return GetOptional<Shop>($"api/shops/{shopId}");
    }

    public void AddShop(Shop shop)
    {
        this.Post("api/shops", shop);
    }

    public void UpdateShop(Shop shop)
    {
        this.Put($"api/shops/{shop.Id}", shop);
    }

    public void DeleteShop(int shopId)
    {
        Delete($"api/shops/{shopId}");
    }

    public IEnumerable<Shop> SearchByName(string input)
    {
        return GetList<Shop>($"api/shops/search?input={Uri.EscapeDataString(input)}");
    }

    public IEnumerable<Shop> SortAlphabetically(IEnumerable<Shop> shops)
    {
        return GetList<Shop>("api/shops/sorted");
    }
}
