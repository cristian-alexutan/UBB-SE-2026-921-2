using System.Collections.Generic;
using System.Linq;

using AirportApp.Data;
using AirportApp.Data.Domain;
using AirportApp.Data.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AirportApp.Data.Repositories;

public class EfCartDbRepo : ICartRepo
{
    private readonly AppDbContext dbContext;

    public EfCartDbRepo(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<Cart> GetAll()
    {
        return dbContext.Carts
            .Include(cart => cart.Client)
            .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.ShopItem)
                    .ThenInclude(shopItem => shopItem.Shop)
            .ToList();
    }

    public Cart GetById(int cartId)
    {
        return dbContext.Carts
            .Include(cart => cart.Client)
            .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.ShopItem)
                    .ThenInclude(shopItem => shopItem.Shop)
            .FirstOrDefault(cart => cart.Id == cartId);
    }

    public void Add(Cart cart)
    {
        dbContext.Carts.Add(cart);
        dbContext.SaveChanges();
    }

    public void Delete(int cartId)
    {
        var relatedItems = dbContext.CartItems
            .Where(ci => EF.Property<int>(ci, "CartId") == cartId)
            .ToList();

        if (relatedItems.Any())
        {
            dbContext.CartItems.RemoveRange(relatedItems);
        }

        var cartToDelete = dbContext.Carts.Find(cartId);
        if (cartToDelete != null)
        {
            dbContext.Carts.Remove(cartToDelete);
            dbContext.SaveChanges();
        }
    }

    public void AddItemToCart(int cartId, CartItem item)
    {
        var cart = dbContext.Carts
            .Include(c => c.CartItems)
            .FirstOrDefault(c => c.Id == cartId);

        if (cart == null)
        {
            return;
        }

        if (item.ShopItem != null)
        {
            dbContext.Entry(item.ShopItem).State = EntityState.Unchanged;
        }

        cart.CartItems.Add(item);
        dbContext.SaveChanges();
    }

    public void RemoveItemFromCart(int cartId, int cartItemId)
    {
        var item = dbContext.CartItems.Find(cartItemId);
        if (item != null)
        {
            dbContext.CartItems.Remove(item);
            dbContext.SaveChanges();
        }
    }

    public void UpdateItemQuantity(int cartId, int cartItemId, int quantity)
    {
        var item = dbContext.CartItems.Find(cartItemId);
        if (item != null)
        {
            item.Quantity = quantity;
            dbContext.SaveChanges();
        }
    }

    public void ClearCart(int cartId)
    {
        var items = dbContext.CartItems
            .Where(ci => EF.Property<int>(ci, "CartId") == cartId)
            .ToList();

        if (items.Any())
        {
            dbContext.CartItems.RemoveRange(items);
            dbContext.SaveChanges();
        }
    }
}