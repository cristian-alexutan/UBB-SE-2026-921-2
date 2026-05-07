using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using AirportApp.Data;
using AirportApp.Data.Domain;
using AirportApp.Data.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportApp.Data.Repositories;

public class EfRunwayRepository : IRunwayRepository
{
    private readonly AppDbContext dbContext;
    public EfRunwayRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public List<Runway> GetAllRunways()
    {
        return dbContext.Runways.ToList();
    }

    public Runway? GetRunwayById(int runwayId)
    {
        return dbContext.Runways.Find(runwayId);
    }

    public int AddRunway(Runway newRunway)
    {
        dbContext.Runways.Add(newRunway);
        dbContext.SaveChanges();
        return newRunway.Id;
    }

    public void UpdateRunway(Runway updatedRunway)
    {
        dbContext.Runways.Update(updatedRunway);
        dbContext.SaveChanges();
    }

    public void DeleteRunwayUsingId(int runwayId)
    {
        var relatedFlights = dbContext.Flights.Where(f => EF.Property<int>(f, "RunwayId") == runwayId).ToList();

        if (relatedFlights.Any())
        {
            dbContext.Flights.RemoveRange(relatedFlights);
        }

        var runwayToDelete = dbContext.Runways.Find(runwayId);
        if (runwayToDelete != null)
        {
            try
            {
                dbContext.Runways.Remove(runwayToDelete);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                foreach (var flight in relatedFlights)
                {
                    dbContext.Entry(flight).State = EntityState.Unchanged;
                }
                dbContext.Entry(runwayToDelete).State = EntityState.Unchanged;
                throw;
            }
        }
    }
}
