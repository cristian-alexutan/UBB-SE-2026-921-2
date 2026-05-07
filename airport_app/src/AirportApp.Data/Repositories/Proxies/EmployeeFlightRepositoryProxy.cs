namespace AirportApp.Data.Repositories.Proxies;

public class EmployeeFlightRepositoryProxy : RepositoryProxyBase, IEmployeeFlightRepository
{
    public EmployeeFlightRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public void AssignFlightToEmployeeUsingIds(int employeeId, int flightId)
    {
        this.Post("api/employee-flights", new EmployeeFlightAssignmentRequest(employeeId, flightId));
    }

    public void RemoveFlightFromEmployeeUsingIds(int employeeId, int flightId)
    {
        this.Delete($"api/employee-flights/employees/{employeeId}/flights/{flightId}");
    }

    public List<int> GetFlightsByEmployeeId(int employeeId)
    {
        return this.GetList<int>($"api/employee-flights/employees/{employeeId}/flights");
    }

    public List<int> GetEmployeesByFlightId(int flightId)
    {
        return this.GetList<int>($"api/employee-flights/flights/{flightId}/employees");
    }

    public void RemoveAllByFlightId(int flightId)
    {
        this.Delete($"api/employee-flights/flights/{flightId}");
    }

    public void RemoveAllByEmployeeId(int employeeId)
    {
        this.Delete($"api/employee-flights/employees/{employeeId}");
    }

    private sealed record EmployeeFlightAssignmentRequest(int EmployeeId, int FlightId);
}
