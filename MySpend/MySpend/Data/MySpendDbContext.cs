using Microsoft.EntityFrameworkCore;
using MySpend.Models.Entities;

namespace MySpend.Data
{
    public class MySpendDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        //public DbSet<User> Users { get; set; }

        public MySpendDbContext(DbContextOptions<MySpendDbContext> options) : base(options)
        {

        }
    }
}
