namespace AirportLib.Domain.DTOs;

public sealed class AssignEmployeeDto
{
    public int FlightId { get; set; }
    public int EmployeeId { get; set; }
}