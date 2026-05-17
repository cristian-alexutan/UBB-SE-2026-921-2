using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/companies")]
public class CompaniesController(ICompanyService companyService) : ControllerBase
{
    private const string EmptyCompanyNameErrorMessage = "The company name cannot be empty.";

    [HttpGet]
    public ActionResult<IEnumerable<Company>> GetAll()
    {
        return this.Ok(companyService.GetAllCompanies());
    }
    [HttpGet("{companyId:int}")]
    public ActionResult<Company> GetById(int companyId)
    {
        Company? company = companyService.GetCompanyById(companyId);

        if (company == null)
        {
            return this.NotFound();
        }

        return this.Ok(company);
    }

    [HttpPost]
    public ActionResult<int> Add([FromBody] string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            return this.BadRequest(EmptyCompanyNameErrorMessage);
        }

        int newCompanyId = companyService.AddCompany(companyName);

        return this.Ok(newCompanyId);
    }

    [HttpPut("{companyId:int}")]
    public IActionResult Update(int companyId, [FromBody] string updatedCompanyName)
    {
        if (companyService.GetCompanyById(companyId) == null)
        {
            return this.NotFound();
        }

        if (string.IsNullOrWhiteSpace(updatedCompanyName))
        {
            return this.BadRequest(EmptyCompanyNameErrorMessage);
        }

        companyService.UpdateCompany(companyId, updatedCompanyName);

        return this.NoContent();
    }

    [HttpDelete("{companyId:int}")]
    public IActionResult Delete(int companyId)
    {
        if (companyService.GetCompanyById(companyId) == null)
        {
            return this.NotFound();
        }

        companyService.DeleteCompanyUsingId(companyId);

        return this.NoContent();
    }

    [HttpGet("{companyId:int}/flight-code")]
    public ActionResult<string> GenerateFlightCode(int companyId)
    {
        return this.Ok(companyService.GenerateFlightCodeUsingCompanyId(companyId));
    }

    [HttpPost("validate-flight")]
    public ActionResult<int> ValidateFlightInputs([FromBody] FlightValidationRequest request)
    {
        if (request == null)
        {
            return this.BadRequest();
        }

        int parsedCapacity = companyService.ValidateFlightCreationInputs(
            request.CompanyId,
            request.AirportId,
            request.CapacityText ?? string.Empty,
            request.RunwayId,
            request.GateId);

        return this.Ok(parsedCapacity);
    }

    public sealed class FlightValidationRequest
    {
        public int CompanyId { get; set; }

        public int AirportId { get; set; }

        public string? CapacityText { get; set; }

        public int RunwayId { get; set; }

        public int GateId { get; set; }
    }
}