using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySpend.Data;
using MySpend.Models.Entities;
using MySpend.Models.ViewModels;
using MySpend.Support;
using Microsoft.AspNetCore.RateLimiting;

namespace MySpend.Controllers
{
    public class UserController : Controller
    {

        /*
         The `_context` is the direct bridge between your C# code and your SQL Server database. 
        It's an instance of something called Entity Framework Core (an ORM).

        Instead of writing pure SQL code like `SELECT * FROM Users WHERE Email = ...`, the `_context` lets you communicate with the database using C#.
         */
        private readonly MySpendDbContext _context;


        public UserController(MySpendDbContext context)
        {
            _context = context;
        }


        //Return View() look fot a view with the same name as the function
        //GET
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? error)
        {
            if (error == "rate-limit")
            {
                ModelState.AddModelError("", "Many tries, please, try some time later.");
            }
            return View();
        }


        //POST
        [HttpPost] //Only respons to POST requests, a label to tell the program that is going to receive data
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //Duplicates validation: Search in the table 'Users'  if a user exist with that email
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                
                //If the email exists, it adds an error tho the model state to show it in the view
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }


            //Map data: Create a object user
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = PasswordHelper.Hash(model.Password)
            };

         
            //Add the object to the following/steps of Entity Framework
            _context.Users.Add(user);

            //Saves the changes in the sql server
            _context.SaveChanges();

            return RedirectToAction("Login");

        }

        [HttpPost]
        [EnableRateLimiting("LoginPolicy")]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || !PasswordHelper.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Email or password are incorrects");
                return View();
            }

            //Save session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Expenses", "Expenses");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

    }
}
