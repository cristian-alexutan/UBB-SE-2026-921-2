using AirportWebApp.Domain;

namespace AirportWebApp.Services.Interfaces
{
    public interface ITicketService
    {
        int CountTicketsBySubcategory(string subcategory);

        void AddTicket(Ticket ticket);
    }
}
