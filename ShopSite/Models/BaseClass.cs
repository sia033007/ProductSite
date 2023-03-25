using System.ComponentModel.DataAnnotations;

namespace ShopSite.Models
{
    public abstract class BaseClass
    {
        [Key]
        public int Id { get; set; }
        public string PictureName { get; set; }
    }
}
