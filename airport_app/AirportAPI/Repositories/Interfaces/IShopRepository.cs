namespace AirportAPI.Repositories.Interfaces
{
    using System.Collections.Generic;

    public interface IShopRepository
    {
        IEnumerable<Shop> GetAll();

        Shop? GetById(int shopId);

        void Add(Shop shop);

        Shop? Delete(int shopId);

        Shop? Update(Shop shop);
    }
}


