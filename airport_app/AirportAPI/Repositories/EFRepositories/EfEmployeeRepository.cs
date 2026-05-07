using AirportAPI.Repositories.Interfaces;

namespace AirportAPI.Repositories
{
    public class EfEmployeeRepository(AppDbContext databaseContext) : IEmployeeRepository
    {
        public List<Employee> GetAllEmployees()
        {
            return databaseContext.Employees.ToList();
        }

        public Employee? GetEmployeeById(int employeeId)
        {
            return databaseContext.Employees.Find(employeeId);
        }

        public int AddEmployee(Employee newEmployee)
        {
            databaseContext.Employees.Add(newEmployee);
            databaseContext.SaveChanges();

            return newEmployee.Id;
        }

        public void UpdateEmployee(Employee updatedEmployee)
        {
            databaseContext.Employees.Update(updatedEmployee);
            databaseContext.SaveChanges();
        }

        public void DeleteEmployee(int employeeId)
        {
            Employee? employeeToRemove = this.GetEmployeeById(employeeId);

            if (employeeToRemove == null)
            {
                return;
            }

            databaseContext.Employees.Remove(employeeToRemove);
            databaseContext.SaveChanges();
        }
    }
}