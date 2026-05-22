namespace AirportLib.Services.Services.Interfaces
{
    public interface IShopService
    {
        IEnumerable<Shop> GetAllAvailableShops();

        Shop? GetShopById(int shopId);

        void AddShop(Shop shop);

        void DeleteShop(int shopId);

        void UpdateShop(Shop shop);

        IEnumerable<Shop> SortAlphabetically(IEnumerable<Shop> shops);
        public IEnumerable<Shop> SearchByName(string input);
    }
}
