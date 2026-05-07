using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        return Ok(employeeRepository.GetAllEmployees());
    }

    [HttpGet("{employeeId:int}")]
    public ActionResult<Employee> GetById(int employeeId)
    {
        Employee? employee = employeeRepository.GetEmployeeById(employeeId);
        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    public ActionResult<Employee> Add(Employee employee)
    {
        int employeeId = employeeRepository.AddEmployee(employee);
        return CreatedAtAction(nameof(GetById), new { employeeId }, employee);
    }

    [HttpPut("{employeeId:int}")]
    public IActionResult Update(int employeeId, Employee employee)
    {
        if (employeeRepository.GetEmployeeById(employeeId) == null)
        {
            return NotFound();
        }

        employee.Id = employeeId;
        employeeRepository.UpdateEmployee(employee);
        return NoContent();
    }

    [HttpDelete("{employeeId:int}")]
    public IActionResult Delete(int employeeId)
    {
        if (employeeRepository.GetEmployeeById(employeeId) == null)
        {
            return NotFound();
        }

        employeeRepository.DeleteEmployee(employeeId);
        return NoContent();
    }
}
