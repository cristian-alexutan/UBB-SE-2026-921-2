using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfTicketRepository(AppDbContext databaseContext) : ITicketRepository
    {
        public IEnumerable<Ticket> GetAll()
        {
            return databaseContext.Tickets
                .AsNoTracking()
                .ToList();
        }

        public Ticket GetById(int ticketId)
        {
            Ticket? foundTicket = databaseContext.Tickets.Find(ticketId);

            if (foundTicket == null)
            {
                throw new KeyNotFoundException($"The ticket with Id {ticketId} was not found.");
            }

            return foundTicket;
        }

        public void Add(Ticket newTicket)
        {
            databaseContext.Tickets.Add(newTicket);
            databaseContext.SaveChanges();
        }

        public void Delete(int ticketId)
        {
            Ticket? ticketToRemove = databaseContext.Tickets.Find(ticketId);

            if (ticketToRemove == null)
            {
                return;
            }

            databaseContext.Tickets.Remove(ticketToRemove);
            databaseContext.SaveChanges();
        }
    }
}