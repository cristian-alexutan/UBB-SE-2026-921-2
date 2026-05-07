using AirportAPI;
using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class ReservationDbRepo : IReservationRepo
    {
        private readonly AppDbContext context;

        public ReservationDbRepo(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Reservation> GetAll()
        {
            return context.Reservations
                .Include(r => r.ReservationCart)
                    .ThenInclude(c => c.Client)
                .Include(r => r.ReservationCart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.ShopItem)
                            .ThenInclude(si => si.Shop)
                .ToList();
        }

        public Reservation GetById(int reservationId)
        {
            return context.Reservations.Include(r => r.ReservationCart)
                    .ThenInclude(c => c.Client)
                .Include(r => r.ReservationCart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.ShopItem)
                            .ThenInclude(si => si.Shop)
                .FirstOrDefault(r => r.Id == reservationId);
        }

        public void Add(Reservation reservation)
        {
            context.Reservations.Add(reservation);
            context.SaveChanges();
        }

        public void Delete(int reservationId)
        {
            var reservation = context.Reservations.Find(reservationId);
            if (reservation != null)
            {
                context.Reservations.Remove(reservation);
                context.SaveChanges();
            }
        }

        public void Update(Reservation reservation)
        {
            context.Reservations.Update(reservation);
            context.SaveChanges();
        }
    }
}



