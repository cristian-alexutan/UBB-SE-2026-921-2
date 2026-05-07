namespace AirportApp.Data.Repositories.Proxies;

public class EmployeeRepositoryProxy : RepositoryProxyBase, IEmployeeRepository
{
    public EmployeeRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Employee> GetAllEmployees()
    {
        return this.GetList<EmployeeDto>("api/employees")
            .Select(MapEmployee)
            .ToList();
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        EmployeeDto? dto = this.GetOptional<EmployeeDto>($"api/employees/{employeeId}");
        return dto == null ? null : MapEmployee(dto);
    }

    public int AddEmployee(Employee newEmployee)
    {
        EmployeeDto result = this.PostForResult<Employee, EmployeeDto>("api/employees", newEmployee);
        return result.Id;
    }

    public void UpdateEmployee(Employee updatedEmployee)
    {
        this.Put($"api/employees/{updatedEmployee.Id}", updatedEmployee);
    }

    public void DeleteEmployee(int employeeId)
    {
        this.Delete($"api/employees/{employeeId}");
    }

    private static Employee MapEmployee(EmployeeDto dto)
    {
        return new Employee
        {
            Id = dto.Id,
            Name = dto.Name ?? string.Empty,
            Role = dto.Role,
            Birthday = dto.Birthday,
            HiringDate = dto.HiringDate,
            Salary = dto.Salary
        };
    }

    private sealed class EmployeeDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public EmployeeRole Role { get; set; }

        public DateOnly Birthday { get; set; }

        public DateOnly HiringDate { get; set; }

        public int Salary { get; set; }
    }
}
