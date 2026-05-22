namespace AirportWebApp.Models.DutyFree
{
    public class CartItemViewModel
    {
        public CartItem? CartItem { get; set; }
        public bool IsLast { get; set; }
    }

    public class CartViewModel
    {
        public int CartId { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();
        public double Total { get; set; }
        public bool HasActiveReservation { get; set; }
        public int? ActiveReservationId { get; set; }
    }
}
