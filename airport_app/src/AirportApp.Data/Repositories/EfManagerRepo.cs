using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace AirportApp.Data.Repositories;

public class EfManagerRepo : IManagerRepo
{
    private readonly AppDbContext dbContext;
    public EfManagerRepo(AppDbContext dbContext)
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
