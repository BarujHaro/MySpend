using Microsoft.EntityFrameworkCore;
using MySpend.Data;
using MySpend.Models.Entities;

namespace MySpend.Service
{
    public class ExpenseService
    {
        private readonly MySpendDbContext _context;

        public ExpenseService(MySpendDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetExpensesAsync(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(int id, int userId)
        {
            return await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        public async Task SaveAsync(Expense expense)
        {
            if (expense.Id == 0)
                _context.Expenses.Add(expense);
            else
                _context.Expenses.Update(expense);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var expense = await GetByIdAsync(id, userId);
            if (expense == null) return;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }
}