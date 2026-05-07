using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController(IShopRepository shopRepository) : ControllerBase
    {
        private const string NullShopDataErrorMessage = "Shop data cannot be null.";
        private const string UpdateNotFoundErrorMessage = "The shop to update was not found.";

        [HttpGet]
        public ActionResult<IEnumerable<Shop>> GetAll()
        {
            return this.Ok(shopRepository.GetAll());
        }

        [HttpGet("{shopId:int}")]
        public ActionResult<Shop> GetById(int shopId)
        {
            Shop? shop = shopRepository.GetById(shopId);

            if (shop == null)
            {
                return this.NotFound();
            }

            return this.Ok(shop);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Shop newShop)
        {
            if (newShop == null)
            {
                return this.BadRequest(NullShopDataErrorMessage);
            }

            shopRepository.Add(newShop);

            return this.CreatedAtAction(nameof(this.GetById), new { shopId = newShop.Id }, newShop);
        }

        [HttpPut]
        public ActionResult<Shop> Update([FromBody] Shop shopToUpdate)
        {
            if (shopToUpdate == null)
            {
                return this.BadRequest(NullShopDataErrorMessage);
            }

            Shop? updatedShop = shopRepository.Update(shopToUpdate);

            if (updatedShop == null)
            {
                return this.NotFound(UpdateNotFoundErrorMessage);
            }

            return this.Ok(updatedShop);
        }

        [HttpDelete("{shopId:int}")]
        public ActionResult<Shop> Delete(int shopId)
        {
            Shop? deletedShop = shopRepository.Delete(shopId);

            if (deletedShop == null)
            {
                return this.NotFound();
            }

            return this.Ok(deletedShop);
        }
    }
}