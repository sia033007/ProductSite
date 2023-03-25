using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSite.Models
{
    public class Category : BaseClass
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "This Field is required")]
        public string CategoryName { get; set; }
        [NotMapped]
        public IFormFile? Picture { get; set; }
        public List<Product> Products { get; set; }
    }
}
