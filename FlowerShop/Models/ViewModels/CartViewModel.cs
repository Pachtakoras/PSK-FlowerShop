namespace FlowerShop.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }

        public decimal Total { get; set; }
    }
}
