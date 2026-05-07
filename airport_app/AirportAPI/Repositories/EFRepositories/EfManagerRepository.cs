using AirportAPI;
using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories;

public class EfManagerRepository : IManagerRepository
{
    private readonly AppDbContext dbContext;
    public EfManagerRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<Manager> GetAll()
    {
        return this.dbContext.Managers.ToList();
    }

    public Manager? GetById(int managerId)
    {
        return this.dbContext.Managers.Find(managerId);
    }

    public void Add(Manager manager)
    {
        this.dbContext.Managers.Add(manager);
        this.dbContext.SaveChanges();
    }

    public Manager? Delete(int managerId)
    {
        var manager = this.dbContext.Managers.Find(managerId);
        if (manager == null)
        {
            return null;
        }
        this.dbContext.Managers.Remove(manager);
        this.dbContext.SaveChanges();
        return manager;
    }

    public Manager? Update(Manager manager)
    {
        this.dbContext.Managers.Update(manager);
        this.dbContext.SaveChanges();
        return manager;
    }
}




