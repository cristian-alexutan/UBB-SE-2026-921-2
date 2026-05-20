using System.ComponentModel.DataAnnotations;

namespace AirportWebApp.Models.AirportManagement
{
    public class AddFlightFormModel
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        public string RouteType { get; set; } = string.Empty;

        [Required]
        public int AirportId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1.")]
        public int Capacity { get; set; }

        [Required]
        [Range(0, 1439, ErrorMessage = "Departure time must be a valid minute offset.")]
        public int DepartureOffsetMinutes { get; set; }

        [Required]
        [Range(0, 1439, ErrorMessage = "Arrival time must be a valid minute offset.")]
        public int ArrivalOffsetMinutes { get; set; }

        public bool IsRecurrent { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? SingleDate { get; set; }

        public string RecurrenceType { get; set; } = "Daily";

        public string? CustomDaysText { get; set; }

        [Required]
        public int RunwayId { get; set; }

        [Required]
        public int GateId { get; set; }

        [Required]
        public string FlightNumberPrefix { get; set; } = string.Empty;

        // Supporting data for dropdowns
        public List<Airport> Airports { get; set; } = new();
        public List<Runway> Runways { get; set; } = new();
        public List<Gate> Gates { get; set; } = new();
    }
}
