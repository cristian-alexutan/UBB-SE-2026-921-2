using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/carts")]
public class CartsController : ControllerBase
{
    private readonly ICartRepo cartRepo;

    public CartsController(ICartRepo cartRepo)
    {
        this.cartRepo = cartRepo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Cart>> GetAll()
    {
        return Ok(cartRepo.GetAll());
    }

    [HttpGet("{cartId:int}")]
    public ActionResult<Cart> GetById(int cartId)
    {
        Cart? cart = cartRepo.GetById(cartId);
        return cart == null ? NotFound() : Ok(cart);
    }

    [HttpPost]
    public ActionResult<Cart> Add(Cart cart)
    {
        cartRepo.Add(cart);
        return CreatedAtAction(nameof(GetById), new { cartId = cart.Id }, cart);
    }

    [HttpDelete("{cartId:int}")]
    public IActionResult Delete(int cartId)
    {
        if (cartRepo.GetById(cartId) == null)
        {
            return NotFound();
        }

        cartRepo.Delete(cartId);
        return NoContent();
    }

    [HttpPost("{cartId:int}/items")]
    public IActionResult AddItemToCart(int cartId, CartItem cartItem)
    {
        if (cartRepo.GetById(cartId) == null)
        {
            return NotFound();
        }

        cartRepo.AddItemToCart(cartId, cartItem);
        return NoContent();
    }

    [HttpDelete("{cartId:int}/items/{cartItemId:int}")]
    public IActionResult RemoveItemFromCart(int cartId, int cartItemId)
    {
        if (cartRepo.GetById(cartId) == null)
        {
            return NotFound();
        }

        cartRepo.RemoveItemFromCart(cartId, cartItemId);
        return NoContent();
    }

    [HttpPut("{cartId:int}/items/{cartItemId:int}/quantity")]
    public IActionResult UpdateItemQuantity(
        int cartId,
        int cartItemId,
        UpdateCartItemQuantityRequest request)
    {
        if (cartRepo.GetById(cartId) == null)
        {
            return NotFound();
        }

        cartRepo.UpdateItemQuantity(cartId, cartItemId, request.Quantity);
        return NoContent();
    }

    [HttpDelete("{cartId:int}/items")]
    public IActionResult ClearCart(int cartId)
    {
        if (cartRepo.GetById(cartId) == null)
        {
            return NotFound();
        }

        cartRepo.ClearCart(cartId);
        return NoContent();
    }
}

public sealed record UpdateCartItemQuantityRequest(int Quantity);
