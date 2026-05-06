using AirportApp.Data.Domain;

namespace AirportApp.Data.Services.Interfaces
{
    public interface ITicketService
    {
        int CountTicketsBySubcategory(string subcategory);

        void AddTicket(Ticket ticket);
    }
}
