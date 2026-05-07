using AirportAPI.Repositories.Interfaces;
using AirportAPI;
namespace AirportAPI.Repositories
{
    public class AirportRepository(AppDbContext databaseContext) : IAirportRepository
    {
        public List<Airport> GetAllAirports()
        {
            return databaseContext.Airports.ToList();
        }

        public Airport? GetAirportById(int airportId)
        {
            return databaseContext.Airports.Find(airportId);
        }

        public int AddAirport(Airport newAirport)
        {
            databaseContext.Airports.Add(newAirport);
            databaseContext.SaveChanges();

            return newAirport.Id;
        }

        public void UpdateAirport(Airport airportToUpdate)
        {
            databaseContext.Airports.Update(airportToUpdate);
            databaseContext.SaveChanges();
        }

        public void DeleteAirportUsingId(int airportId)
        {
            Airport? airportToRemove = this.GetAirportById(airportId);

            if (airportToRemove == null)
            {
                return;
            }

            databaseContext.Airports.Remove(airportToRemove);
            databaseContext.SaveChanges();
        }
    }
}


