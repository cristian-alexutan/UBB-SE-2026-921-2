using AirportAPI.Repositories.Interfaces;
namespace AirportAPI.Repositories
{
    public class EfCompanyRepository(AppDbContext databaseContext) : ICompanyRepository
    {
        public List<Company> GetAllCompanies()
        {
            return databaseContext.Companies.ToList();
        }

        public Company? GetCompanyById(int companyId)
        {
            return databaseContext.Companies.Find(companyId);
        }

        public int AddCompany(Company newCompany)
        {
            databaseContext.Companies.Add(newCompany);

            databaseContext.SaveChanges();

            return newCompany.Id;
        }

        public void UpdateCompany(Company companyToUpdate)
        {
            databaseContext.Companies.Update(companyToUpdate);
            databaseContext.SaveChanges();
        }

        public void DeleteCompanyUsingId(int companyId)
        {
            Company? companyToRemove = this.GetCompanyById(companyId);

            if (companyToRemove != null)
            {
                databaseContext.Companies.Remove(companyToRemove);
                databaseContext.SaveChanges();
            }
        }
    }
}


