using System.Collections.Generic;
using AirportAPI.Domain;

namespace AirportAPI.Repositories.Interfaces
{
    public interface ITicketRepo
    {
        IEnumerable<Ticket> GetAll();

        Ticket GetById(int ticketId);

        void Add(Ticket ticket);

        void Delete(int ticketId);
    }
}


