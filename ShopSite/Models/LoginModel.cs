using System.ComponentModel.DataAnnotations;

namespace ShopSite.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "This Field is required")]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
