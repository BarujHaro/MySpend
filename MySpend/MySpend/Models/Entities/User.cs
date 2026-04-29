using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Expense> Expenses { get; set; } = new();
    }
}
