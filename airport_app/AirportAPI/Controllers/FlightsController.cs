using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController(IFlightRepository flightRepository) : ControllerBase
{
    private const string NullFlightDataErrorMessage = "Flight data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Flight>> GetAll()
    {
        return this.Ok(flightRepository.GetAllFlights());
    }

    [HttpGet("{flightId:int}")]
    public ActionResult<Flight> GetById(int flightId)
    {
        Flight? flight = flightRepository.GetFlightById(flightId);

        if (flight == null)
        {
            return this.NotFound();
        }

        return this.Ok(flight);
    }

    [HttpGet("by-route/{routeId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByRouteId(int routeId)
    {
        return this.Ok(flightRepository.GetFlightsByRouteId(routeId));
    }

    [HttpGet("by-runway/{runwayId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByRunwayId(int runwayId)
    {
        return this.Ok(flightRepository.GetFlightsByRunwayId(runwayId));
    }

    [HttpGet("by-gate/{gateId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByGateId(int gateId)
    {
        return this.Ok(flightRepository.GetFlightsByGateId(gateId));
    }

    [HttpGet("by-airport/{airportId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByAirportId(int airportId)
    {
        return this.Ok(flightRepository.GetFlightsByAirportId(airportId));
    }

    [HttpPost]
    public ActionResult<Flight> Add([FromBody] Flight flight)
    {
        if (flight == null)
        {
            return this.BadRequest(NullFlightDataErrorMessage);
        }

        int flightId = flightRepository.AddFlight(flight);

        return this.CreatedAtAction(nameof(this.GetById), new { flightId }, flight);
    }

    [HttpPut("{flightId:int}")]
    public IActionResult Update(int flightId, [FromBody] Flight flight)
    {
        if (flight == null)
        {
            return this.BadRequest(NullFlightDataErrorMessage);
        }

        if (flightRepository.GetFlightById(flightId) == null)
        {
            return this.NotFound();
        }

        flight.Id = flightId;
        flightRepository.UpdateFlight(flight);

        return this.NoContent();
    }

    [HttpDelete("{flightId:int}")]
    public IActionResult Delete(int flightId)
    {
        // Rule 4: Fail Fast
        if (flightRepository.GetFlightById(flightId) == null)
        {
            return this.NotFound();
        }

        flightRepository.DeleteFlightUsingId(flightId);

        return this.NoContent();
    }
}