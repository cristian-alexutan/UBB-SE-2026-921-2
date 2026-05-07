using Microsoft.EntityFrameworkCore;

namespace AirportApp.Data.Repositories
{
    public class EfEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public EfEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }
        public List<Employee> GetAllEmployees()
        {
            return context.Employees.ToList();
        }

        public Employee? GetEmployeeById(int employeeId)
        {
            return context.Employees
                .FirstOrDefault(employee => employee.Id == employeeId);
        }

        public int AddEmployee(Employee newEmployee)
        {
            context.Employees.Add(newEmployee);
            context.SaveChanges();

            return newEmployee.Id;
        }

        public void UpdateEmployee(Employee updatedEmployee)
        {
            context.Employees.Update(updatedEmployee);
            context.SaveChanges();
        }

        public void DeleteEmployee(int employeeId)
        {
            Employee? employeeToDelete = context.Employees
                .FirstOrDefault(employee => employee.Id == employeeId);

            if (employeeToDelete != null)
            {
                context.Employees.Remove(employeeToDelete);
                context.SaveChanges();
            }
        }
    }
}