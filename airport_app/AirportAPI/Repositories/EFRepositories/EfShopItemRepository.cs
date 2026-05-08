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
            if (newShopItem.Shop != null)
            {
                Shop? existingShop = databaseContext.Shops.Find(newShopItem.Shop.Id);
                if (existingShop == null)
                {
                    return;
                }

                newShopItem.Shop = existingShop;
            }

            databaseContext.ShopItems.Add(newShopItem);
            databaseContext.SaveChanges();
        }

        public void Update(ShopItem shopItemToUpdate)
        {
            ShopItem? existingItem = databaseContext.ShopItems.Find(shopItemToUpdate.Id);
            if (existingItem == null)
            {
                return;
            }

            existingItem.Quantity = shopItemToUpdate.Quantity;
            existingItem.Price = shopItemToUpdate.Price;
            existingItem.Photo = shopItemToUpdate.Photo;
            existingItem.Name = shopItemToUpdate.Name;
            existingItem.Description = shopItemToUpdate.Description;

            if (shopItemToUpdate.Shop != null)
            {
                Shop? existingShop = databaseContext.Shops.Find(shopItemToUpdate.Shop.Id);
                if (existingShop != null)
                {
                    existingItem.Shop = existingShop;
                }
            }

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
