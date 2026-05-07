namespace AirportAPI.Domain
{
    public class CartItem
    {
        public int Id { get; set; }
        public ShopItem ShopItem { get; set; } = null!;
        public int Quantity { get; set; }

        public CartItem(int id, ShopItem shopItem, int quantity)
        {
            this.Id = id;
            this.ShopItem = shopItem;
            this.Quantity = quantity;
        }

        internal CartItem()
        {
        }

        public float GetTotalPrice()
        {
            return ShopItem.Price * Quantity;
        }
    }
}

