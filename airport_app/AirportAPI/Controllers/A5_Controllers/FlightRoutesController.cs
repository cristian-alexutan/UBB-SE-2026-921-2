using AirportLib.Domain.DTOs;

using Microsoft.AspNetCore.Mvc;

using Route = AirportLib.Domain.Domain.Route;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/flight-routes")]
public class FlightRoutesController(IFlightRouteService flightRouteService) : ControllerBase
{
    private const string NullRequestErrorMessage = "Request data cannot be null.";

    [HttpPost]
    public ActionResult<int> AddFlightToRoute([FromBody] AddFlightToRouteRequest request)
    {
        if (request == null)
        {
            return this.BadRequest(NullRequestErrorMessage);
        }

        try
        {
            int routeId = flightRouteService.AddFlightToRoute(
                request.CompanyId,
                request.AirportId,
                request.RouteType,
                request.RecurrenceInterval,
                request.StartDate,
                request.EndDate,
                request.DepartureTime,
                request.ArrivalTime,
                request.Capacity,
                request.FlightNumber,
                request.RunwayId,
                request.GateId);

            return this.Ok(routeId);
        }
        catch (InvalidOperationException ex)
        {
            return this.Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }

    [HttpGet("flights")]
    public ActionResult<IEnumerable<Flight>> GetAllFlights()
    {
        return this.Ok(flightRouteService.GetAllFlights());
    }

    [HttpGet("flights/details")]
    public ActionResult<IEnumerable<Flight>> GetAllFlightsWithDetails()
    {
        return this.Ok(flightRouteService.GetAllFlightsWithDetails());
    }

    [HttpGet("flights/{flightId:int}")]
    public ActionResult<Flight> GetFlightById(int flightId)
    {
        Flight? flight = flightRouteService.GetFlightById(flightId);

        if (flight == null)
        {
            return this.NotFound();
        }

        return this.Ok(flight);
    }

    [HttpGet("flights/by-company/{companyId:int}")]
    public ActionResult<IEnumerable<Flight>> GetFlightsByCompanyId(int companyId)
    {
        return this.Ok(flightRouteService.GetFlightsByCompanyId(companyId));
    }

    [HttpDelete("flights/{flightId:int}")]
    public IActionResult DeleteFlightUsingId(int flightId)
    {
        if (flightRouteService.GetFlightById(flightId) == null)
        {
            return this.NotFound();
        }

        flightRouteService.DeleteFlightUsingId(flightId);

        return this.NoContent();
    }

    [HttpGet("routes")]
    public ActionResult<IEnumerable<Route>> GetAllRoutes()
    {
        return this.Ok(flightRouteService.GetAllRoutes());
    }

    [HttpGet("routes/{routeId:int}")]
    public ActionResult<Route> GetRouteById(int routeId)
    {
        Route? route = flightRouteService.GetRouteById(routeId);

        if (route == null)
        {
            return this.NotFound();
        }

        return this.Ok(route);
    }
}