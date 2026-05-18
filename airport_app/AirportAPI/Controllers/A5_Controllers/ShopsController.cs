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

    [HttpPost]
    public IActionResult CreateShop([FromBody] Shop shop)
    {
        if (shop == null)
        {
            return BadRequest("Shop data is null.");
        }

        shopService.AddShop(shop);

        return CreatedAtAction(nameof(GetAllShops), new { id = shop.Id }, shop);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateShop(int id, [FromBody] Shop shop)
    {
        if (shop == null)
        {
            return BadRequest("Shop data is null.");
        }

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
}
