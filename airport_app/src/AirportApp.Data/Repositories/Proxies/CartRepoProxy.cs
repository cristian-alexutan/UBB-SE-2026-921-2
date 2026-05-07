namespace AirportApp.Data.Repositories.Proxies;

public class CartRepoProxy : RepositoryProxyBase, ICartRepo
{
    public CartRepoProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Cart> GetAll()
    {
        return this.GetList<CartDto>("api/carts")
            .Select(MapCart)
            .ToList();
    }

    public Cart GetById(int cartId)
    {
        CartDto cart = this.GetRequired<CartDto>($"api/carts/{cartId}");
        return MapCart(cart);
    }

    public void Add(Cart cart)
    {
        this.Post("api/carts", cart);
    }

    public void Delete(int cartId)
    {
        this.Delete($"api/carts/{cartId}");
    }

    public void AddItemToCart(int cartId, CartItem item)
    {
        this.Post($"api/carts/{cartId}/items", item);
    }

    public void RemoveItemFromCart(int cartId, int cartItemId)
    {
        this.Delete($"api/carts/{cartId}/items/{cartItemId}");
    }

    public void UpdateItemQuantity(int cartId, int cartItemId, int quantity)
    {
        this.Put($"api/carts/{cartId}/items/{cartItemId}/quantity", new { Quantity = quantity });
    }

    public void ClearCart(int cartId)
    {
        this.Delete($"api/carts/{cartId}/items");
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
}
