using Microsoft.AspNetCore.Mvc;
using MySpend.Models.Entities;
using MySpend.Models.ViewModels;
using MySpend.Service;
using System.Diagnostics;


namespace MySpend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExpenseService _expenseService;

        public HomeController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // Simulación de usuario (luego vendrá del login)
        private int CurrentUserId => 1;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Expenses()
        {
            var expenses = await _expenseService.GetExpensesAsync(CurrentUserId);

            ViewBag.TotalExpenses = expenses.Sum(e => e.Value);

            return View(expenses);
        }

        public async Task<IActionResult> CreateEditExpense(int? id)
        {
            if (id == null)
                return View(new Expense());

            var expense = await _expenseService.GetByIdAsync(id.Value, CurrentUserId);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEditExpense(Expense model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.UserId = CurrentUserId;

            await _expenseService.SaveAsync(model);
            return RedirectToAction(nameof(Expenses));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _expenseService.DeleteAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Expenses));
        }
    }
}
