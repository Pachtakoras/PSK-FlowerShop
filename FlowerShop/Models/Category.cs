using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class Category
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
