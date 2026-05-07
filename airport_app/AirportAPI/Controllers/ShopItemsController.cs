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
        public ActionResult Add([FromBody] ShopItem newShopItem)
        {
            if (newShopItem == null)
            {
                return this.BadRequest(NullShopItemDataErrorMessage);
            }

            shopItemRepository.Add(newShopItem);

            return this.CreatedAtAction(nameof(this.GetById), new { shopItemId = newShopItem.Id }, newShopItem);
        }

        [HttpPut("{shopItemId:int}")]
        public IActionResult Update(int shopItemId, [FromBody] ShopItem shopItemToUpdate)
        {
            if (shopItemToUpdate == null)
            {
                return this.BadRequest(NullShopItemDataErrorMessage);
            }

            if (shopItemRepository.GetById(shopItemId) == null)
            {
                return this.NotFound();
            }

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
    }
}