using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories;

public class EfTicketRepository : ITicketRepository
{
    private readonly AppDbContext dbContext;

    public EfTicketRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<Ticket> GetAll()
    {
        return this.dbContext.Tickets.AsNoTracking().ToList();
    }

    public Ticket GetById(int ticketId)
    {
        return this.dbContext.Tickets.Find(ticketId)!;
    }

    public void Add(Ticket ticket)
    {
        this.dbContext.Tickets.Add(ticket);
        this.dbContext.SaveChanges();
    }

    public void Delete(int ticketId)
    {
        Ticket? ticket = this.dbContext.Tickets.Find(ticketId);
        if (ticket == null)
        {
            return;
        }

        this.dbContext.Tickets.Remove(ticket);
        this.dbContext.SaveChanges();
    }
}



