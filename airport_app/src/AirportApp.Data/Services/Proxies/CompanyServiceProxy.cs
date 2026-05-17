using AirportApp.Data.Repositories.Proxies;

namespace AirportApp.Data.Services.Proxies;

public class CompanyServiceProxy : RepositoryProxyBase, ICompanyService
{
    public CompanyServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Company> GetAllCompanies()
    {
        return this.GetList<CompanyDto>("api/companies")
            .Select(MapCompany)
            .ToList();
    }

    public Company? GetCompanyById(int id)
    {
        CompanyDto? company = this.GetOptional<CompanyDto>($"api/companies/{id}");
        return company == null ? null : MapCompany(company);
    }

    public int AddCompany(string companyName)
    {
        return this.PostForResult<string, int>("api/companies", companyName);
    }

    public string GenerateFlightCodeUsingCompanyId(int companyId)
    {
        return this.GetRequired<string>($"api/companies/{companyId}/flight-code");
    }

    public void UpdateCompany(int companyId, string? newName = null)
    {
        this.Put($"api/companies/{companyId}", newName);
    }

    public void DeleteCompanyUsingId(int companyId)
    {
        this.Delete($"api/companies/{companyId}");
    }

    public int ValidateFlightCreationInputs(int companyId, int airportId, string capacityText, int runwayId, int gateId)
    {
        return this.PostForResult<FlightValidationRequest, int>("api/companies/validate-flight", new FlightValidationRequest
        {
            CompanyId = companyId,
            AirportId = airportId,
            CapacityText = capacityText,
            RunwayId = runwayId,
            GateId = gateId
        });
    }

    private static Company MapCompany(CompanyDto company)
    {
        return new Company
        {
            Id = company.Id,
            Name = company.Name ?? string.Empty
        };
    }

    private sealed class CompanyDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }

    private sealed class FlightValidationRequest
    {
        public int CompanyId { get; set; }

        public int AirportId { get; set; }

        public string CapacityText { get; set; } = string.Empty;

        public int RunwayId { get; set; }

        public int GateId { get; set; }
    }
}
