namespace AirportApp.Data.Repositories.Proxies;

public class CompanyRepositoryProxy : RepositoryProxyBase, ICompanyRepository
{
    public CompanyRepositoryProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public List<Company> GetAllCompanies()
    {
        return this.GetList<CompanyDto>("api/companies")
            .Select(MapCompany)
            .ToList();
    }

    public Company? GetCompanyById(int companyId)
    {
        CompanyDto? company = this.GetOptional<CompanyDto>($"api/companies/{companyId}");
        return company == null ? null : MapCompany(company);
    }

    public int AddCompany(Company newCompany)
    {
        CompanyDto company = this.PostForResult<Company, CompanyDto>("api/companies", newCompany);
        return company.Id;
    }

    public void DeleteCompanyUsingId(int comapnyId)
    {
        this.Delete($"api/companies/{comapnyId}");
    }

    public void UpdateCompany(Company updatedCompany)
    {
        this.Put($"api/companies/{updatedCompany.Id}", updatedCompany);
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
}
