using AirportLib.Services.Services.Interfaces;

namespace AirportLib.Services.Services.Proxies;

public class TicketServiceProxy : ServiceProxyBase, ITicketService
{
    public TicketServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public void AddTicket(Ticket ticketToAdd)
    {
        this.Post("api/tickets", ticketToAdd);
    }

    public int CountTicketsBySubcategory(string subcategoryName)
    {
        string escapedName = Uri.EscapeDataString(subcategoryName);
        return GetRequired<int>($"api/tickets/count/subcategory?name={escapedName}");
    }
}
