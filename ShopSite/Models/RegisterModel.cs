using System.ComponentModel.DataAnnotations;

namespace ShopSite.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "This Field is required")]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must be the same")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
