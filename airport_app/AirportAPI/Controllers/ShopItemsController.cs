using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/shop-items")]
    public class ShopItemsController(IShopItemRepo shopItemRepository) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ShopItem>> GetAll()
        {
            return this.Ok(shopItemRepository.GetAll());
        }

        [HttpGet("{shopItemId}")]
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
                return this.BadRequest("Shop item data is required.");
            }

            shopItemRepository.Add(newShopItem);
            return this.Ok();
        }

        [HttpPut]
        public ActionResult Update([FromBody] ShopItem shopItemToUpdate)
        {
            shopItemRepository.Update(shopItemToUpdate);
            return this.NoContent();
        }

        [HttpDelete("{shopItemId}")]
        public ActionResult Delete(int shopItemId)
        {
            shopItemRepository.Delete(shopItemId);
            return this.NoContent();
        }
    }
}