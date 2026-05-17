using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeRepository employeeRepository) : ControllerBase
{
    private const string NullEmployeeDataErrorMessage = "Employee data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        return this.Ok(employeeRepository.GetAllEmployees());
    }

    [HttpGet("{employeeId:int}")]
    public ActionResult<Employee> GetById(int employeeId)
    {
        Employee? employee = employeeRepository.GetEmployeeById(employeeId);

        if (employee == null)
        {
            return this.NotFound();
        }

        return this.Ok(employee);
    }

    [HttpPost]
    public ActionResult<Employee> Add([FromBody] Employee employee)
    {
        if (employee == null)
        {
            return this.BadRequest(NullEmployeeDataErrorMessage);
        }

        int employeeId = employeeRepository.AddEmployee(employee);

        return this.CreatedAtAction(nameof(this.GetById), new { employeeId }, employee);
    }

    [HttpPut("{employeeId:int}")]
    public IActionResult Update(int employeeId, [FromBody] Employee employee)
    {
        if (employee == null)
        {
            return this.BadRequest(NullEmployeeDataErrorMessage);
        }

        if (employeeRepository.GetEmployeeById(employeeId) == null)
        {
            return this.NotFound();
        }

        employee.Id = employeeId;
        employeeRepository.UpdateEmployee(employee);

        return this.NoContent();
    }

    [HttpDelete("{employeeId:int}")]
    public IActionResult Delete(int employeeId)
    {
        if (employeeRepository.GetEmployeeById(employeeId) == null)
        {
            return this.NotFound();
        }

        employeeRepository.DeleteEmployee(employeeId);

        return this.NoContent();
    }
}