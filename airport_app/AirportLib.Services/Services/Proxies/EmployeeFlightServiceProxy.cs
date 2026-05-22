using AirportLib.Services.Services.Interfaces;

namespace AirportLib.Services.Services.Proxies;

public class EmployeeFlightServiceProxy : ServiceProxyBase, IEmployeeFlightService
{
    private sealed class AssignDto
    {
        public int FlightId { get; set; }
        public int EmployeeId { get; set; }
    }

    private sealed class CrewListDto
    {
        public string? Result { get; set; }
    }

    public EmployeeFlightServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public List<Employee> GetEmployeesAssignedToFlight(int flightId)
    {
        return this.GetList<Employee>($"api/employee-flights/flights/{flightId}/employees");
    }

    public List<Flight> GetEmployeeSchedule(int employeeId)
    {
        return this.GetList<Flight>($"api/employee-flights/employees/{employeeId}/schedule");
    }

    public List<EmployeeScheduleItem> GetFormattedEmployeeSchedule(int employeeId)
    {
        return this.GetList<EmployeeScheduleItem>($"api/employee-flights/employees/{employeeId}/formatted-schedule");
    }

    public bool IsEmployeeAvailable(int employeeId, DateTime targetDate, int targetRouteId, int? excludedFlightId)
    {
        string url = $"api/employee-flights/employees/{employeeId}/available?targetDate=" +
            $"{Uri.EscapeDataString(targetDate.ToString("o"))}&targetRouteId={targetRouteId}";

        if (excludedFlightId.HasValue)
        {
            url += $"&excludedFlightId={excludedFlightId.Value}";
        }

        return this.GetRequired<bool>(url);
    }

    public string FormatCrewList(int flightId)
    {
        CrewListDto? dto = this.GetOptional<CrewListDto>($"api/employee-flights/flights/{flightId}/crew-list");
        return dto?.Result ?? string.Empty;
    }

    public void AssignEmployeeToFlightUsingIds(int flightId, int employeeId)
    {
        var dto = new AssignDto
        {
            FlightId = flightId,
            EmployeeId = employeeId
        };
        this.Post("api/employee-flights/assign", dto);
    }

    public void AssignEmpolyeesToFlightUsingIds(int flightId, List<int> employeeIds)
    {
        this.Post($"api/employee-flights/flights/{flightId}/employees", employeeIds);
    }

    public void UpdateEmployeesForFlightUsingIds(int flightId, List<int> updatedEmployeeIds)
    {
        this.Put($"api/employee-flights/flights/{flightId}/employees", updatedEmployeeIds);
    }

    public List<Employee> GetAvailableEmployeesGroupedByRole(Flight flight)
    {
        return this.GetAvailableEmployeesGroupedByRoleById(flight.Id);
    }

    public List<Employee> GetAvailableEmployeesGroupedByRoleById(int flightId)
    {
        return this.GetList<Employee>($"api/employee-flights/flights/{flightId}/available-employees");
    }

    public List<CrewMemberSelectionData> GetCrewSelectionData(Flight flight)
    {
        return this.GetCrewSelectionDataById(flight.Id);
    }

    public List<CrewMemberSelectionData> GetCrewSelectionDataById(int flightId)
    {
        return this.GetList<CrewMemberSelectionData>($"api/employee-flights/flights/{flightId}/crew-selection-data");
    }

    public void RemoveEmployeeFromFlightUsingIds(int flightId, int employeeId)
    {
        this.Delete($"api/employee-flights/flights/{flightId}/employees/{employeeId}");
    }

    public void RemoveAllCrewAssignmentsForFlight(int flightId)
    {
        this.Delete($"api/employee-flights/flights/{flightId}");
    }

    public void RemoveAllFlightsAssignmentsForEmployee(int employeeId)
    {
        this.Delete($"api/employee-flights/employees/{employeeId}");
    }
}
