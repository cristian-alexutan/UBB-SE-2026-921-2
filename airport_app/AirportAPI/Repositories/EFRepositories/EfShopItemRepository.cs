using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfShopItemRepository(AppDbContext databaseContext) : IShopItemRepository
    {
        public IEnumerable<ShopItem> GetAll()
        {
            return databaseContext.ShopItems
                .Include(item => item.Shop)
                .ToList();
        }

        public ShopItem? GetById(int shopItemId)
        {
            return databaseContext.ShopItems
                .Include(item => item.Shop)
                .FirstOrDefault(item => item.Id == shopItemId);
        }

        public void Add(ShopItem newShopItem)
        {
            databaseContext.ShopItems.Add(newShopItem);
            databaseContext.SaveChanges();
        }

        public void Update(ShopItem shopItemToUpdate)
        {
            databaseContext.ShopItems.Update(shopItemToUpdate);
            databaseContext.SaveChanges();
        }

        public void Delete(int shopItemId)
        {
            ShopItem? itemToRemove = databaseContext.ShopItems.Find(shopItemId);

            if (itemToRemove == null)
            {
                return;
            }

            databaseContext.ShopItems.Remove(itemToRemove);
            databaseContext.SaveChanges();
        }
    }
}