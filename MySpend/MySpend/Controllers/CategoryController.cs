using Microsoft.AspNetCore.Mvc;
using MySpend.Models.Entities;
using MySpend.Service;
using System;

namespace MySpend.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        } 
        private int? CurrentUserId => HttpContext.Session.GetInt32("UserId");

        public async Task<IActionResult> Categories()
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");

            try
            {
                var categories = await _categoryService.GetCategoriesAsync(CurrentUserId.Value);
                return View(categories);
            }catch(Exception ex)
            {
                return Content($"Critical error loading categories: {ex.Message}");
            } 
        }


        public async Task<IActionResult> CreateEditCategories(int? id)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");

            try
            {
                if (id == null)
                    return View(new Category());

                var category = await _categoryService.GetByIdAsync(id.Value, CurrentUserId.Value);

                if (category == null)
                    return NotFound();

                return View(category);
            }
            catch(Exception ex)
            {
                return Content($"Critical error in the server: {ex.Message}");
            }

        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        //recibe datos del formulario
        public async Task<IActionResult> CreateEditCategories(Category model)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");
    


            try
            {
                ModelState.Remove("User");
                ModelState.Remove("Expenses");

                if (string.IsNullOrEmpty(model.Name))
                {
                    ModelState.AddModelError("Name", "Name cannot be null or empty");
                }

                if (!ModelState.IsValid)
                    return View(model);



                model.UserId = CurrentUserId.Value;
                await _categoryService.SaveAsync(model);
                return RedirectToAction(nameof(Categories));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error has happened on the server: " + ex.Message);
                return View(model);
            }






        }

         
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "User");

            try
            {
                await _categoryService.DeleteAsync(id, CurrentUserId.Value);
                TempData["SuccessMessage"] = "Category deleted successfully.";
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Could not delete the category: " + ex.Message;
            }
            return RedirectToAction(nameof(Categories));

        }



    }
}
