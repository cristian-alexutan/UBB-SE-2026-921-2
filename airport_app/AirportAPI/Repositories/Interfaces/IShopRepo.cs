namespace AirportAPI.Repositories.Interfaces
{
    using System.Collections.Generic;
    using AirportAPI.Domain;

    public interface IShopRepo
    {
        IEnumerable<Shop> GetAll();

        Shop? GetById(int shopId);

        void Add(Shop shop);

        Shop? Delete(int shopId);

        Shop? Update(Shop shop);
    }
}


