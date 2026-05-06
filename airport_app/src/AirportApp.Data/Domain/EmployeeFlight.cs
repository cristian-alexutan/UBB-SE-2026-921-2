namespace AirportApp.Data.Domain
{
    public class EmployeeFlight
    {
        public Employee Employee { get; set; }
        public Flight Flight { get; set; }
        internal EmployeeFlight()
        {
        }
    }
}
