using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using AirportAPI;
using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories;

public class EfGateRepository : IGateRepository
{
    private readonly AppDbContext context;

    public EfGateRepository(AppDbContext context)
    {
        this.context = context;
    }

    public List<Gate> GetAllGates()
    {
        return context.Gates.ToList();
    }

    public Gate? GetGateById(int gateId)
    {
        return context.Gates.Find(gateId);
    }

    public int AddGate(Gate newGate)
    {
        context.Gates.Add(newGate);
        context.SaveChanges();
        return newGate.Id;
    }

    public void UpdateGate(Gate updatedGate)
    {
        context.Gates.Update(updatedGate);
        context.SaveChanges();
    }

    public void DeleteGateUsingId(int gateId)
    {
        var relatedFlights = context.Flights.Where(f => f.Gate.Id == gateId).ToList();

        if (relatedFlights.Any())
        {
            context.Flights.RemoveRange(relatedFlights);
        }

        var gate = context.Gates.SingleOrDefault(g => g.Id == gateId);

        if (gate != null)
        {
            context.Gates.Remove(gate);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Update Exception: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}



