using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/flights")]
public class FlightsController(IFlightService flightService) : ControllerBase
{
    private const string NullFlightDataErrorMessage = "Flight data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Flight>> GetAllFlights()
    {
        return this.Ok(flightService.GetAllFlights());
    }

    [HttpGet("{flightId:int}")]
    public ActionResult<Flight> GetFlightById(int flightId)
    {
        Flight? flight = flightService.GetFlightById(flightId);

        if (flight == null)
        {
            return this.NotFound();
        }

        return this.Ok(flight);
    }

    [HttpGet("by-route/{routeId:int}")]
    public ActionResult<IEnumerable<Flight>> GetFlightsByRouteId(int routeId)
    {
        return this.Ok(flightService.GetFlightsByRouteId(routeId));
    }

    [HttpPost]
    public ActionResult<int> AddFlight([FromBody] Flight flight)
    {
        if (flight == null)
        {
            return this.BadRequest(NullFlightDataErrorMessage);
        }

        int flightId = flightService.AddFlight(
            flight.FlightNumber,
            flight.Route.Id,
            flight.Date,
            flight.Runway.Id,
            flight.Gate.Id);

        return this.Ok(flightId);
    }

    [HttpPut("{flightId:int}")]
    public IActionResult UpdateFlight(int flightId, [FromBody] Flight flight)
    {
        if (flight == null)
        {
            return this.BadRequest(NullFlightDataErrorMessage);
        }

        if (flightService.GetFlightById(flightId) == null)
        {
            return this.NotFound();
        }

        flightService.UpdateFlight(
            flightId,
            flight.Date,
            flight.FlightNumber,
            flight.Runway?.Id,
            flight.Gate?.Id);

        return this.NoContent();
    }

    [HttpDelete("{flightId:int}")]
    public IActionResult DeleteFlightUsingId(int flightId)
    {
        if (flightService.GetFlightById(flightId) == null)
        {
            return this.NotFound();
        }

        flightService.DeleteFlightUsingId(flightId);

        return this.NoContent();
    }
}