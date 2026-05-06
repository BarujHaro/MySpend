using Microsoft.AspNetCore.Mvc;
using MySpend.Models.Entities;
using MySpend.Service;

namespace MySpend.Controllers
{
    //CRUD de gastos
    public class ExpensesController : Controller
    {


        //Inyeccion de dependencias para atraer el servicio, esto permite que el controlador no tenga que saber como se guarda los datos

        private readonly ExpenseService _expenseService;

        public ExpensesController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // Simulación de usuario (luego vendrá del login)
        private int CurrentUserId => 1;


        // GET: Expenses
        //Llama al servicio para obtener la lista de gastos del usuario actual
        //Task<IActionResult>
        //Task= representa la operación que se ejecuta en segundo plano (para que no se pare el programa)
        //IActionResult= Es una interfaz que define lo que el controlador le devuelve al navegador, en este caso devuelve una vista
        public async Task<IActionResult> Expenses()
        {
            var expenses = await _expenseService.GetExpensesAsync(CurrentUserId);

            ViewBag.TotalExpenses = expenses.Sum(e => e.Value);

            return View(expenses);
        }

        // GET: Expenses/CreateEditExpense/5
        //Si el id es null, envía un objeto nuevo para crear un gasto
        //si el id existe, busca el gasto en la base de datos para editarlo
        public async Task<IActionResult> CreateEditExpense(int? id)
        {
            if (id == null)
                return View(new Expense());

            var expense = await _expenseService.GetByIdAsync(id.Value, CurrentUserId);
            
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        //recibe datos del formulario
        public async Task<IActionResult> CreateEditExpense(Expense model)
        {
            //verifica que los datos cumplan con las reglas
            if (!ModelState.IsValid)
                return View(model);

            model.UserId = CurrentUserId;

            await _expenseService.SaveAsync(model);
            return RedirectToAction(nameof(Expenses));
        }


        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            await _expenseService.DeleteAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Expenses));
        }
    }
}
