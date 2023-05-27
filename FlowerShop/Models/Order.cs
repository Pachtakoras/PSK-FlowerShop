using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class Order
    {
        public enum orderStatus 
        {
            New,
            Approved,
            Cancelled,
            Delivered
        }
        public long Id { get; set; }


        [Required]
        public string CustomerId { get; set; }
        [Required]
        public List<OrderProduct> OrderProducts { get; set; }

        [MaxLength(60)]
        public string DeliveryOption { get; set; } = string.Empty;
        [MaxLength(60)]
        public string PaymentMethod { get; set; } = string.Empty;

        public int ItemsCount { get; set; }

        [Range(0.01, 999.00)]
        public decimal PriceTotal { get; set; }

        [EnumDataType(typeof(orderStatus))]
        public string Status { get; set; }
        [Timestamp]
        public byte[]? Timestamp { get; set; }
        public DateTime OrderDate { get; set; }

        public Order() { }
    }
}
