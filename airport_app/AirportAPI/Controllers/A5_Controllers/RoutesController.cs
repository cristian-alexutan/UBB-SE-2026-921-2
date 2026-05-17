using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Route = AirportAPI.Domain.Route;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/routes")]
public class RoutesController(IRouteService routeService) : ControllerBase
{
    private const string NullRouteDataErrorMessage = "Route data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Route>> GetAll()
    {
        return this.Ok(routeService.GetAllRoutes());
    }

    [HttpGet("{routeId:int}")]
    public ActionResult<Route> GetById(int routeId)
    {
        Route? route = routeService.GetRouteById(routeId);

        if (route == null)
        {
            return this.NotFound();
        }

        return this.Ok(route);
    }

    [HttpPost]
    public ActionResult<int> AddWithInitialFlight([FromBody] AddRouteWithFlightRequest request)
    {
        if (request == null)
        {
            return this.BadRequest(NullRouteDataErrorMessage);
        }

        try
        {
            int routeId = routeService.AddWithInitialFlight(
                request.CompanyId,
                request.AirportId,
                request.RouteType,
                request.Interval,
                request.Start,
                request.End,
                request.Dep,
                request.Arr,
                request.Capacity,
                request.FlightNum,
                request.RunwayId,
                request.GateId);

            return this.Ok(routeId);
        }
        catch (InvalidOperationException ex)
        {
            return this.Conflict(ex.Message);
        }
    }

    [HttpGet("normalize-type")]
    public ActionResult<string> NormalizeFlightType([FromQuery] string? routeType)
    {
        return this.Ok(new { value = routeService.NormalizeFlightType(routeType) });
    }

    [HttpPost("relevant-time")]
    public ActionResult<string> GetRelevantTime([FromBody] RouteTimeRequest request)
    {
        Route route = new()
        {
            RouteType = request.RouteType ?? string.Empty,
            DepartureTime = request.DepartureTime,
            ArrivalTime = request.ArrivalTime
        };

        return this.Ok(new { value = routeService.GetRelevantTime(route) });
    }
}

public sealed record AddRouteWithFlightRequest(
    int CompanyId,
    int AirportId,
    string RouteType,
    int Interval,
    DateTime Start,
    DateTime End,
    TimeOnly Dep,
    TimeOnly Arr,
    int Capacity,
    string FlightNum,
    int RunwayId,
    int GateId);

public sealed record RouteTimeRequest(string? RouteType, TimeOnly DepartureTime, TimeOnly ArrivalTime);
