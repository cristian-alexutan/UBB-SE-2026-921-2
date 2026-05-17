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
        public ActionResult Add([FromBody] ShopRequest newShop)
        {
            if (newShop == null)
            {
                return this.BadRequest(NullShopDataErrorMessage);
            }

            Shop shop = ToShop(newShop);
            shopRepository.Add(shop);

            return this.CreatedAtAction(nameof(this.GetById), new { shopId = shop.Id }, shop);
        }

        [HttpPut]
        public ActionResult<Shop> Update([FromBody] ShopRequest shopToUpdate)
        {
            if (shopToUpdate == null)
            {
                return this.BadRequest(NullShopDataErrorMessage);
            }

            Shop? updatedShop = shopRepository.Update(ToShop(shopToUpdate));

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

        private static Shop ToShop(ShopRequest request)
        {
            return new Shop
            {
                Id = request.Id,
                Name = request.Name,
                Type = request.Type,
                Manager = new Manager { Id = request.ManagerId }
            };
        }
    }

    public sealed record ShopRequest(int Id, string Name, string Type, int ManagerId);
}
