using System;
using System.Collections.Generic;
using System.Text;

using AirportWebApp.Services.Proxies;

namespace AirportWebApp.Services.Proxies;

public class TicketServiceProxy : RepositoryProxyBase, ITicketService
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
