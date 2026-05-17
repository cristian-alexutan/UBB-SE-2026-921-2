using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
// explicit string to force hyphen
[Route("api/employee-flights")]
public class EmployeeFlightsController(IEmployeeFlightRepository employeeFlightRepository) : ControllerBase
{
    [HttpGet]
    public ActionResult<string> GetInfo()
    {
        return this.Ok("Employee-Flights Assignment API is active.");
    }

    [HttpGet("employees/{employeeId:int}/flights")]
    public ActionResult<IEnumerable<int>> GetFlightsByEmployeeId(int employeeId)
    {
        return this.Ok(employeeFlightRepository.GetFlightsByEmployeeId(employeeId));
    }

    [HttpGet("flights/{flightId:int}/employees")]
    public ActionResult<IEnumerable<int>> GetEmployeesByFlightId(int flightId)
    {
        return this.Ok(employeeFlightRepository.GetEmployeesByFlightId(flightId));
    }

    [HttpPost]
    public IActionResult AssignFlightToEmployee([FromBody] EmployeeFlightAssignmentRequest request)
    {
        if (request == null)
        {
            return this.BadRequest("Assignment request data cannot be null.");
        }

        try
        {
            employeeFlightRepository.AssignFlightToEmployeeUsingIds(request.EmployeeId, request.FlightId);
            return this.NoContent();
        }
        catch (InvalidOperationException exception)
        {
            return this.NotFound(exception.Message);
        }
    }

    [HttpDelete("employees/{employeeId:int}/flights/{flightId:int}")]
    public IActionResult RemoveFlightFromEmployee(int employeeId, int flightId)
    {
        employeeFlightRepository.RemoveFlightFromEmployeeUsingIds(employeeId, flightId);
        return this.NoContent();
    }

    [HttpDelete("flights/{flightId:int}")]
    public IActionResult RemoveAllByFlightId(int flightId)
    {
        employeeFlightRepository.RemoveAllByFlightId(flightId);
        return this.NoContent();
    }

    [HttpDelete("employees/{employeeId:int}")]
    public IActionResult RemoveAllByEmployeeId(int employeeId)
    {
        employeeFlightRepository.RemoveAllByEmployeeId(employeeId);
        return this.NoContent();
    }
}

public sealed record EmployeeFlightAssignmentRequest(int EmployeeId, int FlightId);