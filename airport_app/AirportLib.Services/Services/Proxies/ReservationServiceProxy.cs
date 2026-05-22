using AirportLib.Services.Services.Interfaces;

namespace AirportLib.Services.Services.Proxies;

public class ReservationServiceProxy : ServiceProxyBase, IReservationService
{
    public ReservationServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Reservation> GetAllReservations()
    {
        return this.GetList<ReservationDto>("api/reservations")
            .Select(MapReservation)
            .ToList();
    }

    public Reservation GetReservationById(int reservationId)
    {
        return MapReservation(this.GetRequired<ReservationDto>($"api/reservations/{reservationId}"));
    }

    public void ReserveCart(Reservation reservation)
    {
        var request = new ReserveCartRequest(
            reservation.ReservationCart.Id,
            reservation.Active,
            reservation.ReservationDate,
            reservation.ReservationCart.CartItems
                .Select(cartItem => new CartItemRequest(cartItem.Id, cartItem.ShopItem.Id, cartItem.Quantity))
                .ToList());

        int newId = this.PostForResult<ReserveCartRequest, int>("api/reservations/reserve", request);
        reservation.Id = newId;
    }

    public void DeleteReservation(int reservationId)
    {
        this.Delete($"api/reservations/{reservationId}");
    }

    public void CancelReservation(int reservationId)
    {
        this.Put($"api/reservations/{reservationId}/cancel", (object?)null);
    }

    public Reservation? GetActiveReservationForCart(int cartId)
    {
        ReservationDto? reservationDto = this.GetOptional<ReservationDto>($"api/reservations/cart/{cartId}/active");
        return reservationDto == null ? null : MapReservation(reservationDto);
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

        return new Cart(
            dto.Id,
            new Client(dto.Client?.Id ?? 0, dto.Client?.Name ?? string.Empty),
            dto.CartItems?.Select(MapCartItem).ToList() ?? new List<CartItem>());
    }

    private static CartItem MapCartItem(CartItemDto dto)
    {
        ShopItem shopItem = dto.ShopItem == null
            ? new ShopItem()
            : new ShopItem(
                dto.ShopItem.Id,
                dto.ShopItem.Quantity,
                dto.ShopItem.Price,
                MapShop(dto.ShopItem.Shop),
                dto.ShopItem.Photo ?? string.Empty,
                dto.ShopItem.Name ?? string.Empty,
                dto.ShopItem.Description ?? string.Empty);

        return new CartItem(dto.Id, shopItem, dto.Quantity);
    }

    private static Shop MapShop(ShopDto? dto)
    {
        if (dto == null)
        {
            return new Shop(string.Empty, string.Empty, new Manager(0, string.Empty, string.Empty, string.Empty));
        }

        return new Shop(
            dto.Id,
            dto.Name ?? string.Empty,
            dto.Type ?? string.Empty,
            new Manager(dto.Manager?.Id ?? 0, dto.Manager?.Name ?? string.Empty, dto.Manager?.Email ?? string.Empty, dto.Manager?.Phone ?? string.Empty));
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

    private sealed record ReserveCartRequest(
        int CartId,
        bool Active,
        DateTime ReservationDate,
        List<CartItemRequest> CartItems);

    private sealed record CartItemRequest(int CartItemId, int ShopItemId, int Quantity);
}
