using System.Linq;
using AirportApp.Data.Domain;
using AirportApp.Data.Repositories.Interfaces;
using AirportApp.Data.Services.Interfaces;

namespace AirportApp.Data.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepo ticketRepo;

        public TicketService(ITicketRepo ticketRepo)
        {
            this.ticketRepo = ticketRepo;
        }

        public int CountTicketsBySubcategory(string subcategory)
        {
            if (string.IsNullOrEmpty(subcategory))
            {
                throw new ArgumentException("Subcategory cannot be null or empty.", nameof(subcategory));
            }

            return this.ticketRepo.GetAll().Count(ticket => ticket.Subcategory == subcategory);
        }

        public void AddTicket(Ticket ticket)
        {
            this.ticketRepo.Add(ticket);
        }
    }
}
