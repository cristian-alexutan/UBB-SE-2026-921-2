using AirportLib.Domain.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/employee-flights")]
public class EmployeeFlightsController(IEmployeeFlightService employeeFlightService) : ControllerBase
{
    private const string NullFlightDataErrorMessage = "Flight data cannot be null.";
    private const string NullAssignmentDataErrorMessage = "Assignment data cannot be null.";

    [HttpGet("flights/{flightId:int}/employees")]
    public ActionResult<IEnumerable<Employee>> GetEmployeesAssignedToFlight(int flightId)
    {
        return this.Ok(employeeFlightService.GetEmployeesAssignedToFlight(flightId));
    }

    [HttpGet("employees/{employeeId:int}/schedule")]
    public ActionResult<IEnumerable<Flight>> GetEmployeeSchedule(int employeeId)
    {
        return this.Ok(employeeFlightService.GetEmployeeSchedule(employeeId));
    }

    [HttpGet("employees/{employeeId:int}/formatted-schedule")]
    public ActionResult<IEnumerable<EmployeeScheduleItem>> GetFormattedEmployeeSchedule(int employeeId)
    {
        return this.Ok(employeeFlightService.GetFormattedEmployeeSchedule(employeeId));
    }

    [HttpGet("employees/{employeeId:int}/available")]
    public ActionResult<bool> IsEmployeeAvailable(int employeeId, [FromQuery] DateTime targetDate,
        [FromQuery] int targetRouteId, [FromQuery] int? excludedFlightId)
    {
        return this.Ok(employeeFlightService.IsEmployeeAvailable(employeeId, targetDate, targetRouteId, excludedFlightId));
    }

    [HttpGet("flights/{flightId:int}/crew-list")]
    public ActionResult<string> FormatCrewList(int flightId)
    {
        return this.Ok(new { result = employeeFlightService.FormatCrewList(flightId) });
    }

    [HttpPost("assign")]
    public IActionResult AssignEmployeeToFlight([FromBody] AssignEmployeeDto dto)
    {
        if (dto == null)
        {
            return this.BadRequest(NullAssignmentDataErrorMessage);
        }

        employeeFlightService.AssignEmployeeToFlightUsingIds(dto.FlightId, dto.EmployeeId);

        return this.NoContent();
    }

    [HttpPost("flights/{flightId:int}/employees")]
    public IActionResult AssignEmployeesToFlight(int flightId, [FromBody] List<int> employeeIds)
    {
        if (employeeIds == null)
        {
            return this.BadRequest(NullAssignmentDataErrorMessage);
        }

        employeeFlightService.AssignEmpolyeesToFlightUsingIds(flightId, employeeIds);

        return this.NoContent();
    }

    [HttpPut("flights/{flightId:int}/employees")]
    public IActionResult UpdateEmployeesForFlight(int flightId, [FromBody] List<int> updatedEmployeeIds)
    {
        if (updatedEmployeeIds == null)
        {
            return this.BadRequest(NullAssignmentDataErrorMessage);
        }

        employeeFlightService.UpdateEmployeesForFlightUsingIds(flightId, updatedEmployeeIds);

        return this.NoContent();
    }

    [HttpGet("flights/{flightId:int}/available-employees")]
    public ActionResult<IEnumerable<Employee>> GetAvailableEmployeesGroupedByRole(int flightId)
    {
        return this.Ok(employeeFlightService.GetAvailableEmployeesGroupedByRoleById(flightId));
    }

    [HttpGet("flights/{flightId:int}/crew-selection-data")]
    public ActionResult<IEnumerable<CrewMemberSelectionData>> GetCrewSelectionData(int flightId)
    {
        return this.Ok(employeeFlightService.GetCrewSelectionDataById(flightId));
    }

    [HttpDelete("flights/{flightId:int}/employees/{employeeId:int}")]
    public IActionResult RemoveEmployeeFromFlight(int flightId, int employeeId)
    {
        employeeFlightService.RemoveEmployeeFromFlightUsingIds(flightId, employeeId);

        return this.NoContent();
    }

    [HttpDelete("flights/{flightId:int}")]
    public IActionResult RemoveAllCrewAssignmentsForFlight(int flightId)
    {
        employeeFlightService.RemoveAllCrewAssignmentsForFlight(flightId);

        return this.NoContent();
    }

    [HttpDelete("employees/{employeeId:int}")]
    public IActionResult RemoveAllFlightsAssignmentsForEmployee(int employeeId)
    {
        employeeFlightService.RemoveAllFlightsAssignmentsForEmployee(employeeId);

        return this.NoContent();
    }
}