using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfShopRepository(AppDbContext databaseContext) : IShopRepository
    {
        public IEnumerable<Shop> GetAll()
        {
            return databaseContext.Shops
                .Include(shop => shop.Manager)
                .AsNoTracking()
                .ToList();
        }

        public Shop? GetById(int shopId)
        {
            return databaseContext.Shops
                .Include(shop => shop.Manager)
                .AsNoTracking()
                .FirstOrDefault(shop => shop.Id == shopId);
        }

        public void Add(Shop newShop)
        {
            databaseContext.Managers.Attach(newShop.Manager);

            databaseContext.Shops.Add(newShop);
            databaseContext.SaveChanges();
        }

        public Shop? Update(Shop shopToUpdate)
        {
            Shop? existingShop = databaseContext.Shops
                .FirstOrDefault(shop => shop.Id == shopToUpdate.Id);

            if (existingShop == null)
            {
                return null;
            }

            existingShop.Name = shopToUpdate.Name;
            existingShop.Type = shopToUpdate.Type;

            Manager managerInstance = databaseContext.Managers.Local
                .FirstOrDefault(manager => manager.Id == shopToUpdate.Manager.Id)
                ?? shopToUpdate.Manager;

            if (databaseContext.Entry(managerInstance).State == EntityState.Detached)
            {
                databaseContext.Managers.Attach(managerInstance);
            }

            existingShop.Manager = managerInstance;

            databaseContext.SaveChanges();

            return existingShop;
        }

        public Shop? Delete(int shopId)
        {
            Shop? shopToRemove = databaseContext.Shops
                .FirstOrDefault(shop => shop.Id == shopId);

            if (shopToRemove == null)
            {
                return null;
            }

            databaseContext.Shops.Remove(shopToRemove);
            databaseContext.SaveChanges();

            return shopToRemove;
        }
    }
}