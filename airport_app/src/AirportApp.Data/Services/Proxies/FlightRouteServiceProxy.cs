using AirportApp.Data.Repositories.Proxies;
using AirportApp.Data.Services;

using Route = AirportApp.Data.Domain.Route;

namespace AirportApp.Data.Services.Proxies;

public class FlightRouteServiceProxy : RepositoryProxyBase, IFlightRouteService
{
    private const int MinutesInADay = 1440;
    private const int MinutesInAnHour = 60;
    private const string ArrivalText = "Arrival";
    private const string ArrivalCode = "ARR";
    private const string DepartureCode = "DEP";
    private const string FlightDateTimeFormat = "dd.MM.yyyy HH:mm";
    private const string EmptyFieldPlaceholder = "-";

    private const int DailyIntervalDays = 1;
    private const int WeeklyIntervalDays = 7;
    private const int MonthlyIntervalDays = 30;

    public FlightRouteServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    private sealed class AddFlightToRouteRequest
    {
        public int CompanyId { get; set; }
        public int AirportId { get; set; }
        public string RouteType { get; set; } = string.Empty;
        public int RecurrenceInterval { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public int Capacity { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public int RunwayId { get; set; }
        public int GateId { get; set; }
    }

    public int AddFlightToRoute(
        int companyId,
        int airportId,
        string routeType,
        int recurrenceInterval,
        DateTime startDate,
        DateTime endDate,
        TimeOnly departureTime,
        TimeOnly arrivalTime,
        int capacity,
        string flightNumber,
        int runwayId,
        int gateId)
    {
        var request = new AddFlightToRouteRequest
        {
            CompanyId = companyId,
            AirportId = airportId,
            RouteType = routeType,
            RecurrenceInterval = recurrenceInterval,
            StartDate = startDate,
            EndDate = endDate,
            DepartureTime = departureTime,
            ArrivalTime = arrivalTime,
            Capacity = capacity,
            FlightNumber = flightNumber,
            RunwayId = runwayId,
            GateId = gateId
        };

        return this.PostForResult<AddFlightToRouteRequest, int>("api/flight-routes", request);
    }

    public void CreateFlightWithSchedule(
        int companyId,
        string routeTypeDisplayName,
        int airportId,
        int capacity,
        TimeSpan departureOffset,
        TimeSpan arrivalOffset,
        bool isRecurrent,
        DateTime? startDate,
        DateTime? endDate,
        DateTime? singleDate,
        string recurrenceType,
        string customDaysText,
        int runwayId,
        int gateId,
        Func<int, string> flightCodeGenerator)
    {
        if (companyId <= 0)
        {
            throw new InvalidOperationException("A company must be selected before adding a flight.");
        }
        if (airportId <= 0 || runwayId <= 0 || gateId <= 0)
        {
            throw new InvalidOperationException("Please ensure all required fields are populated.");
        }
        if (capacity <= 0)
        {
            throw new InvalidOperationException("The provided capacity value is invalid.");
        }

        string routeType = routeTypeDisplayName == ArrivalText ? ArrivalCode : DepartureCode;

        DateTime start = isRecurrent ? startDate?.Date ?? DateTime.Today : singleDate?.Date ?? DateTime.Today;
        DateTime end = isRecurrent ? endDate?.Date ?? start : start;

        if (isRecurrent && end < start)
        {
            throw new InvalidOperationException("The end date must be after the start date.");
        }

        int interval = 0;
        if (isRecurrent)
        {
            interval = recurrenceType switch
            {
                nameof(RecurrenceType.Daily) => DailyIntervalDays,
                nameof(RecurrenceType.Weekly) => WeeklyIntervalDays,
                nameof(RecurrenceType.Monthly) => MonthlyIntervalDays,
                nameof(RecurrenceType.Custom) => int.TryParse(customDaysText, out int custom) && custom > 0
                    ? custom
                    : throw new InvalidOperationException("Invalid custom interval."),
                _ => throw new InvalidOperationException("A recurrence type is required for recurrent flights.")
            };
        }

        TimeOnly departureTime = TimeOnly.FromTimeSpan(departureOffset);
        TimeOnly arrivalTime = TimeOnly.FromTimeSpan(arrivalOffset);

        if (departureTime == arrivalTime)
        {
            throw new InvalidOperationException("Arrival time cannot be identical to departure time.");
        }

        string flightNumber = flightCodeGenerator(companyId);
        this.AddFlightToRoute(companyId, airportId, routeType, interval, start, end, departureTime, arrivalTime, capacity, flightNumber, runwayId, gateId);
    }

    public List<Flight> GetAllFlightsWithDetails()
    {
        return this.GetList<Flight>("api/flight-routes/flights/details");
    }

    public Route? GetRouteById(int routeId)
    {
        return this.GetOptional<Route>($"api/flight-routes/routes/{routeId}");
    }

    public Flight? GetFlightById(int flightId)
    {
        return this.GetOptional<Flight>($"api/flight-routes/flights/{flightId}");
    }

    public List<Route> GetAllRoutes()
    {
        return this.GetList<Route>("api/flight-routes/routes");
    }

    public List<Flight> GetAllFlights()
    {
        return this.GetList<Flight>("api/flight-routes/flights");
    }

    public void DeleteFlightUsingId(int flightId)
    {
        this.Delete($"api/flight-routes/flights/{flightId}");
    }

    public List<Flight> GetFlightsByCompanyId(int companyId)
    {
        return this.GetList<Flight>($"api/flight-routes/flights/by-company/{companyId}");
    }

    public string GetDestinationText(Flight flight)
    {
        if (flight.Route == null || flight.Route.Airport == null)
        {
            return EmptyFieldPlaceholder;
        }

        return $"{flight.Route.Airport.Code} - {flight.Route.Airport.Name}";
    }

    public List<Flight> SearchFlights(List<Flight> flights, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return flights;
        }

        List<Flight> matching = new List<Flight>();
        foreach (Flight flight in flights)
        {
            if (this.IsFlightMatch(flight, query))
            {
                matching.Add(flight);
            }
        }

        return matching;
    }

    public List<Flight> SearchFlightsByNumber(List<Flight> flights, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return flights;
        }

        List<Flight> matching = new List<Flight>();
        foreach (Flight flight in flights)
        {
            if (flight.FlightNumber != null && flight.FlightNumber.ToLowerInvariant().Contains(query))
            {
                matching.Add(flight);
            }
        }

        return matching;
    }

    public FlightSummary BuildFlightSummary(Flight flight, string crewText)
    {
        return new FlightSummary
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber ?? string.Empty,
            DateText = flight.Date.ToString(FlightDateTimeFormat),
            DestinationText = this.GetDestinationText(flight),
            RunwayText = flight.Runway?.Name ?? EmptyFieldPlaceholder,
            GateText = flight.Gate?.Name ?? EmptyFieldPlaceholder,
            CrewText = crewText
        };
    }

    private bool IsFlightMatch(Flight flight, string query)
    {
        if (flight.FlightNumber != null && flight.FlightNumber.ToLowerInvariant().Contains(query))
        {
            return true;
        }

        if (flight.Date.ToString(FlightDateTimeFormat).ToLowerInvariant().Contains(query))
        {
            return true;
        }

        string destination = this.GetDestinationText(flight).ToLowerInvariant();
        if (destination.Contains(query))
        {
            return true;
        }

        if (flight.Runway?.Name != null && flight.Runway.Name.ToLowerInvariant().Contains(query))
        {
            return true;
        }

        if (flight.Gate?.Name != null && flight.Gate.Name.ToLowerInvariant().Contains(query))
        {
            return true;
        }

        return false;
    }
}