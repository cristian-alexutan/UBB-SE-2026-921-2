using System;
using System.Collections.Generic;
using AirportWebApp.Domain;

namespace AirportWebApp.Services.Interfaces
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
