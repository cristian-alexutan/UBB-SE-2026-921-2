namespace AirportApp.Data.Repositories.Interfaces
{
    using System.Collections.Generic;
    using AirportApp.Data.Domain;

    public interface IShopRepo
    {
        IEnumerable<Shop> GetAll();

        Shop? GetById(int shopId);

        void Add(Shop shop);

        Shop? Delete(int shopId);

        Shop? Update(Shop shop);
    }
}
