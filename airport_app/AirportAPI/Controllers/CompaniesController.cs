using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyRepository companyRepository;

    public CompaniesController(ICompanyRepository companyRepository)
    {
        this.companyRepository = companyRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Company>> GetAll()
    {
        return Ok(companyRepository.GetAllCompanies());
    }

    [HttpGet("{companyId:int}")]
    public ActionResult<Company> GetById(int companyId)
    {
        Company? company = companyRepository.GetCompanyById(companyId);
        return company == null ? NotFound() : Ok(company);
    }

    [HttpPost]
    public ActionResult<Company> Add(Company company)
    {
        int companyId = companyRepository.AddCompany(company);
        return CreatedAtAction(nameof(GetById), new { companyId }, company);
    }

    [HttpPut("{companyId:int}")]
    public IActionResult Update(int companyId, Company company)
    {
        if (companyRepository.GetCompanyById(companyId) == null)
        {
            return NotFound();
        }

        company.Id = companyId;
        companyRepository.UpdateCompany(company);
        return NoContent();
    }

    [HttpDelete("{companyId:int}")]
    public IActionResult Delete(int companyId)
    {
        if (companyRepository.GetCompanyById(companyId) == null)
        {
            return NotFound();
        }

        companyRepository.DeleteCompanyUsingId(companyId);
        return NoContent();
    }
}
