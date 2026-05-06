namespace AirportApp.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using AirportApp.Data.Domain;
    using AirportApp.Data.Repositories.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class EfShopRepository : IShopRepo
    {
        private readonly AppDbContext context;

        public EfShopRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Shop> GetAll()
        {
            return context.Shops
                .Include(shop => shop.Manager)
                .AsNoTracking()
                .ToList();
        }

        public Shop? GetById(int shopId)
        {
            return context.Shops
                .Include(shop => shop.Manager)
                .AsNoTracking()
                .FirstOrDefault(shop => shop.Id == shopId);
        }

        public void Add(Shop shop)
        {
            context.Managers.Attach(shop.Manager);
            context.Shops.Add(shop);
            context.SaveChanges();
        }

        public Shop? Delete(int shopId)
        {
            var shop = context.Shops
                .FirstOrDefault(shop => shop.Id == shopId);
            if (shop == null)
            {
                return null;
            }

            context.Shops.Remove(shop);
            context.SaveChanges();
            return shop;
        }

        public Shop? Update(Shop shop)
        {
            var existing = context.Shops
                .FirstOrDefault(existingShop => existingShop.Id == shop.Id);
            if (existing == null)
            {
                return null;
            }

            existing.Name = shop.Name;
            existing.Type = shop.Type;
            existing.ManagerId = shop.Manager.Id;
            context.SaveChanges();
            return existing;
        }
    }
}