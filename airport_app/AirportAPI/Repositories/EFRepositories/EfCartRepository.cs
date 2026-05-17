using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Repositories
{
    public class EfCartRepository(AppDbContext databaseContext) : ICartRepository
    {
        public IEnumerable<Cart> GetAll()
        {
            return databaseContext.Carts
                .Include(cart => cart.Client)
                .Include(cart => cart.CartItems)
                    .ThenInclude(cartItem => cartItem.ShopItem)
                        .ThenInclude(shopItem => shopItem.Shop)
                .ToList();
        }

        public Cart? GetById(int cartId)
        {
            return databaseContext.Carts
                .Include(cart => cart.Client)
                .Include(cart => cart.CartItems)
                    .ThenInclude(cartItem => cartItem.ShopItem)
                        .ThenInclude(shopItem => shopItem.Shop)
                .FirstOrDefault(cart => cart.Id == cartId);
        }

        public void Add(Cart newCart)
        {
            if (newCart.Client != null)
            {
                Client? existingClient = databaseContext.Clients.Find(newCart.Client.Id);
                if (existingClient == null)
                {
                    databaseContext.Clients.Add(newCart.Client);
                }
                else
                {
                    newCart.Client = existingClient;
                }
            }

            databaseContext.Carts.Add(newCart);
            databaseContext.SaveChanges();
        }

        public void Delete(int cartId)
        {
            List<CartItem> relatedItems = databaseContext.CartItems
                .Where(cartItem => EF.Property<int>(cartItem, "CartId") == cartId)
                .ToList();

            if (relatedItems.Count > 0)
            {
                databaseContext.CartItems.RemoveRange(relatedItems);
            }

            Cart? cartToDelete = databaseContext.Carts.Find(cartId);

            if (cartToDelete == null)
            {
                return;
            }

            databaseContext.Carts.Remove(cartToDelete);
            databaseContext.SaveChanges();
        }

        public void AddItemToCart(int cartId, CartItem itemToAdd)
        {
            itemToAdd.Id = 0;

            Cart? cart = databaseContext.Carts
                .Include(cartInstance => cartInstance.CartItems)
                .FirstOrDefault(cartInstance => cartInstance.Id == cartId);

            if (cart == null)
            {
                return;
            }

            if (itemToAdd.ShopItem != null)
            {
                ShopItem? existingShopItem = databaseContext.ShopItems.Find(itemToAdd.ShopItem.Id);
                if (existingShopItem == null)
                {
                    return;
                }

                itemToAdd.ShopItem = existingShopItem;
            }

            cart.CartItems.Add(itemToAdd);
            databaseContext.SaveChanges();
        }

        public void RemoveItemFromCart(int cartId, int cartItemId)
        {
            CartItem? itemToRemove = databaseContext.CartItems.Find(cartItemId);

            if (itemToRemove == null)
            {
                return;
            }

            databaseContext.CartItems.Remove(itemToRemove);
            databaseContext.SaveChanges();
        }

        public void UpdateItemQuantity(int cartId, int cartItemId, int newQuantity)
        {
            CartItem? itemToUpdate = databaseContext.CartItems.Find(cartItemId);

            if (itemToUpdate == null)
            {
                return;
            }

            itemToUpdate.Quantity = newQuantity;
            databaseContext.SaveChanges();
        }

        public void ClearCart(int cartId)
        {
            List<CartItem> itemsToClear = databaseContext.CartItems
                .Where(cartItem => EF.Property<int>(cartItem, "CartId") == cartId)
                .ToList();

            if (itemsToClear.Count == 0)
            {
                return;
            }

            databaseContext.CartItems.RemoveRange(itemsToClear);
            databaseContext.SaveChanges();
        }
    }
}
