using AirportAPI.Repositories.Interfaces;

namespace AirportAPI.Repositories
{
    public class EfClientRepository(AppDbContext databaseContext) : IClientRepository
    {
        public IEnumerable<Client> GetAll()
        {
            return databaseContext.Clients.ToList();
        }

        public Client? GetById(int clientId)
        {
            return databaseContext.Clients.Find(clientId);
        }

        public void Add(Client newClient)
        {
            databaseContext.Clients.Add(newClient);
            databaseContext.SaveChanges();
        }

        public Client? Update(Client clientToUpdate)
        {
            databaseContext.Clients.Update(clientToUpdate);
            databaseContext.SaveChanges();

            return clientToUpdate;
        }

        public Client? Delete(int clientId)
        {
            Client? clientToRemove = databaseContext.Clients.Find(clientId);

            if (clientToRemove == null)
            {
                return null;
            }

            databaseContext.Clients.Remove(clientToRemove);
            databaseContext.SaveChanges();

            return clientToRemove;
        }
    }
}