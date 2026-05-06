using Microsoft.EntityFrameworkCore;

namespace AirportApp.Data.Repositories
{
    public class EfEmployeeFlightRepository : IEmployeeFlightRepository
    {
        private readonly AppDbContext context;

        public EfEmployeeFlightRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void AssignFlightToEmployeeUsingIds(int employeeId, int flightId)
        {
            EmployeeFlight employeeFlight = new EmployeeFlight
            {
                EmployeeId = employeeId,
                FlightId = flightId,
                Employee = context.Employees.Find(employeeId) ?? throw new InvalidOperationException("Employee not found"),
                Flight = context.Flights.Find(flightId) ?? throw new InvalidOperationException("Flight not found")
            };

            context.EmployeeFlights.Add(employeeFlight);
            context.SaveChanges();
        }

        public void RemoveFlightFromEmployeeUsingIds(int employeeId, int flightId)
        {
            EmployeeFlight? employeeFlight = context.EmployeeFlights
                .FirstOrDefault(employeeFlight =>
                    employeeFlight.EmployeeId == employeeId &&
                    employeeFlight.FlightId == flightId);

            if (employeeFlight != null)
            {
                context.EmployeeFlights.Remove(employeeFlight);
                context.SaveChanges();
            }
        }

        public List<int> GetFlightsByEmployeeId(int employeeId)
        {
            return context.EmployeeFlights
                .Where(employeeFlight => employeeFlight.EmployeeId == employeeId)
                .Select(employeeFlight => employeeFlight.FlightId)
                .ToList();
        }

        public List<int> GetEmployeesByFlightId(int flightId)
        {
            return context.EmployeeFlights
                .Where(employeeFlight => employeeFlight.FlightId == flightId)
                .Select(employeeFlight => employeeFlight.EmployeeId)
                .ToList();
        }

        public void RemoveAllByFlightId(int flightId)
        {
            List<EmployeeFlight> employeeFlights = context.EmployeeFlights
                .Where(employeeFlight => employeeFlight.FlightId == flightId)
                .ToList();

            context.EmployeeFlights.RemoveRange(employeeFlights);
            context.SaveChanges();
        }

        public void RemoveAllByEmployeeId(int employeeId)
        {
            List<EmployeeFlight> employeeFlights = context.EmployeeFlights
                .Where(employeeFlight => employeeFlight.EmployeeId == employeeId)
                .ToList();

            context.EmployeeFlights.RemoveRange(employeeFlights);
            context.SaveChanges();
        }
    }
}