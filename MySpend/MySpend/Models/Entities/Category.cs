using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
