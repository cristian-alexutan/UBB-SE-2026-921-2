using AirportApp.Data.Repositories.Proxies;

namespace AirportApp.Data.Services.Proxies;

public class CartServiceProxy : RepositoryProxyBase, ICartService
{
    private const int MinimumCartItemQuantity = 1;

    public CartServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Cart> GetAllCarts()
    {
        return this.GetList<CartDto>("api/carts")
            .Select(MapCart)
            .ToList();
    }

    public Cart GetCartById(int cartId)
    {
        CartDto? cart = this.GetOptional<CartDto>($"api/carts/{cartId}");
        return cart == null ? null! : MapCart(cart);
    }

    public Cart GetOrCreateCart(int userId)
    {
        Cart? existingCart = this.GetCartById(userId);

        if (existingCart != null)
        {
            return existingCart;
        }

        Cart newCart = new(
            userId,
            new Client(userId, "Current Client"),
            []);

        this.AddCart(newCart);
        return newCart;
    }

    public void AddCart(Cart cart)
    {
        this.Post("api/carts", MapCartRequest(cart));
    }

    public void DeleteCart(int cartId)
    {
        this.Delete($"api/carts/{cartId}");
    }

    public void AddItemToCart(int cartId, CartItem item)
    {
        this.Post($"api/carts/{cartId}/items", MapCartItemRequest(item));
    }

    public void RemoveItemFromCart(int cartId, int cartItemId)
    {
        this.Delete($"api/carts/{cartId}/items/{cartItemId}");
    }

    public void UpdateItemQuantity(int cartId, int cartItemId, int quantity)
    {
        this.Put($"api/carts/{cartId}/items/{cartItemId}/quantity", new UpdateCartItemQuantityRequest(quantity));
    }

    public void ClearCart(int cartId)
    {
        this.Delete($"api/carts/{cartId}/items");
    }

    public void DecreaseItemQuantity(int cartId, int cartItemId)
    {
        Cart? targetCart = this.GetCartById(cartId);
        CartItem? itemToModify = FindItemInCartById(targetCart, cartItemId);
        if (itemToModify.Quantity > MinimumCartItemQuantity)
        {
            this.UpdateItemQuantity(cartId, cartItemId, itemToModify.Quantity - MinimumCartItemQuantity);
        }
        else
        {
            this.RemoveItemFromCart(cartId, cartItemId);
        }
    }

    public double GetCartTotal(int cartId)
    {
        Cart? targetCart = this.GetCartById(cartId);
        return targetCart?.GetOverallPrice() ?? 0;
    }

    public bool IsLastCartItem(int cartId, int cartItemId)
    {
        Cart? targetCart = this.GetCartById(cartId);
        CartItem? targetItem = FindItemInCartById(targetCart, cartItemId);
        return targetItem != null && targetItem.Quantity == MinimumCartItemQuantity;
    }

    public IEnumerable<CartItem> GetCartItems(int cartId)
    {
        Cart? targetCart = this.GetCartById(cartId);
        return targetCart?.CartItems ?? [];
    }

    private static CartItem? FindItemInCartById(Cart cart, int cartItemId)
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

    private static Cart MapCart(CartDto cart)
    {
        return new Cart(
            cart.Id,
            MapClient(cart.Client),
            cart.CartItems?.Select(MapCartItem).ToList() ?? []);
    }

    private static CartItem MapCartItem(CartItemDto cartItem)
    {
        return new CartItem(
            cartItem.Id,
            MapShopItem(cartItem.ShopItem),
            cartItem.Quantity);
    }

    private static CartRequest MapCartRequest(Cart cart)
    {
        return new CartRequest(
            cart.Id,
            new ClientRequest(cart.Client.Id, cart.Client.Name),
            cart.CartItems?.Select(MapCartItemRequest).ToList() ?? []);
    }

    private static CartItemRequest MapCartItemRequest(CartItem cartItem)
    {
        return new CartItemRequest(
            cartItem.Id,
            cartItem.ShopItem.Id,
            cartItem.Quantity);
    }

    private static ShopItem MapShopItem(ShopItemDto? shopItem)
    {
        if (shopItem == null)
        {
            return new ShopItem();
        }

        return new ShopItem(
            shopItem.Id,
            shopItem.Quantity,
            shopItem.Price,
            MapShop(shopItem.Shop),
            shopItem.Photo ?? string.Empty,
            shopItem.Name ?? string.Empty,
            shopItem.Description ?? string.Empty);
    }

    private static Shop MapShop(ShopDto? shop)
    {
        if (shop == null)
        {
            return new Shop(string.Empty, string.Empty, MapManager(null));
        }

        return new Shop(
            shop.Id,
            shop.Name ?? string.Empty,
            shop.Type ?? string.Empty,
            MapManager(shop.Manager));
    }

    private static Manager MapManager(ManagerDto? manager)
    {
        if (manager == null)
        {
            return new Manager(0, string.Empty, string.Empty, string.Empty);
        }

        return new Manager(
            manager.Id,
            manager.Name ?? string.Empty,
            manager.Email ?? string.Empty,
            manager.Phone ?? string.Empty);
    }

    private static Client MapClient(ClientDto? client)
    {
        if (client == null)
        {
            return new Client(0, string.Empty);
        }

        return new Client(client.Id, client.Name ?? string.Empty);
    }

    private sealed class CartDto
    {
        public int Id { get; set; }

        public ClientDto? Client { get; set; }

        public List<CartItemDto>? CartItems { get; set; }
    }

    private sealed class CartItemDto
    {
        public int Id { get; set; }

        public ShopItemDto? ShopItem { get; set; }

        public int Quantity { get; set; }
    }

    private sealed class ShopItemDto
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public ShopDto? Shop { get; set; }

        public string? Photo { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    private sealed class ShopDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public ManagerDto? Manager { get; set; }
    }

    private sealed class ManagerDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }

    private sealed class ClientDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }

    private sealed record CartRequest(int Id, ClientRequest Client, List<CartItemRequest> CartItems);

    private sealed record CartItemRequest(int Id, int ShopItemId, int Quantity);

    private sealed record ClientRequest(int Id, string Name);

    private sealed record UpdateCartItemQuantityRequest(int Quantity);
}
