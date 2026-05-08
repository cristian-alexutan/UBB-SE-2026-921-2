using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfRunwayRepository(AppDbContext databaseContext) : IRunwayRepository
    {
        private const string RunwayIdShadowPropertyName = "RunwayId";

        public List<Runway> GetAllRunways()
        {
            return databaseContext.Runways.ToList();
        }

        public Runway? GetRunwayById(int runwayId)
        {
            return databaseContext.Runways.Find(runwayId);
        }

        public int AddRunway(Runway newRunway)
        {
            newRunway.Id = 0;
            databaseContext.Runways.Add(newRunway);
            databaseContext.SaveChanges();
            return newRunway.Id;
        }

        public void UpdateRunway(Runway updatedRunway)
        {
            Runway? existingRunway = databaseContext.Runways.Find(updatedRunway.Id);
            if (existingRunway == null)
            {
                return;
            }

            existingRunway.Name = updatedRunway.Name;
            existingRunway.HandleTime = updatedRunway.HandleTime;
            databaseContext.SaveChanges();
        }

        public void DeleteRunwayUsingId(int runwayId)
        {
            List<Flight> associatedFlights = databaseContext.Flights
                .Where(flight => EF.Property<int>(flight, RunwayIdShadowPropertyName) == runwayId)
                .ToList();

            if (associatedFlights.Count > 0)
            {
                databaseContext.Flights.RemoveRange(associatedFlights);
            }

            Runway? runwayToDelete = databaseContext.Runways.Find(runwayId);

            if (runwayToDelete == null)
            {
                return;
            }

            try
            {
                databaseContext.Runways.Remove(runwayToDelete);
                databaseContext.SaveChanges();
            }
            catch (Exception)
            {
                foreach (Flight flightInstance in associatedFlights)
                {
                    databaseContext.Entry(flightInstance).State = EntityState.Unchanged;
                }

                databaseContext.Entry(runwayToDelete).State = EntityState.Unchanged;

                throw;
            }
        }
    }
}
