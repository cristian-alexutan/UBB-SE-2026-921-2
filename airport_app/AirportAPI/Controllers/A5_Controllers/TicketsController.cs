using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService ticketService;

    public TicketsController(ITicketService ticketService)
    {
        this.ticketService = ticketService;
    }

    [HttpPost]
    public IActionResult CreateTicket([FromBody] Ticket ticket)
    {
        if (ticket == null)
        {
            return BadRequest("Ticket data is null.");
        }

        try
        {
            ticketService.AddTicket(ticket);
            return StatusCode(StatusCodes.Status201Created, ticket);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while saving the ticket: {ex.Message}");
        }
    }

    [HttpGet("count/subcategory")]
    public ActionResult<int> GetTicketCountBySubcategory([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Subcategory name cannot be null or empty.");
        }

        try
        {
            int count = ticketService.CountTicketsBySubcategory(name);
            return Ok(count);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }
}