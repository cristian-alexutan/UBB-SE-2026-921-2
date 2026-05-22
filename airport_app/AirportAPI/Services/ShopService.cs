using AirportAPI.Repositories.Interfaces;

namespace AirportAPI.Services
{
    public class ShopService(IShopRepository shopRepository) : IShopService
    {
        private const string EmptyNameErrorMessage = "The shop name field must not be empty.";
        private const string EmptyTypeErrorMessage = "The shop type field must not be empty.";
        private const string DuplicateNameErrorMessage = "A shop with this name already exists in the system.";

        public IEnumerable<Shop> GetAllAvailableShops()
        {
            return shopRepository.GetAll();
        }

        public Shop? GetShopById(int shopId)
        {
            return shopRepository.GetById(shopId);
        }

        public void AddShop(Shop shopToAdd)
        {
            if (string.IsNullOrWhiteSpace(shopToAdd.Name))
            {
                throw new ArgumentException(EmptyNameErrorMessage, nameof(shopToAdd));
            }

            if (string.IsNullOrWhiteSpace(shopToAdd.Type))
            {
                throw new ArgumentException(EmptyTypeErrorMessage, nameof(shopToAdd));
            }

            bool doesNameAlreadyExist = this.CheckIfNameExists(shopToAdd.Name);
            if (doesNameAlreadyExist)
            {
                throw new InvalidOperationException(DuplicateNameErrorMessage);
            }

            shopRepository.Add(shopToAdd);
        }

        public void UpdateShop(Shop shopToUpdate)
        {
            if (string.IsNullOrWhiteSpace(shopToUpdate.Name))
            {
                throw new ArgumentException(EmptyNameErrorMessage, nameof(shopToUpdate));
            }

            if (string.IsNullOrWhiteSpace(shopToUpdate.Type))
            {
                throw new ArgumentException(EmptyTypeErrorMessage, nameof(shopToUpdate));
            }

            bool isDuplicateName = this.CheckIfNameIsDuplicateForOtherShop(shopToUpdate.Id, shopToUpdate.Name);
            if (isDuplicateName)
            {
                throw new InvalidOperationException(DuplicateNameErrorMessage);
            }

            shopRepository.Update(shopToUpdate);
        }

        public void DeleteShop(int shopId)
        {
            shopRepository.Delete(shopId);
        }

        public IEnumerable<Shop> SortAlphabetically(IEnumerable<Shop> shopsToSort)
        {
            List<Shop> sortedList = new List<Shop>(shopsToSort);

            sortedList.Sort(new ShopNameComparer());

            return sortedList;
        }

        public IEnumerable<Shop> SearchByName(string searchText)
        {
            IEnumerable<Shop> allShops = this.GetAllAvailableShops();
            List<Shop> matchingShops = new List<Shop>();

            foreach (Shop shop in allShops)
            {
                if (shop.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    matchingShops.Add(shop);
                }
            }

            return matchingShops;
        }

        private bool CheckIfNameExists(string nameToFind)
        {
            foreach (Shop existingShop in shopRepository.GetAll())
            {
                if (string.Equals(existingShop.Name, nameToFind, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfNameIsDuplicateForOtherShop(int currentShopId, string nameToVerify)
        {
            foreach (Shop otherShop in shopRepository.GetAll())
            {
                if (otherShop.Id != currentShopId && string.Equals(otherShop.Name, nameToVerify, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private class ShopNameComparer : IComparer<Shop>
        {
            public int Compare(Shop? firstShop, Shop? secondShop)
            {
                if (firstShop == null || secondShop == null)
                {
                    return 0;
                }

                return string.Compare(firstShop.Name, secondShop.Name, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}