using System.Collections.Generic;
using AirportApp.Data.Domain;

namespace AirportApp.Data.Services.Interfaces
{
    public interface ICartService
    {
        IEnumerable<Cart> GetAllCarts();

        Cart GetCartById(int cartId);

        Cart GetOrCreateCart(int userId);

        void AddCart(Cart cart);

        void DeleteCart(int cartId);

        void AddItemToCart(int cartId, CartItem item);

        void RemoveItemFromCart(int cartId, int cartItemId);

        void UpdateItemQuantity(int cartId, int cartItemId, int quantity);

        void ClearCart(int cartId);

        void DecreaseItemQuantity(int cartId, int cartItemId);

        double GetCartTotal(int cartId);

        bool IsLastCartItem(int cartId, int cartItemId);

        IEnumerable<CartItem> GetCartItems(int cartId);
    }
}