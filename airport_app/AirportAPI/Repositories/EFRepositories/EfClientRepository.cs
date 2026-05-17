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
            newClient.Id = 0;
            databaseContext.Clients.Add(newClient);
            databaseContext.SaveChanges();
        }

        public Client? Update(Client clientToUpdate)
        {
            Client? existingClient = databaseContext.Clients.Find(clientToUpdate.Id);
            if (existingClient == null)
            {
                return null;
            }

            existingClient.Name = clientToUpdate.Name;
            databaseContext.SaveChanges();

            return existingClient;
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
