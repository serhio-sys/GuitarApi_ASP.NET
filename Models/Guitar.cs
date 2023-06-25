using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShopAPI.Models
{
    public class Guitar
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
        public string? Price { get; set; }
        public string? ImgUrl { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public GuitarCategory? Category { get; set; }

    }
}
