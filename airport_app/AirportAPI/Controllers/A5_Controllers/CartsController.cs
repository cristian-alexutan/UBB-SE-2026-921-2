using Microsoft.AspNetCore.Mvc;
using AirportAPI.Services.Interfaces;
namespace AirportAPI.Controllers.A5_Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController(ICartService cartService) : ControllerBase
{
    private const string MissingCartDataErrorMessage = "Cart data cannot be null.";
    private const string MissingItemDataErrorMessage = "Cart item data cannot be null.";
    private const string MissingRequestDataErrorMessage = "Update request data cannot be null."; [HttpGet]
    public ActionResult<IEnumerable<Cart>> GetAll()
    {
        return this.Ok(cartService.GetAllCarts());
    }

    [HttpGet("{cartId:int}")]
    public ActionResult<Cart> GetById(int cartId)
    {
        Cart? cart = cartService.GetCartById(cartId);

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

        cartService.AddCart(cart);

        return this.CreatedAtAction(nameof(this.GetById), new { cartId = cart.Id }, cart);
    }

    [HttpDelete("{cartId:int}")]
    public IActionResult Delete(int cartId)
    {
        if (cartService.GetCartById(cartId) == null)
        {
            return this.NotFound();
        }

        cartService.DeleteCart(cartId);

        return this.NoContent();
    }

    [HttpPost("{cartId:int}/items")]
    public IActionResult AddItemToCart(int cartId, [FromBody] CartItemRequest request)
    {
        if (request == null)
        {
            return this.BadRequest(MissingItemDataErrorMessage);
        }

        if (cartService.GetCartById(cartId) == null)
        {
            return this.NotFound();
        }

        CartItem cartItem = new()
        {
            ShopItem = new ShopItem { Id = request.ShopItemId },
            Quantity = request.Quantity
        };

        cartService.AddItemToCart(cartId, cartItem);

        return this.NoContent();
    }

    [HttpDelete("{cartId:int}/items/{cartItemId:int}")]
    public IActionResult RemoveItemFromCart(int cartId, int cartItemId)
    {
        if (cartService.GetCartById(cartId) == null)
        {
            return this.NotFound();
        }

        cartService.RemoveItemFromCart(cartId, cartItemId);

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

        if (cartService.GetCartById(cartId) == null)
        {
            return this.NotFound();
        }

        cartService.UpdateItemQuantity(cartId, cartItemId, request.Quantity);

        return this.NoContent();
    }
    [HttpDelete("{cartId:int}/items")]
    public IActionResult ClearCart(int cartId)
    {
        if (cartService.GetCartById(cartId) == null)
        {
            return this.NotFound();
        }

        cartService.ClearCart(cartId);

        return this.NoContent();
    }
}
public sealed record UpdateCartItemQuantityRequest(int Quantity);

public sealed record CartItemRequest(int Id, int ShopItemId, int Quantity);