using AirportAPI.Repositories.Interfaces;
using AirportAPI.Services.Interfaces;

namespace AirportAPI.Services
{
    public class ShopItemService(IShopItemRepository shopItemRepository) : IShopItemService
    {
        private const string ItemNotFoundErrorMessage = "Shop item with Id {0} does not exist.";
        private const string NegativeQuantityErrorMessage = "Quantity cannot be negative.";
        private const string InvalidPriceErrorMessage = "Price must be greater than zero.";
        private const string EmptyNameErrorMessage = "Shop item name cannot be empty.";
        private const string InvalidShopErrorMessage = "Shop item must have a valid shop Id.";

        public IEnumerable<ShopItem> GetAll()
        {
            return shopItemRepository.GetAll();
        }

        public ShopItem GetById(int shopItemId)
        {
            ShopItem? shopItem = shopItemRepository.GetById(shopItemId);

            if (shopItem == null)
            {
                throw new InvalidOperationException(string.Format(ItemNotFoundErrorMessage, shopItemId));
            }

            return shopItem;
        }

        public IEnumerable<ShopItem> GetItemsByShopId(int shopId)
        {
            IEnumerable<ShopItem> allItems = shopItemRepository.GetAll();
            List<ShopItem> filteredItems = [];

            foreach (ShopItem item in allItems)
            {
                if (item.Shop != null && item.Shop.Id == shopId)
                {
                    filteredItems.Add(item);
                }
            }

            return filteredItems;
        }

        public IEnumerable<ShopItem> SearchItemsByName(int shopId, string searchText)
        {
            string query = searchText ?? string.Empty;
            IEnumerable<ShopItem> shopItems = this.GetItemsByShopId(shopId);
            List<ShopItem> matchingItems = [];

            foreach (ShopItem item in shopItems)
            {
                if (item.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    matchingItems.Add(item);
                }
            }

            return matchingItems;
        }

        public void RemoveShopItem(int shopItemId)
        {
            shopItemRepository.Delete(shopItemId);
        }

        public void AddShopItem(ShopItem shopItem)
        {
            this.ValidateShopItem(shopItem);
            shopItemRepository.Add(shopItem);
        }

        public void UpdateShopItem(ShopItem shopItem)
        {
            this.ValidateShopItem(shopItem);
            shopItemRepository.Update(shopItem);
        }

        public IEnumerable<ShopItem> GetItemsSortedByPrice(Shop currentShop)
        {
            if (currentShop == null)
            {
                throw new ArgumentNullException(nameof(currentShop));
            }

            List<ShopItem> items = (List<ShopItem>)this.GetItemsByShopId(currentShop.Id);

            items.Sort(new ShopItemPriceComparer());

            return items;
        }

        public IEnumerable<ShopItem> GetItemsSortedAlphabetically(Shop currentShop)
        {
            if (currentShop == null)
            {
                throw new ArgumentNullException(nameof(currentShop));
            }

            List<ShopItem> items = (List<ShopItem>)this.GetItemsByShopId(currentShop.Id);

            items.Sort(new ShopItemNameComparer());

            return items;
        }

        private void ValidateShopItem(ShopItem shopItem)
        {
            if (shopItem.Shop == null || shopItem.Shop.Id <= 0)
            {
                throw new ArgumentException(InvalidShopErrorMessage, nameof(shopItem));
            }

            if (shopItem.Quantity < 0)
            {
                throw new ArgumentException(NegativeQuantityErrorMessage, nameof(shopItem));
            }

            if (shopItem.Price <= 0)
            {
                throw new ArgumentException(InvalidPriceErrorMessage, nameof(shopItem));
            }

            if (string.IsNullOrWhiteSpace(shopItem.Name))
            {
                throw new ArgumentException(EmptyNameErrorMessage, nameof(shopItem));
            }
        }

        private class ShopItemPriceComparer : IComparer<ShopItem>
        {
            public int Compare(ShopItem? firstItem, ShopItem? secondItem)
            {
                if (firstItem == null || secondItem == null)
                {
                    return 0;
                }

                return firstItem.Price.CompareTo(secondItem.Price);
            }
        }

        private class ShopItemNameComparer : IComparer<ShopItem>
        {
            public int Compare(ShopItem? firstItem, ShopItem? secondItem)
            {
                if (firstItem == null || secondItem == null)
                {
                    return 0;
                }

                return string.Compare(firstItem.Name, secondItem.Name, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}