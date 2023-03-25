using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSite.Models
{
    public class EditModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; } = string.Empty;
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        public string? PhoneNumber { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? IconFile { get; set; }
        [MaxLength(50)]
        public string? IconFileName { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? FacebookUrl { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? TwitterUrl { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? InstagramUrl { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? LinkedinUrl { get; set; } = string.Empty;
        [NotMapped]
        public string? Address { get; set; } = string.Empty;
        [NotMapped]
        public string? AboutUs { get; set; } = string.Empty;
    }
}
