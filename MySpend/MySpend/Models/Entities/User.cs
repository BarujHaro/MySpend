using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;


        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
