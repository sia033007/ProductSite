using System.ComponentModel.DataAnnotations;

namespace ShopSite.Models
{
    public class UpdateUserModel
    {
        [Required(ErrorMessage = "This Field is required")]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm New Password must be the same")]
        public string ConfirmNewPassword { get; set; }
    }
}
