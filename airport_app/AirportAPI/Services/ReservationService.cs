using AirportAPI.Repositories.Interfaces;
using AirportAPI.Services.Interfaces;

namespace AirportAPI.Services
{
    public class ReservationService(
        IReservationRepository reservationRepository,
        IShopItemService shopItemService,
        ICartService cartService) : IReservationService
    {
        private const string OutOfStockErrorMessageTemplate = "Not enough stock for '{0}'. Requested: {1}, Available: {2}";
        private const string MissingShopItemErrorMessage = "A product included in the reservation could not be found in the inventory.";

        public IEnumerable<Reservation> GetAllReservations()
        {
            return reservationRepository.GetAll();
        }

        public Reservation? GetReservationById(int reservationId)
        {
            return reservationRepository.GetById(reservationId);
        }

        public void ReserveCart(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            var reservationCartItems = reservation.ReservationCart.CartItems;

            foreach (CartItem cartItem in reservationCartItems)
            {
                ShopItem? shopItem = shopItemService.GetById(cartItem.ShopItem.Id);

                if (shopItem == null)
                {
                    throw new InvalidOperationException(MissingShopItemErrorMessage);
                }

                if (shopItem.Quantity < cartItem.Quantity)
                {
                    string errorMessage = string.Format(
                        OutOfStockErrorMessageTemplate,
                        shopItem.Name,
                        cartItem.Quantity,
                        shopItem.Quantity);

                    throw new InvalidOperationException(errorMessage);
                }
            }

            foreach (CartItem cartItem in reservationCartItems)
            {
                ShopItem shopItem = shopItemService.GetById(cartItem.ShopItem.Id)!;
                shopItem.Quantity -= cartItem.Quantity;
                shopItemService.UpdateShopItem(shopItem);
            }

            reservationRepository.Add(reservation);
        }

        public Reservation? GetActiveReservationForCart(int cartId)
        {
            IEnumerable<Reservation> allReservations = reservationRepository.GetAll();

            foreach (Reservation reservation in allReservations)
            {
                if (reservation.ReservationCart.Id == cartId && reservation.Active)
                {
                    return reservation;
                }
            }

            return null;
        }

        public void DeleteReservation(int reservationId)
        {
            reservationRepository.Delete(reservationId);
        }

        public void CancelReservation(int reservationId)
        {
            Reservation? reservation = reservationRepository.GetById(reservationId);

            if (reservation == null)
            {
                return;
            }

            if (!reservation.Active)
            {
                return;
            }

            if (reservation.ReservationCart != null && reservation.ReservationCart.CartItems != null)
            {
                foreach (CartItem cartItem in reservation.ReservationCart.CartItems)
                {
                    ShopItem? shopItem = shopItemService.GetById(cartItem.ShopItem.Id);
                    if (shopItem != null)
                    {
                        shopItem.Quantity += cartItem.Quantity;
                        shopItemService.UpdateShopItem(shopItem);
                    }
                }
            }

            cartService.ClearCart(reservation.ReservationCart!.Id);
            reservation.Active = false;

            reservationRepository.Update(reservation);
        }
    }
}