using Microsoft.AspNetCore.Identity;

namespace FlowerShop.Models.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public ApplicationUser User { get; set; }
    }
}
