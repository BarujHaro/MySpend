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
        private readonly CategoryService _categoryService;
        public ExpensesController(ExpenseService expenseService, CategoryService categoryService)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
        }
 
        private int? CurrentUserId => HttpContext.Session.GetInt32("UserId");

        // GET: Expenses
        //Llama al servicio para obtener la lista de gastos del usuario actual
        //Task<IActionResult>
        //Task= representa la operación que se ejecuta en segundo plano (para que no se pare el programa)
        //IActionResult= Es una interfaz que define lo que el controlador le devuelve al navegador, en este caso devuelve una vista
        public async Task<IActionResult> Expenses()
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");

            var expenses = await _expenseService.GetExpensesAsync(CurrentUserId.Value);

            ViewBag.TotalExpenses = expenses.Sum(e => e.Value);

            return View(expenses);
        }

        // GET: Expenses/CreateEditExpense/5
        //Si el id es null, envía un objeto nuevo para crear un gasto
        //si el id existe, busca el gasto en la base de datos para editarlo
        public async Task<IActionResult> CreateEditExpense(int? id) 
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");

            var categories = await _categoryService.GetCategoriesAsync(CurrentUserId.Value);

            ViewBag.Categories = categories;

            if (id == null)
                return View(new Expense());

            var expense = await _expenseService.GetByIdAsync(id.Value, CurrentUserId.Value);
            
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
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");

            if (model.Value <= 0)
            {
                ModelState.AddModelError("Value", "Value most be more than 0");
            }

            ModelState.Remove("User");
            ModelState.Remove("Category");
            //verifica que los datos cumplan con las reglas
            if (!ModelState.IsValid){
                var categories = await _categoryService.GetCategoriesAsync(CurrentUserId.Value);
                ViewBag.Categories = categories;
                return View(model);
            }

            try
            {
                model.UserId = CurrentUserId.Value;
                await _expenseService.SaveAsync(model);
                return RedirectToAction(nameof(Expenses));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "An error has happened on the server: " + ex.Message);
                var categories = await _categoryService.GetCategoriesAsync(CurrentUserId.Value);
                ViewBag.Categories = categories;
                return View(model);
            }


            

            
            
        }


        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");

            try
            {
                await _expenseService.DeleteAsync(id, CurrentUserId.Value);
                TempData["SuccessMessage"] = "Expense deleted successfully.";
            }
            catch( Exception ex)
            {
                TempData["ErrorMessage"] = "Could not delete the expense: " + ex.Message;
            }
         
             return RedirectToAction(nameof(Expenses));
        }

  


    }
}
