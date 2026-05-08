using System.Collections.Generic;
using AirportApp.Data.Domain;

namespace AirportApp.Data.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetAll();

        Ticket GetById(int ticketId);

        void Add(Ticket ticket);

        void Delete(int ticketId);
    }
}
