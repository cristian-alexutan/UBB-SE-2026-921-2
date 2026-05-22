using AirportWebApp.Services.Proxies;
using AirportWebApp.Services.Interfaces;

using Route = AirportWebApp.Domain.Route;
namespace AirportWebApp.Services.Proxies;

public class RouteServiceProxy : ServiceProxyBase, IRouteService
{
    private const int MinutesInADay = 1440;
    private const int MinutesInAnHour = 60;
    private const string ArrivalCode = "ARR";
    private const string ArrivalFullName = "ARRIVAL";
    private const string DepartureCode = "DEP";
    private const string DepartureFullName = "DEPARTURE";
    private const string EmptyFieldPlaceholder = "-";
    private const string TimeFormat = "HH:mm";

    public RouteServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Route> GetAllRoutes()
    {
        return this.GetList<Route>("api/routes");
    }

    public Route? GetRouteById(int routeId)
    {
        return this.GetOptional<Route>($"api/routes/{routeId}");
    }

    public int AddWithInitialFlight(
        int companyId,
        int airportId,
        string routeType,
        int interval,
        DateTime start,
        DateTime end,
        TimeOnly dep,
        TimeOnly arr,
        int capacity,
        string flightNum,
        int runwayId,
        int gateId)
    {
        var request = new AddRouteWithFlightRequest(
            companyId, airportId, routeType,
            interval, start, end,
            dep, arr, capacity,
            flightNum, runwayId, gateId);

        return this.PostForResult<AddRouteWithFlightRequest, int>("api/routes", request);
    }

    public string NormalizeFlightType(string? routeType)
    {
        if (string.IsNullOrWhiteSpace(routeType))
        {
            return EmptyFieldPlaceholder;
        }

        string upperCaseType = routeType.Trim().ToUpperInvariant();

        if (upperCaseType.StartsWith(ArrivalCode) || upperCaseType.StartsWith(ArrivalFullName))
        {
            return ArrivalCode;
        }

        if (upperCaseType.StartsWith(DepartureCode) || upperCaseType.StartsWith(DepartureFullName))
        {
            return DepartureCode;
        }

        return upperCaseType;
    }

    public string GetRelevantTime(Route? route)
    {
        if (route == null)
        {
            return EmptyFieldPlaceholder;
        }

        string normalizedType = this.NormalizeFlightType(route.RouteType);

        if (normalizedType == ArrivalCode)
        {
            return route.ArrivalTime.ToString(TimeFormat);
        }

        return route.DepartureTime.ToString(TimeFormat);
    }

    private sealed record AddRouteWithFlightRequest(
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
}
