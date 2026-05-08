using System.Collections.Generic;
using AirportApp.Data.Domain;

namespace AirportApp.Data.Repositories.Interfaces
{
    public interface IShopItemRepository
    {
        IEnumerable<ShopItem> GetAll();

        ShopItem? GetById(int shopItemId);

        void Add(ShopItem shopItem);

        void Delete(int shopItemId);

        void Update(ShopItem shopItem);
    }
}
