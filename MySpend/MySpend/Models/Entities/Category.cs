using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
