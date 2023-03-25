using System.ComponentModel.DataAnnotations;

namespace ShopSite.Models
{
    public class ResetPasswordViewModel
    {
        public string userId { get; set; }
        public string token { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must be the same")]
        public string ConfirmPassword { get; set; }
    }
}
