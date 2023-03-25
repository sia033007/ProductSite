using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSite.Models
{
    public class Product : BaseClass
    {
        [Required(ErrorMessage = "This Field is required")]
        [MaxLength(50)]
        public string Name { get; set; }
        [NotMapped]
        public string? Description { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        public double? Price { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "This Field is required")]
        public string? Currency { get; set; }
        public double? OldPrice { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        public int? Quantity { get; set; }
        [NotMapped]
        public IFormFile? Picture { get; set; }
        [MaxLength(50)]
        public string? DescriptionFileName { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }

}
