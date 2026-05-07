using AirportAPI.Repositories.Interfaces;

namespace AirportAPI.Repositories
{
    public class EfManagerRepository(AppDbContext databaseContext) : IManagerRepository
    {
        public IEnumerable<Manager> GetAll()
        {
            return databaseContext.Managers.ToList();
        }

        public Manager? GetById(int managerId)
        {
            return databaseContext.Managers.Find(managerId);
        }

        public void Add(Manager newManager)
        {
            databaseContext.Managers.Add(newManager);
            databaseContext.SaveChanges();
        }

        public Manager? Update(Manager managerToUpdate)
        {
            databaseContext.Managers.Update(managerToUpdate);
            databaseContext.SaveChanges();

            return managerToUpdate;
        }

        public Manager? Delete(int managerId)
        {
            Manager? managerToRemove = databaseContext.Managers.Find(managerId);

            if (managerToRemove == null)
            {
                return null;
            }

            databaseContext.Managers.Remove(managerToRemove);
            databaseContext.SaveChanges();

            return managerToRemove;
        }
    }
}