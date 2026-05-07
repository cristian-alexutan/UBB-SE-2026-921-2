using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/flights")]
public class FlightsController : ControllerBase
{
    private readonly IFlightRepository flightRepository;

    public FlightsController(IFlightRepository flightRepository)
    {
        this.flightRepository = flightRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Flight>> GetAll()
    {
        return Ok(flightRepository.GetAllFlights());
    }

    [HttpGet("{flightId:int}")]
    public ActionResult<Flight> GetById(int flightId)
    {
        Flight? flight = flightRepository.GetFlightById(flightId);
        return flight == null ? NotFound() : Ok(flight);
    }

    [HttpGet("by-route/{routeId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByRouteId(int routeId)
    {
        return Ok(flightRepository.GetFlightsByRouteId(routeId));
    }

    [HttpGet("by-runway/{runwayId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByRunwayId(int runwayId)
    {
        return Ok(flightRepository.GetFlightsByRunwayId(runwayId));
    }

    [HttpGet("by-gate/{gateId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByGateId(int gateId)
    {
        return Ok(flightRepository.GetFlightsByGateId(gateId));
    }

    [HttpGet("by-airport/{airportId:int}")]
    public ActionResult<IEnumerable<Flight>> GetByAirportId(int airportId)
    {
        return Ok(flightRepository.GetFlightsByAirportId(airportId));
    }

    [HttpPost]
    public ActionResult<Flight> Add(Flight flight)
    {
        int flightId = flightRepository.AddFlight(flight);
        return CreatedAtAction(nameof(GetById), new { flightId }, flight);
    }

    [HttpPut("{flightId:int}")]
    public IActionResult Update(int flightId, Flight flight)
    {
        if (flightRepository.GetFlightById(flightId) == null)
        {
            return NotFound();
        }

        flight.Id = flightId;
        flightRepository.UpdateFlight(flight);
        return NoContent();
    }

    [HttpDelete("{flightId:int}")]
    public IActionResult Delete(int flightId)
    {
        if (flightRepository.GetFlightById(flightId) == null)
        {
            return NotFound();
        }

        flightRepository.DeleteFlightUsingId(flightId);
        return NoContent();
    }
}
