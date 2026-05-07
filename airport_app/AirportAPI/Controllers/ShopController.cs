using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/shop")]
    public class ShopController(IShopRepo shopRepository) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Shop>> GetAll()
        {
            return this.Ok(shopRepository.GetAll());
        }

        [HttpGet("{shopId}")]
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
                return this.BadRequest("Shop data is required.");
            }

            shopRepository.Add(newShop);
            return this.Ok();
        }

        [HttpPut]
        public ActionResult<Shop> Update([FromBody] Shop shopToUpdate)
        {
            Shop? updatedShop = shopRepository.Update(shopToUpdate);

            if (updatedShop == null)
            {
                return this.NotFound("The shop to update was not found.");
            }

            return this.Ok(updatedShop);
        }

        [HttpDelete("{shopId}")]
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