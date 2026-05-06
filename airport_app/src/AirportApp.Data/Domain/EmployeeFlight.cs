namespace AirportApp.Data.Domain
{
    public class EmployeeFlight
    {
        public Employee Employee { get; set; }
        public Flight Flight { get; set; }

        public int EmployeeId { get; set; }
        public int FlightId { get; set; }
        internal EmployeeFlight()
        {
        }
    }
}
