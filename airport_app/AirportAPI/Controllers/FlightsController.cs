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
    public ActionResult<Flight> Add([FromBody] FlightRequest flight)
    {
        if (flight == null)
        {
            return this.BadRequest(NullFlightDataErrorMessage);
        }

        Flight flightToAdd = ToFlight(flight);
        int flightId = flightRepository.AddFlight(flightToAdd);

        return this.CreatedAtAction(nameof(this.GetById), new { flightId }, flightToAdd);
    }

    [HttpPut("{flightId:int}")]
    public IActionResult Update(int flightId, [FromBody] FlightRequest flight)
    {
        if (flight == null)
        {
            return this.BadRequest(NullFlightDataErrorMessage);
        }

        if (flightRepository.GetFlightById(flightId) == null)
        {
            return this.NotFound();
        }

        Flight flightToUpdate = ToFlight(flight);
        flightToUpdate.Id = flightId;
        flightRepository.UpdateFlight(flightToUpdate);

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

    private static Flight ToFlight(FlightRequest request)
    {
        return new Flight
        {
            Id = request.Id,
            Date = request.Date,
            FlightNumber = request.FlightNumber,
            Route = new AirportAPI.Domain.Route { Id = request.RouteId },
            Runway = new Runway { Id = request.RunwayId },
            Gate = new Gate { Id = request.GateId }
        };
    }
}

public sealed record FlightRequest(
    int Id,
    DateTime Date,
    string FlightNumber,
    int RouteId,
    int RunwayId,
    int GateId);
