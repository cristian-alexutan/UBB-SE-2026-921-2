namespace AirportApp.Data.Repositories.Proxies;

public class ReservationRepoProxy : RepositoryProxyBase, IReservationRepo
{
    public ReservationRepoProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Reservation> GetAll()
    {
        return this.GetList<ReservationDto>("api/reservations")
            .Select(MapReservation)
            .ToList();
    }

    public Reservation GetById(int reservationId)
    {
        return MapReservation(this.GetRequired<ReservationDto>($"api/reservations/{reservationId}"));
    }

    public void Add(Reservation reservation)
    {
        this.Post("api/reservations", reservation);
    }

    public void Delete(int reservationId)
    {
        this.Delete($"api/reservations/{reservationId}");
    }

    public void Update(Reservation reservation)
    {
        this.Put("api/reservations", reservation);
    }

    private static Reservation MapReservation(ReservationDto dto)
    {
        return new Reservation(dto.Id, MapCart(dto.ReservationCart), dto.Active, dto.ReservationDate);
    }

    private static Cart MapCart(CartDto? dto)
    {
        if (dto == null)
        {
            return new Cart(0, new Client(0, string.Empty), new List<CartItem>());
        }

        return new Cart(dto.Id, MapClient(dto.Client), dto.CartItems?.Select(MapCartItem).ToList() ?? new List<CartItem>());
    }

    private static CartItem MapCartItem(CartItemDto dto)
    {
        return new CartItem(dto.Id, MapShopItem(dto.ShopItem), dto.Quantity);
    }

    private static ShopItem MapShopItem(ShopItemDto? dto)
    {
        if (dto == null)
        {
            return new ShopItem();
        }

        return new ShopItem(dto.Id, dto.Quantity, dto.Price, MapShop(dto.Shop), dto.Photo ?? string.Empty, dto.Name ?? string.Empty, dto.Description ?? string.Empty);
    }

    private static Shop MapShop(ShopDto? dto)
    {
        if (dto == null)
        {
            return new Shop(string.Empty, string.Empty, MapManager(null));
        }

        return new Shop(dto.Id, dto.Name ?? string.Empty, dto.Type ?? string.Empty, MapManager(dto.Manager));
    }

    private static Manager MapManager(ManagerDto? dto)
    {
        if (dto == null)
        {
            return new Manager(0, string.Empty, string.Empty, string.Empty);
        }

        return new Manager(dto.Id, dto.Name ?? string.Empty, dto.Email ?? string.Empty, dto.Phone ?? string.Empty);
    }

    private static Client MapClient(ClientDto? dto)
    {
        if (dto == null)
        {
            return new Client(0, string.Empty);
        }

        return new Client(dto.Id, dto.Name ?? string.Empty);
    }

    private sealed class ReservationDto
    {
        public int Id { get; set; }

        public CartDto? ReservationCart { get; set; }

        public bool Active { get; set; }

        public DateTime ReservationDate { get; set; }
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
