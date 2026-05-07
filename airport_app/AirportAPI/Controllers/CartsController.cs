using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController(ICartRepository cartRepository) : ControllerBase
{
    private const string MissingCartDataErrorMessage = "Cart data cannot be null.";
    private const string MissingItemDataErrorMessage = "Cart item data cannot be null.";
    private const string MissingRequestDataErrorMessage = "Update request data cannot be null."; [HttpGet]
    public ActionResult<IEnumerable<Cart>> GetAll()
    {
        return this.Ok(cartRepository.GetAll());
    }

    [HttpGet("{cartId:int}")]
    public ActionResult<Cart> GetById(int cartId)
    {
        Cart? cart = cartRepository.GetById(cartId);

        if (cart == null)
        {
            return this.NotFound();
        }

        return this.Ok(cart);
    }

    [HttpPost]
    public ActionResult<Cart> Add([FromBody] Cart cart)
    {
        if (cart == null)
        {
            return this.BadRequest(MissingCartDataErrorMessage);
        }

        cartRepository.Add(cart);

        return this.CreatedAtAction(nameof(this.GetById), new { cartId = cart.Id }, cart);
    }

    [HttpDelete("{cartId:int}")]
    public IActionResult Delete(int cartId)
    {
        if (cartRepository.GetById(cartId) == null)
        {
            return this.NotFound();
        }

        cartRepository.Delete(cartId);

        return this.NoContent();
    }

    [HttpPost("{cartId:int}/items")]
    public IActionResult AddItemToCart(int cartId, [FromBody] CartItem cartItem)
    {
        if (cartItem == null)
        {
            return this.BadRequest(MissingItemDataErrorMessage);
        }

        if (cartRepository.GetById(cartId) == null)
        {
            return this.NotFound();
        }

        cartRepository.AddItemToCart(cartId, cartItem);

        return this.NoContent();
    }

    [HttpDelete("{cartId:int}/items/{cartItemId:int}")]
    public IActionResult RemoveItemFromCart(int cartId, int cartItemId)
    {
        if (cartRepository.GetById(cartId) == null)
        {
            return this.NotFound();
        }

        cartRepository.RemoveItemFromCart(cartId, cartItemId);

        return this.NoContent();
    }

    [HttpPut("{cartId:int}/items/{cartItemId:int}/quantity")]
    public IActionResult UpdateItemQuantity(
        int cartId,
        int cartItemId,
        [FromBody] UpdateCartItemQuantityRequest request)
    {
        if (request == null)
        {
            return this.BadRequest(MissingRequestDataErrorMessage);
        }

        if (cartRepository.GetById(cartId) == null)
        {
            return this.NotFound();
        }

        cartRepository.UpdateItemQuantity(cartId, cartItemId, request.Quantity);

        return this.NoContent();
    }
    [HttpDelete("{cartId:int}/items")]
    public IActionResult ClearCart(int cartId)
    {
        if (cartRepository.GetById(cartId) == null)
        {
            return this.NotFound();
        }

        cartRepository.ClearCart(cartId);

        return this.NoContent();
    }
}

public sealed record UpdateCartItemQuantityRequest(int Quantity);