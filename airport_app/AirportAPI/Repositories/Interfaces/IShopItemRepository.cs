namespace AirportAPI.Repositories.Interfaces
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


