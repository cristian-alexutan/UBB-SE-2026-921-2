using AirportAPI;
using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories;

public class EfClientRepo : IClientRepo
{
    private readonly AppDbContext dbContext;
    public EfClientRepo(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<Client> GetAll()
    {
        return this.dbContext.Clients.ToList();
    }

    public Client? GetById(int clientId)
    {
        return this.dbContext.Clients.Find(clientId);
    }

    public void Add(Client client)
    {
        this.dbContext.Clients.Add(client);
        this.dbContext.SaveChanges();
    }

    public Client? Delete(int clientId)
    {
        var client = this.dbContext.Clients.Find(clientId);

        if (client == null)
        {
            return null;
        }

        this.dbContext.Clients.Remove(client);
        this.dbContext.SaveChanges();
        return client;
    }

    public Client? Update(Client client)
    {
        this.dbContext.Clients.Update(client);
        this.dbContext.SaveChanges();
        return client;
    }
}



