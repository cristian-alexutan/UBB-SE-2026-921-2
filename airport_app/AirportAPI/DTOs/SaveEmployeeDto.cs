namespace AirportAPI.DTOs;

public sealed class SaveEmployeeDto
{
    public Employee Employee { get; set; } = null!;
    public DateTimeOffset? Birthday { get; set; }
    public DateTimeOffset? HiringDate { get; set; }
    public string SalaryText { get; set; } = string.Empty;
}
