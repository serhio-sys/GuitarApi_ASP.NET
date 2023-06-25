using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopAPI.Models
{
    public class GuitarCategory
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public ICollection<Guitar> Guitars { get; set; } = new HashSet<Guitar>();
    }
}
