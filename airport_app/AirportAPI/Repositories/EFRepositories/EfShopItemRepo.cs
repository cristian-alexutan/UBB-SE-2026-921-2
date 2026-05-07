using AirportAPI;
using AirportAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfShopItemRepo : IShopItemRepo
    {
        private readonly AppDbContext dbContext;

        public EfShopItemRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ShopItem> GetAll()
        {
            return this.dbContext.ShopItems.Include(shopItem => shopItem.Shop).ToList();
        }

        public ShopItem? GetById(int shopItemId)
        {
            return this.dbContext.ShopItems.Include(shopItem => shopItem.Shop).FirstOrDefault(shopItem => shopItem.Id == shopItemId);
        }

        public void Add(ShopItem shopItem)
        {
            this.dbContext.ShopItems.Add(shopItem);
            this.dbContext.SaveChanges();
        }

        public void Delete(int shopItemId)
        {
            ShopItem? shopItem = this.dbContext.ShopItems.Find(shopItemId);

            if (shopItem != null)
            {
                this.dbContext.ShopItems.Remove(shopItem);
                this.dbContext.SaveChanges();
            }
        }

        public void Update(ShopItem shopItem)
        {
            this.dbContext.ShopItems.Update(shopItem);
            this.dbContext.SaveChanges();
        }
    }
}


