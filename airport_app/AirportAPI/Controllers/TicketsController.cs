using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController(ITicketRepo ticketRepository) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Ticket>> GetAll()
    {
        return this.Ok(ticketRepository.GetAll());
    }

    [HttpGet("{ticketId}")]
    public ActionResult<Ticket> GetById(int ticketId)
    {
        Ticket? ticket = ticketRepository.GetById(ticketId);

        if (ticket == null)
        {
            return this.NotFound();
        }

        return this.Ok(ticket);
    }

    [HttpPost]
    public ActionResult Add([FromBody] Ticket newTicket)
    {
        if (newTicket == null)
        {
            return this.BadRequest("Ticket data is required.");
        }

        ticketRepository.Add(newTicket);
        return this.Ok();
    }

    [HttpDelete("{ticketId}")]
    public ActionResult Delete(int ticketId)
    {
        ticketRepository.Delete(ticketId);
        return this.NoContent();
    }
}