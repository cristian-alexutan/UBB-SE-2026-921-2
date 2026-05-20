namespace AirportWebApp.Models.AirportManagement
{
    public enum AdminSection
    {
        Flights,
        Employees,
        Configuration,
    }

    public class AirportAdminViewModel
    {
        public AdminSection ActiveSection { get; set; } = AdminSection.Flights;
        public FlightsDashboardViewModel FlightsDashboard { get; set; } = new();
        public EmployeesDashboardViewModel EmployeesDashboard { get; set; } = new();
    }
}
