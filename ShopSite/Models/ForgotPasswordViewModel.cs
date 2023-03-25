using System.ComponentModel.DataAnnotations;

namespace ShopSite.Models
{
	public class ForgotPasswordViewModel
	{
        [Required(ErrorMessage = "This Field is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
