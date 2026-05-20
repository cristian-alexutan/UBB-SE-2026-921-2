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
            newShop.Id = 0;
            if (newShop.Manager == null || newShop.Manager.Id == 0)
            {
                newShop.Manager = databaseContext.Managers.FirstOrDefault()
                    ?? throw new InvalidOperationException("No managers exist in the database table.");
            }
            else
            {
                newShop.Manager = databaseContext.Managers.Find(newShop.Manager.Id)
                    ?? throw new InvalidOperationException($"Manager with id {newShop.Manager.Id} does not exist.");
            }

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

            if (existingShop.Manager == null || existingShop.Manager.Id == 0)
            {
                existingShop.Manager = databaseContext.Managers.FirstOrDefault()
                    ?? throw new InvalidOperationException("No managers exist in the database table.");
            }
            else
            {
                existingShop.Manager = databaseContext.Managers.Find(existingShop.Manager.Id)
                    ?? throw new InvalidOperationException($"Manager with id {existingShop.Manager.Id} does not exist.");
            }

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
