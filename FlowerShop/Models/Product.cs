using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FlowerShop.Models
{
    public class Product
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        public long CategoryId { get; set; }

        [Required]
        [Range(0.01, 999.00)]
        public decimal Price { get; set; }

        [AllowNull]
        public string? Image { get; set; } = "no-image.png";

        public Category? Category { get; set; }

        public List<OrderProduct>  OrderProducts { get; set;}
    }
}
