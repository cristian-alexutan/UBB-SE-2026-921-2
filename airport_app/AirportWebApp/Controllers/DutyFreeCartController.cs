using AirportWebApp.Infrastructure;
using AirportWebApp.Models.DutyFree;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

public class DutyFreeCartController : Controller
{
    private readonly WebUserSession session;
    private readonly ICartService cartService;
    private readonly IReservationService reservationService;

    public DutyFreeCartController(
        WebUserSession session,
        ICartService cartService,
        IReservationService reservationService)
    {
        this.session = session;
        this.cartService = cartService;
        this.reservationService = reservationService;
    }

    public IActionResult Index()
    {
        var cart = cartService.GetOrCreateCart(session.DutyFreeUserId);
        var cartItems = cart.CartItems.ToList();
        var total = cart.GetOverallPrice();

        Reservation? activeReservation = null;
        try
        {
            activeReservation = reservationService.GetActiveReservationForCart(cart.Id);
        }
        catch
        {
            // No active reservation.
        }

        var itemViewModels = cartItems.Select(cartItem => new CartItemViewModel
        {
            CartItem = cartItem,
            IsLast = cartItem.Quantity == 1
        }).ToList();

        var model = new CartViewModel
        {
            CartId = cart.Id,
            Items = itemViewModels,
            Total = total,
            HasActiveReservation = activeReservation != null,
            ActiveReservationId = activeReservation?.Id,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int cartId, int cartItemId, int quantity)
    {
        cartService.UpdateItemQuantity(cartId, cartItemId, quantity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveItem(int cartId, int cartItemId)
    {
        cartService.RemoveItemFromCart(cartId, cartItemId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Reserve(int cartId)
    {
        var cart = cartService.GetCartById(cartId);
        var reservation = new Reservation(cart, true, DateTime.UtcNow);
        reservationService.ReserveCart(reservation);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CancelReservation(int reservationId)
    {
        reservationService.CancelReservation(reservationId);
        return RedirectToAction(nameof(Index));
    }
}
