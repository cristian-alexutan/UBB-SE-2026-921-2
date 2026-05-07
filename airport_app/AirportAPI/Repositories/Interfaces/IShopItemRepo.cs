using System.Collections.Generic;
using AirportAPI.Domain;

namespace AirportAPI.Repositories.Interfaces
{
    public interface IShopItemRepo
    {
        IEnumerable<ShopItem> GetAll();

        ShopItem? GetById(int shopItemId);

        void Add(ShopItem shopItem);

        void Delete(int shopItemId);

        void Update(ShopItem shopItem);
    }
}


