using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController(ICompanyRepository companyRepository) : ControllerBase
{
    private const string MissingCompanyDataErrorMessage = "Company data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Company>> GetAll()
    {
        return this.Ok(companyRepository.GetAllCompanies());
    }
    [HttpGet("{companyId:int}")]
    public ActionResult<Company> GetById(int companyId)
    {
        Company? company = companyRepository.GetCompanyById(companyId);

        if (company == null)
        {
            return this.NotFound();
        }

        return this.Ok(company);
    }

    [HttpPost]
    public ActionResult<Company> Add([FromBody] Company company)
    {
        if (company == null)
        {
            return this.BadRequest(MissingCompanyDataErrorMessage);
        }

        int companyId = companyRepository.AddCompany(company);

        return this.CreatedAtAction(nameof(this.GetById), new { companyId }, company);
    }

    [HttpPut("{companyId:int}")]
    public IActionResult Update(int companyId, [FromBody] Company company)
    {
        if (company == null)
        {
            return this.BadRequest(MissingCompanyDataErrorMessage);
        }

        if (companyRepository.GetCompanyById(companyId) == null)
        {
            return this.NotFound();
        }

        company.Id = companyId;
        companyRepository.UpdateCompany(company);

        return this.NoContent();
    }

    [HttpDelete("{companyId:int}")]
    public IActionResult Delete(int companyId)
    {
        if (companyRepository.GetCompanyById(companyId) == null)
        {
            return this.NotFound();
        }

        companyRepository.DeleteCompanyUsingId(companyId);

        return this.NoContent();
    }
}