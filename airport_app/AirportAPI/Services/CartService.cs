using AirportAPI.Repositories.Interfaces;

namespace AirportAPI.Services
{
    public class CartService(
        ICartRepository cartRepository,
        IShopItemService shopItemService) : ICartService
    {
        private const int MinimumCartItemQuantity = 1;

        public IEnumerable<Cart> GetAllCarts()
        {
            return cartRepository.GetAll();
        }

        public Cart? GetCartById(int cartId)
        {
            return cartRepository.GetById(cartId);
        }

        public Cart GetOrCreateCart(int userId)
        {
            Cart? existingCart = cartRepository.GetById(userId);

            if (existingCart != null)
            {
                return existingCart;
            }

            Client currentClient = new Client(userId, "Current Client");
            List<CartItem> emptyItemList = [];

            Cart newCart = new Cart(userId, currentClient, emptyItemList);
            cartRepository.Add(newCart);

            return newCart;
        }

        public void AddCart(Cart newCart)
        {
            cartRepository.Add(newCart);
        }

        public void DeleteCart(int cartId)
        {
            cartRepository.Delete(cartId);
        }

        public void AddItemToCart(int cartId, CartItem itemToAdd)
        {
            Cart? targetCart = cartRepository.GetById(cartId);

            if (targetCart == null)
            {
                return;
            }

            CartItem? existingItemInCart = this.FindItemInCart(targetCart, itemToAdd.ShopItem.Id);

            ShopItem? productInStock = shopItemService.GetById(itemToAdd.ShopItem.Id);

            int requestedQuantity = itemToAdd.Quantity;
            if (existingItemInCart != null)
            {
                requestedQuantity += existingItemInCart.Quantity;
            }

            if (productInStock == null || requestedQuantity > productInStock.Quantity)
            {
                throw new InvalidOperationException("Operation failed: There is insufficient stock available for this item.");
            }

            if (existingItemInCart != null)
            {
                cartRepository.UpdateItemQuantity(cartId, existingItemInCart.Id, requestedQuantity);
            }
            else
            {
                cartRepository.AddItemToCart(cartId, itemToAdd);
            }
        }

        public void RemoveItemFromCart(int cartId, int cartItemId)
        {
            cartRepository.RemoveItemFromCart(cartId, cartItemId);
        }

        public void UpdateItemQuantity(int cartId, int cartItemId, int newQuantity)
        {
            Cart? targetCart = cartRepository.GetById(cartId);

            if (targetCart == null)
            {
                return;
            }

            CartItem? targetItem = this.FindItemInCartById(targetCart, cartItemId);

            if (targetItem != null)
            {
                ShopItem? productInStock = shopItemService.GetById(targetItem.ShopItem.Id);

                if (productInStock != null && newQuantity > productInStock.Quantity)
                {
                    throw new InvalidOperationException("Cannot update quantity: Total exceeds available stock.");
                }
            }

            cartRepository.UpdateItemQuantity(cartId, cartItemId, newQuantity);
        }

        public void ClearCart(int cartId)
        {
            cartRepository.ClearCart(cartId);
        }

        public double GetCartTotal(int cartId)
        {
            Cart? targetCart = cartRepository.GetById(cartId);

            if (targetCart == null)
            {
                return 0;
            }

            return targetCart.GetOverallPrice();
        }

        public void DecreaseItemQuantity(int cartId, int cartItemId)
        {
            Cart? targetCart = cartRepository.GetById(cartId);

            if (targetCart == null)
            {
                return;
            }

            CartItem? itemToModify = this.FindItemInCartById(targetCart, cartItemId);

            if (itemToModify == null)
            {
                return;
            }

            if (itemToModify.Quantity > MinimumCartItemQuantity)
            {
                int updatedQuantity = itemToModify.Quantity - MinimumCartItemQuantity;
                cartRepository.UpdateItemQuantity(cartId, cartItemId, updatedQuantity);
            }
            else
            {
                cartRepository.RemoveItemFromCart(cartId, cartItemId);
            }
        }

        public IEnumerable<CartItem> GetCartItems(int cartId)
        {
            Cart? targetCart = cartRepository.GetById(cartId);

            if (targetCart == null || targetCart.CartItems == null)
            {
                return [];
            }

            return targetCart.CartItems;
        }

        public bool IsLastCartItem(int cartId, int cartItemId)
        {
            Cart? targetCart = cartRepository.GetById(cartId);

            if (targetCart == null)
            {
                return false;
            }

            CartItem? targetItem = this.FindItemInCartById(targetCart, cartItemId);

            return targetItem != null && targetItem.Quantity == MinimumCartItemQuantity;
        }

        private CartItem? FindItemInCart(Cart cart, int shopItemId)
        {
            foreach (CartItem currentItem in cart.CartItems)
            {
                if (currentItem.ShopItem != null && currentItem.ShopItem.Id == shopItemId)
                {
                    return currentItem;
                }
            }

            return null;
        }

        private CartItem? FindItemInCartById(Cart cart, int cartItemId)
        {
            foreach (CartItem currentItem in cart.CartItems)
            {
                if (currentItem.Id == cartItemId)
                {
                    return currentItem;
                }
            }

            return null;
        }
    }
}