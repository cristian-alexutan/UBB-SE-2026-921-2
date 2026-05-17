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
            newEmployee.Id = 0;
            databaseContext.Employees.Add(newEmployee);
            databaseContext.SaveChanges();

            return newEmployee.Id;
        }

        public void UpdateEmployee(Employee updatedEmployee)
        {
            Employee? existingEmployee = databaseContext.Employees.Find(updatedEmployee.Id);
            if (existingEmployee == null)
            {
                return;
            }

            existingEmployee.Name = updatedEmployee.Name;
            existingEmployee.Role = updatedEmployee.Role;
            existingEmployee.Birthday = updatedEmployee.Birthday;
            existingEmployee.Salary = updatedEmployee.Salary;
            existingEmployee.HiringDate = updatedEmployee.HiringDate;
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
