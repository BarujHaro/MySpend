using Microsoft.EntityFrameworkCore;
using MySpend.Data;
using MySpend.Models.Entities;

namespace MySpend.Service
{
    public class CategoryService
    {
        private readonly MySpendDbContext _context;

        public CategoryService(MySpendDbContext context)
        {
            _context = context;
        }
 
        public async Task<List<Category>> GetCategoriesAsync(int userId)
        {
            return await _context.Categories
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id, int userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        public async Task SaveAsync(Category category)
        {
            if (category.Id == 0)
                _context.Categories.Add(category);
            else
                _context.Categories.Update(category);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var category = await GetByIdAsync(id, userId);
            if (category == null) return;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }


    }
}
