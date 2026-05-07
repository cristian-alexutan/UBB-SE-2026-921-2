using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfGateRepository(AppDbContext databaseContext) : IGateRepository
    {
        public List<Gate> GetAllGates()
        {
            return databaseContext.Gates.ToList();
        }

        public Gate? GetGateById(int gateId)
        {
            return databaseContext.Gates.Find(gateId);
        }

        public int AddGate(Gate newGate)
        {
            databaseContext.Gates.Add(newGate);
            databaseContext.SaveChanges();

            return newGate.Id;
        }

        public void UpdateGate(Gate updatedGate)
        {
            databaseContext.Gates.Update(updatedGate);
            databaseContext.SaveChanges();
        }

        public void DeleteGateUsingId(int gateId)
        {
            List<Flight> associatedFlights = databaseContext.Flights
                .Where(flight => flight.Gate.Id == gateId)
                .ToList();

            if (associatedFlights.Count > 0)
            {
                databaseContext.Flights.RemoveRange(associatedFlights);
            }

            Gate? gateToRemove = databaseContext.Gates.Find(gateId);

            if (gateToRemove == null)
            {
                return;
            }

            databaseContext.Gates.Remove(gateToRemove);

            try
            {
                databaseContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                System.Diagnostics.Debug.WriteLine($"Database Update Exception: {exception.InnerException?.Message}");
                throw;
            }
        }
    }
}