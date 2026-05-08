using AirportApp.Data.Domain;
using AirportApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportApp.Data.Repositories;

public class EfTicketRepo : ITicketRepository
{
    private readonly AppDbContext dbContext;

    public EfTicketRepo(AppDbContext dbContext)
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
