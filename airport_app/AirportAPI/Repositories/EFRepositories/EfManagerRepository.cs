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
            newManager.Id = 0;
            databaseContext.Managers.Add(newManager);
            databaseContext.SaveChanges();
        }

        public Manager? Update(Manager managerToUpdate)
        {
            Manager? existingManager = databaseContext.Managers.Find(managerToUpdate.Id);
            if (existingManager == null)
            {
                return null;
            }

            existingManager.Name = managerToUpdate.Name;
            existingManager.Email = managerToUpdate.Email;
            existingManager.Phone = managerToUpdate.Phone;
            databaseContext.SaveChanges();

            return existingManager;
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
