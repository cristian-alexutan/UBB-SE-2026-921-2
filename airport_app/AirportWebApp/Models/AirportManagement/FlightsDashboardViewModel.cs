namespace AirportWebApp.Models.AirportManagement
{
    public class FlightsDashboardViewModel
    {
        public List<FlightSummary> Flights { get; set; } = new();
        public string SearchQuery { get; set; } = string.Empty;
        public int? EditFlightId { get; set; }
    }
}
