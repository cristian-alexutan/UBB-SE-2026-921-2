using AirportAPI.DTOs;
using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    private const string NullEmployeeDataErrorMessage = "Employee data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetAllEmployees()
    {
        return this.Ok(employeeService.GetAllEmployees());
    }

    [HttpGet("{employeeId:int}")]
    public ActionResult<Employee> GetEmployeeById(int employeeId)
    {
        Employee? employee = employeeService.GetEmployeeById(employeeId);

        if (employee == null)
        {
            return this.NotFound();
        }

        return this.Ok(employee);
    }

    [HttpGet("pilots")]
    public ActionResult<IEnumerable<Employee>> GetPilots()
    {
        return this.Ok(employeeService.GetPilots());
    }

    [HttpGet("flight-attendants")]
    public ActionResult<IEnumerable<Employee>> GetFlightAttendants()
    {
        return this.Ok(employeeService.GetFlightAttendants());
    }

    [HttpGet("co-pilots")]
    public ActionResult<IEnumerable<Employee>> GetCoPilots()
    {
        return this.Ok(employeeService.GetCoPilots());
    }

    [HttpGet("flight-dispatchers")]
    public ActionResult<IEnumerable<Employee>> GetFlightDispatchers()
    {
        return this.Ok(employeeService.GetFlightDispatchers());
    }

    [HttpPost]
    public ActionResult<Employee> AddEmployee([FromBody] Employee employee)
    {
        if (employee == null)
        {
            return this.BadRequest(NullEmployeeDataErrorMessage);
        }

        int employeeId = employeeService.AddEmployee(employee.Name, employee.Role, employee.Birthday, employee.Salary, employee.HiringDate);

        return this.CreatedAtAction(nameof(this.GetEmployeeById), new { employeeId }, employee);
    }

    [HttpPut("{employeeId:int}")]
    public IActionResult UpdateEmployee(int employeeId, [FromBody] Employee employee)
    {
        if (employee == null)
        {
            return this.BadRequest(NullEmployeeDataErrorMessage);
        }

        if (employeeService.GetEmployeeById(employeeId) == null)
        {
            return this.NotFound();
        }

        employee.Id = employeeId;
        employeeService.UpdateEmployee(employee.Id, employee.Name, employee.Role, employee.Salary, employee.Birthday, employee.HiringDate);

        return this.NoContent();
    }

    [HttpDelete("{employeeId:int}")]
    public IActionResult DeleteEmployeeUsingId(int employeeId)
    {
        if (employeeService.GetEmployeeById(employeeId) == null)
        {
            return this.NotFound();
        }

        employeeService.DeleteEmployeeUsingId(employeeId);

        return this.NoContent();
    }

    [HttpDelete("{employeeId:int}/with-assignments")]
    public IActionResult DeleteWithAssignments(int employeeId)
    {
        if (employeeService.GetEmployeeById(employeeId) == null)
        {
            return this.NotFound();
        }

        employeeService.DeleteWithAssignments(employeeId);

        return this.NoContent();
    }

    [HttpPost("save")]
    public IActionResult Save([FromBody] SaveEmployeeDto dto)
    {
        if (dto?.Employee == null)
        {
            return this.BadRequest(NullEmployeeDataErrorMessage);
        }

        employeeService.SaveEmployee(dto.Employee, dto.Birthday, dto.HiringDate, dto.SalaryText);

        return this.NoContent();
    }

    [HttpGet("parse-role")]
    public ActionResult<EmployeeRole> ParseRole([FromQuery] string roleText)
    {
        return this.Ok(employeeService.ParseRole(roleText));
    }

    [HttpGet("login")]
    public ActionResult<int> Login([FromQuery] string employeeIdText)
    {
        int employeeId = employeeService.Login(employeeIdText);

        return this.Ok(employeeId);
    }
}