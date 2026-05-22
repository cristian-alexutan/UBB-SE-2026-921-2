namespace AirportLib.Services.Services.Interfaces
{
    public interface ITicketService
    {
        int CountTicketsBySubcategory(string subcategory);

        void AddTicket(Ticket ticket);
    }
}
