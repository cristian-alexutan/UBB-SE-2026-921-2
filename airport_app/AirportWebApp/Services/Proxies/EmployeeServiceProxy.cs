using AirportWebApp.Services.Proxies;

namespace AirportWebApp.Services.Proxies;

public class EmployeeServiceProxy : RepositoryProxyBase, IEmployeeService
{
    private sealed class SaveEmployeeDto
    {
        public Employee Employee { get; set; } = null!;
        public DateTimeOffset? Birthday { get; set; }
        public DateTimeOffset? HiringDate { get; set; }
        public string SalaryText { get; set; } = string.Empty;
    }

    public EmployeeServiceProxy(HttpClient httpClient) : base(httpClient)
    {
    }

    public List<Employee> GetAllEmployees()
    {
        return this.GetList<Employee>("api/employees");
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        return this.GetOptional<Employee>($"api/employees/{employeeId}");
    }

    public List<Employee> GetPilots()
    {
        return this.GetList<Employee>("api/employees/pilots");
    }

    public List<Employee> GetFlightAttendants()
    {
        return this.GetList<Employee>("api/employees/flight-attendants");
    }

    public List<Employee> GetCoPilots()
    {
        return this.GetList<Employee>("api/employees/co-pilots");
    }

    public List<Employee> GetFlightDispatchers()
    {
        return this.GetList<Employee>("api/employees/flight-dispatchers");
    }

    public void SaveEmployee(Employee editingEmployee, DateTimeOffset? birthday, DateTimeOffset? hiringDate, string salaryText)
    {
        var dto = new SaveEmployeeDto
        {
            Employee = editingEmployee,
            Birthday = birthday,
            HiringDate = hiringDate,
            SalaryText = salaryText
        };

        this.Post("api/employees/save", dto);
    }

    public int AddEmployee(string name, EmployeeRole role, DateOnly birthday, int salary, DateOnly hiringDate)
    {
        var employee = new Employee
        {
            Name = name,
            Role = role,
            Birthday = birthday,
            Salary = salary,
            HiringDate = hiringDate
        };

        return this.PostForResult<Employee, int>("api/employees", employee);
    }

    public void UpdateEmployee(int id, string? name = null, EmployeeRole? role = null, int? salary = null,
        DateOnly? birthday = null, DateOnly? hiringDate = null)
    {
        var employee = new Employee
        {
            Id = id,
            Name = name ?? string.Empty,
            Role = role ?? EmployeeRole.Other,
            Salary = salary ?? 0,
            Birthday = birthday ?? DateOnly.MinValue,
            HiringDate = hiringDate ?? DateOnly.MinValue
        };

        this.Put($"api/employees/{id}", employee);
    }

    public void DeleteEmployeeUsingId(int employeeId)
    {
        this.Delete($"api/employees/{employeeId}");
    }

    public void DeleteWithAssignments(int employeeId)
    {
        this.Delete($"api/employees/{employeeId}/with-assignments");
    }

    public EmployeeRole ParseRole(string roleText)
    {
        return this.GetRequired<EmployeeRole>($"api/employees/parse-role?roleText={Uri.EscapeDataString(roleText)}");
    }

    public int Login(string employeeIdText)
    {
        return this.GetRequired<int>($"api/employees/login?employeeIdText={Uri.EscapeDataString(employeeIdText)}");
    }
}