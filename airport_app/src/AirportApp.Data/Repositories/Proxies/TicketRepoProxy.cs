namespace AirportApp.Data.Repositories.Proxies;

public class TicketRepoProxy : RepositoryProxyBase, ITicketRepository
{
    public TicketRepoProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Ticket> GetAll()
    {
        return this.GetList<TicketDto>("api/tickets")
            .Select(MapTicket)
            .ToList();
    }

    public Ticket GetById(int ticketId)
    {
        return MapTicket(this.GetRequired<TicketDto>($"api/tickets/{ticketId}"));
    }

    public void Add(Ticket ticket)
    {
        this.Post("api/tickets", ticket);
    }

    public void Delete(int ticketId)
    {
        this.Delete($"api/tickets/{ticketId}");
    }

    private static Ticket MapTicket(TicketDto dto)
    {
        return new Ticket(dto.Id, dto.Category ?? string.Empty, dto.Subcategory ?? string.Empty);
    }

    private sealed class TicketDto
    {
        public int Id { get; set; }

        public string? Category { get; set; }

        public string? Subcategory { get; set; }
    }
}
