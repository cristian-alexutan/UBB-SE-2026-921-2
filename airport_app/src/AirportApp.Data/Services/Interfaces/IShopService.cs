using System;
using System.Collections.Generic;
using AirportApp.Data.Domain;

namespace AirportApp.Data.Services.Interfaces
{
    public interface IShopService
    {
        IEnumerable<Shop> GetAllAvailableShops();

        void AddShop(Shop shop);

        void DeleteShop(int shopId);

        void UpdateShop(Shop shop);

        IEnumerable<Shop> SortAlphabetically(IEnumerable<Shop> shops);
        public IEnumerable<Shop> SearchByName(string input);
    }
}
