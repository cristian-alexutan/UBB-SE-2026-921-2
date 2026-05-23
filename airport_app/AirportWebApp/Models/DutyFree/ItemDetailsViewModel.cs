using AirportLib.Domain.User;

namespace AirportWebApp.Models.DutyFree
{
    public class ItemDetailsViewModel
    {
        public ShopItem Item { get; set; } = new ShopItem();
        public Shop Shop { get; set; } = null!;
        public DutyFreeModuleRole UserRole { get; set; }
        public bool IsManager => UserRole == DutyFreeModuleRole.Manager;
        public int CartId { get; set; }
        public int QuantityToAdd { get; set; } = 1;
        public bool ItemAlreadyInCart { get; set; }
    }
}
