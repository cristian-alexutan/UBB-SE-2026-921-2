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
            databaseContext.Reservations.Add(newReservation);
            databaseContext.SaveChanges();
        }

        public void Update(Reservation reservationToUpdate)
        {
            databaseContext.Reservations.Update(reservationToUpdate);
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