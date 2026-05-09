using Microsoft.EntityFrameworkCore;
using MySpend.Models.Entities;


namespace MySpend.Data
{
    public class MySpendDbContext : DbContext
    {
        public MySpendDbContext(DbContextOptions<MySpendDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Expense> Expenses => Set<Expense>();
    }
}
