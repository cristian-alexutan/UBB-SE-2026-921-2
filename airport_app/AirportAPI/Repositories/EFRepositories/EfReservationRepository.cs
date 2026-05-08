using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfReservationRepository(AppDbContext databaseContext) : IReservationRepository
    {
        public IEnumerable<Reservation> GetAll()
        {
            return databaseContext.Reservations
                .Include(reservation => reservation.ReservationCart)
                    .ThenInclude(cart => cart.Client)
                .Include(reservation => reservation.ReservationCart)
                    .ThenInclude(cart => cart.CartItems)
                        .ThenInclude(cartItem => cartItem.ShopItem)
                            .ThenInclude(shopItem => shopItem.Shop)
                .ToList();
        }

        public Reservation? GetById(int reservationId)
        {
            return databaseContext.Reservations
                .Include(reservation => reservation.ReservationCart)
                    .ThenInclude(cart => cart.Client)
                .Include(reservation => reservation.ReservationCart)
                    .ThenInclude(cart => cart.CartItems)
                        .ThenInclude(cartItem => cartItem.ShopItem)
                            .ThenInclude(shopItem => shopItem.Shop)
                .FirstOrDefault(reservation => reservation.Id == reservationId);
        }

        public void Add(Reservation newReservation)
        {
            if (newReservation.ReservationCart != null)
            {
                Cart? existingCart = databaseContext.Carts.Find(newReservation.ReservationCart.Id);
                if (existingCart == null)
                {
                    return;
                }

                newReservation.ReservationCart = existingCart;
            }

            databaseContext.Reservations.Add(newReservation);
            databaseContext.SaveChanges();
        }

        public void Update(Reservation reservationToUpdate)
        {
            Reservation? existingReservation = databaseContext.Reservations.Find(reservationToUpdate.Id);
            if (existingReservation == null)
            {
                return;
            }

            existingReservation.Active = reservationToUpdate.Active;
            existingReservation.ReservationDate = reservationToUpdate.ReservationDate;

            if (reservationToUpdate.ReservationCart != null)
            {
                Cart? existingCart = databaseContext.Carts.Find(reservationToUpdate.ReservationCart.Id);
                if (existingCart != null)
                {
                    existingReservation.ReservationCart = existingCart;
                }
            }

            databaseContext.SaveChanges();
        }

        public void Delete(int reservationId)
        {
            Reservation? reservationToRemove = databaseContext.Reservations.Find(reservationId);

            if (reservationToRemove == null)
            {
                return;
            }

            databaseContext.Reservations.Remove(reservationToRemove);
            databaseContext.SaveChanges();
        }
    }
}
