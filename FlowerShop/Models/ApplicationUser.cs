using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace FlowerShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(40)]
        public string Email { get; set; }
    }
}
