using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopsController : ControllerBase
{
    private readonly IShopService shopService;

    public ShopsController(IShopService shopService)
    {
        this.shopService = shopService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Shop>> GetAllShops()
    {
        var shops = shopService.GetAllAvailableShops();
        return Ok(shops);
    }

    [HttpGet("{id}")]
    public ActionResult<Shop> GetShopById(int id)
    {
        var shop = shopService.GetShopById(id);
        if (shop == null)
        {
            return NotFound();
        }

        return Ok(shop);
    }

    [HttpPost]
    public IActionResult CreateShop([FromBody] ShopRequest shopRequest)
    {
        if (shopRequest == null)
        {
            return BadRequest("Shop data is null.");
        }

        Shop shop = ToShop(shopRequest);
        shopService.AddShop(shop);

        return CreatedAtAction(nameof(GetAllShops), new { id = shop.Id }, shop);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateShop(int id, [FromBody] ShopRequest shopRequest)
    {
        if (shopRequest == null)
        {
            return BadRequest("Shop data is null.");
        }

        Shop shop = ToShop(shopRequest);
        shop.Id = id;
        shopService.UpdateShop(shop);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteShop(int id)
    {
        shopService.DeleteShop(id);
        return NoContent();
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Shop>> SearchShops([FromQuery] string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return BadRequest("Search input cannot be empty.");
        }

        var results = shopService.SearchByName(input);
        return Ok(results);
    }

    [HttpGet("sorted")]
    public ActionResult<IEnumerable<Shop>> GetSortedShops()
    {
        var shops = shopService.GetAllAvailableShops();
        var sortedShops = shopService.SortAlphabetically(shops);
        return Ok(sortedShops);
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

    public sealed record ShopRequest(int Id, string Name, string Type, int ManagerId);
}
