using System.ComponentModel;

namespace FlowerShop.Models
{
    public class OrderProduct
    {
        public long OrderId { get; set; }
        public Order Order { get; set; }

        public long ProductId { get; set; }

        [DefaultValue(1)]
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
