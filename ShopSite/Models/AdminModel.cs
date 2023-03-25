using System.ComponentModel.DataAnnotations;

namespace ShopSite.Models
{
    public class AdminModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
