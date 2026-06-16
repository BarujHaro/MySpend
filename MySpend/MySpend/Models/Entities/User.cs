using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;


     

        public bool EmailConfirmed { get; set; } = false;
        public string? EmailToken { get; set; }
        public DateTimeOffset? EmailTokenExpiresAt { get; set; }

       

        public string? ResetToken { get; set; }
        public DateTimeOffset? ResetTokenExpiresAt { get; set; }



        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
