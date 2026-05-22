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
        Shop shop = new Shop(shopId, null!, null!, null!);

        return this.Ok(shopItemService.GetItemsSortedByPrice(shop));
    }

    [HttpGet("shop/{shopId:int}/sorted-alphabetically")]
    public ActionResult<IEnumerable<ShopItem>> GetSortedAlphabetically(int shopId)
    {
        Shop shop = new Shop(shopId, null!, null!, null!);

        return this.Ok(shopItemService.GetItemsSortedAlphabetically(shop));
    }

    [HttpPost]
    public ActionResult Add([FromBody] ShopItemRequest shopItemRequest)
    {
        shopItemService.AddShopItem(ToShopItem(shopItemRequest));

        return this.Ok();
    }

    [HttpPut("{shopItemId:int}")]
    public ActionResult Update(int shopItemId, [FromBody] ShopItemRequest shopItemRequest)
    {
        ShopItem shopItem = ToShopItem(shopItemRequest);
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

    private static ShopItem ToShopItem(ShopItemRequest request)
    {
        return new ShopItem
        {
            Id = request.Id,
            Quantity = request.Quantity,
            Price = request.Price,
            Shop = new Shop(request.ShopId, null!, null!, null!),
            Photo = request.Photo,
            Name = request.Name,
            Description = request.Description
        };
    }

    public sealed record ShopItemRequest(
        int Id,
        int Quantity,
        float Price,
        int ShopId,
        string Photo,
        string Name,
        string Description);
}
