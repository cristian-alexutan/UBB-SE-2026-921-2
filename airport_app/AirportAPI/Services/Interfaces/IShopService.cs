namespace AirportAPI.Services.Interfaces
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
