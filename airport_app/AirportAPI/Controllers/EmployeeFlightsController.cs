using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/employee-flights")]
public class EmployeeFlightsController : ControllerBase
{
    private readonly IEmployeeFlightRepository employeeFlightRepository;

    public EmployeeFlightsController(IEmployeeFlightRepository employeeFlightRepository)
    {
        this.employeeFlightRepository = employeeFlightRepository;
    }

    [HttpGet("employees/{employeeId:int}/flights")]
    public ActionResult<IEnumerable<int>> GetFlightsByEmployeeId(int employeeId)
    {
        return Ok(employeeFlightRepository.GetFlightsByEmployeeId(employeeId));
    }

    [HttpGet("flights/{flightId:int}/employees")]
    public ActionResult<IEnumerable<int>> GetEmployeesByFlightId(int flightId)
    {
        return Ok(employeeFlightRepository.GetEmployeesByFlightId(flightId));
    }

    [HttpPost]
    public IActionResult AssignFlightToEmployee(EmployeeFlightAssignmentRequest request)
    {
        try
        {
            employeeFlightRepository.AssignFlightToEmployeeUsingIds(request.EmployeeId, request.FlightId);
            return NoContent();
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpDelete("employees/{employeeId:int}/flights/{flightId:int}")]
    public IActionResult RemoveFlightFromEmployee(int employeeId, int flightId)
    {
        employeeFlightRepository.RemoveFlightFromEmployeeUsingIds(employeeId, flightId);
        return NoContent();
    }

    [HttpDelete("flights/{flightId:int}")]
    public IActionResult RemoveAllByFlightId(int flightId)
    {
        employeeFlightRepository.RemoveAllByFlightId(flightId);
        return NoContent();
    }

    [HttpDelete("employees/{employeeId:int}")]
    public IActionResult RemoveAllByEmployeeId(int employeeId)
    {
        employeeFlightRepository.RemoveAllByEmployeeId(employeeId);
        return NoContent();
    }
}

public sealed record EmployeeFlightAssignmentRequest(int EmployeeId, int FlightId);
