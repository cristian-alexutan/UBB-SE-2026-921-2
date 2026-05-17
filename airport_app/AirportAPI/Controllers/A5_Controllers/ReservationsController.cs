using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll()
    {
        return this.Ok(reservationService.GetAllReservations());
    }

    [HttpGet("{reservationId:int}")]
    public ActionResult<Reservation> GetById(int reservationId)
    {
        Reservation? reservation = reservationService.GetReservationById(reservationId);

        if (reservation == null)
        {
            return this.NotFound();
        }

        return this.Ok(reservation);
    }

    [HttpPost("reserve")]
    public ActionResult Reserve([FromBody] ReserveCartRequest request)
    {
        if (request == null)
        {
            return this.BadRequest("Reservation data is required.");
        }

        Reservation reservation = new Reservation(
            new Cart
            {
                Id = request.CartId,
                CartItems = request.CartItems
                    .Select(ci => new CartItem
                    {
                        Id = ci.Id,
                        ShopItem = new ShopItem { Id = ci.ShopItemId },
                        Quantity = ci.Quantity
                    })
                    .ToList()
            },
            request.Active,
            request.ReservationDate);

        try
        {
            reservationService.ReserveCart(reservation);
            return this.Ok(reservation.Id);
        }
        catch (InvalidOperationException ex)
        {
            return this.Conflict(ex.Message);
        }
    }

    [HttpDelete("{reservationId:int}")]
    public ActionResult Delete(int reservationId)
    {
        reservationService.DeleteReservation(reservationId);
        return this.NoContent();
    }

    [HttpPut("{reservationId:int}/cancel")]
    public ActionResult Cancel(int reservationId)
    {
        reservationService.CancelReservation(reservationId);
        return this.NoContent();
    }

    [HttpGet("cart/{cartId:int}/active")]
    public ActionResult<Reservation> GetActiveReservationForCart(int cartId)
    {
        Reservation? reservation = reservationService.GetActiveReservationForCart(cartId);

        if (reservation == null)
        {
            return this.NotFound();
        }

        return this.Ok(reservation);
    }
}

public sealed record ReserveCartRequest(
    int CartId,
    bool Active,
    DateTime ReservationDate,
    List<CartItemRequest> CartItems);
