using System.ComponentModel.DataAnnotations;
using AirportWebApp.Domain;

namespace AirportWebApp.Models.AirportManagement
{
    public class EmployeesDashboardViewModel
    {
        public List<Employee> Employees { get; set; } = new();
        public Employee? EditEmployee { get; set; }
    }

    public class EmployeeFormModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
