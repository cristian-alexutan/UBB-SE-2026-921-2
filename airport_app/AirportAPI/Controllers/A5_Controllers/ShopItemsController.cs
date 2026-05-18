using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/shop-items")]
public class ShopItemsController(IShopItemService shopItemService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<ShopItem>> GetAll()
    {
        return this.Ok(shopItemService.GetAll());
    }

    [HttpGet("{shopItemId:int}")]
    public ActionResult<ShopItem> GetById(int shopItemId)
    {
        ShopItem shopItem = shopItemService.GetById(shopItemId);

        return this.Ok(shopItem);
    }

    [HttpGet("shop/{shopId:int}")]
    public ActionResult<IEnumerable<ShopItem>> GetByShopId(int shopId)
    {
        return this.Ok(shopItemService.GetItemsByShopId(shopId));
    }

    [HttpGet("shop/{shopId:int}/search")]
    public ActionResult<IEnumerable<ShopItem>> Search(int shopId, [FromQuery] string searchText = "")
    {
        return this.Ok(shopItemService.SearchItemsByName(shopId, searchText));
    }

    [HttpGet("shop/{shopId:int}/sorted-by-price")]
    public ActionResult<IEnumerable<ShopItem>> GetSortedByPrice(int shopId)
    {
        Shop shop = new Shop { Id = shopId };

        return this.Ok(shopItemService.GetItemsSortedByPrice(shop));
    }

    [HttpGet("shop/{shopId:int}/sorted-alphabetically")]
    public ActionResult<IEnumerable<ShopItem>> GetSortedAlphabetically(int shopId)
    {
        Shop shop = new Shop { Id = shopId };

        return this.Ok(shopItemService.GetItemsSortedAlphabetically(shop));
    }

    [HttpPost]
    public ActionResult Add([FromBody] ShopItem shopItem)
    {
        shopItemService.AddShopItem(shopItem);

        return this.Ok();
    }

    [HttpPut("{shopItemId:int}")]
    public ActionResult Update(int shopItemId, [FromBody] ShopItem shopItem)
    {
        shopItem.Id = shopItemId;

        shopItemService.UpdateShopItem(shopItem);

        return this.NoContent();
    }

    [HttpDelete("{shopItemId:int}")]
    public ActionResult Delete(int shopItemId)
    {
        shopItemService.RemoveShopItem(shopItemId);

        return NoContent();
    }
}
