using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers
{
    [ApiController]
    // explicit string to force hyphen
    [Route("api/shop-items")]
    public class ShopItemsController(IShopItemRepository shopItemRepository) : ControllerBase
    {
        private const string NullShopItemDataErrorMessage = "Shop item data cannot be null.";

        [HttpGet]
        public ActionResult<IEnumerable<ShopItem>> GetAll()
        {
            return this.Ok(shopItemRepository.GetAll());
        }

        [HttpGet("{shopItemId:int}")]
        public ActionResult<ShopItem> GetById(int shopItemId)
        {
            ShopItem? shopItem = shopItemRepository.GetById(shopItemId);

            if (shopItem == null)
            {
                return this.NotFound();
            }

            return this.Ok(shopItem);
        }

        [HttpPost]
        public ActionResult Add([FromBody] ShopItemRequest request)
        {
            if (request == null)
            {
                return this.BadRequest(NullShopItemDataErrorMessage);
            }

            ShopItem newShopItem = MapShopItem(request);
            shopItemRepository.Add(newShopItem);

            return this.CreatedAtAction(nameof(this.GetById), new { shopItemId = newShopItem.Id }, newShopItem);
        }

        [HttpPut("{shopItemId:int}")]
        public IActionResult Update(int shopItemId, [FromBody] ShopItemRequest request)
        {
            if (request == null)
            {
                return this.BadRequest(NullShopItemDataErrorMessage);
            }

            if (shopItemRepository.GetById(shopItemId) == null)
            {
                return this.NotFound();
            }

            ShopItem shopItemToUpdate = MapShopItem(request);
            shopItemToUpdate.Id = shopItemId;

            shopItemRepository.Update(shopItemToUpdate);

            return this.NoContent();
        }

        [HttpDelete("{shopItemId:int}")]
        public IActionResult Delete(int shopItemId)
        {
            if (shopItemRepository.GetById(shopItemId) == null)
            {
                return this.NotFound();
            }

            shopItemRepository.Delete(shopItemId);

            return this.NoContent();
        }

        private static ShopItem MapShopItem(ShopItemRequest request)
        {
            return new ShopItem
            {
                Id = request.Id,
                Quantity = request.Quantity,
                Price = request.Price,
                Shop = new Shop { Id = request.ShopId },
                Photo = request.Photo,
                Name = request.Name,
                Description = request.Description
            };
        }
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
