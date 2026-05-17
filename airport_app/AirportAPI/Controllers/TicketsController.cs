using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController(ITicketRepository ticketRepository) : ControllerBase
    {
        private const string NullTicketDataErrorMessage = "Ticket data cannot be null.";

        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> GetAll()
        {
            return this.Ok(ticketRepository.GetAll());
        }

        [HttpGet("{ticketId:int}")]
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
                return this.BadRequest(NullTicketDataErrorMessage);
            }

            ticketRepository.Add(newTicket);

            return this.CreatedAtAction(nameof(this.GetById), new { ticketId = newTicket.Id }, newTicket);
        }

        [HttpDelete("{ticketId:int}")]
        public IActionResult Delete(int ticketId)
        {
            if (ticketRepository.GetById(ticketId) == null)
            {
                return this.NotFound();
            }

            ticketRepository.Delete(ticketId);

            return this.NoContent();
        }
    }
}