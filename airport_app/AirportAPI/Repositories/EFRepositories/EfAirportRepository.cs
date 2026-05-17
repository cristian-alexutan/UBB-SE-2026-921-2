using AirportAPI.Repositories.Interfaces;
namespace AirportAPI.Repositories
{
    public class EfAirportRepository(AppDbContext databaseContext) : IAirportRepository
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
            newAirport.Id = 0;
            databaseContext.Airports.Add(newAirport);
            databaseContext.SaveChanges();

            return newAirport.Id;
        }

        public void UpdateAirport(Airport airportToUpdate)
        {
            Airport? existingAirport = databaseContext.Airports.Find(airportToUpdate.Id);
            if (existingAirport == null)
            {
                return;
            }

            existingAirport.Code = airportToUpdate.Code;
            existingAirport.Name = airportToUpdate.Name;
            existingAirport.City = airportToUpdate.City;
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


