using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySpend.Data;
using MySpend.Models.Entities;
using MySpend.Support;

namespace MySpend.Controllers
{
    public class UserController : Controller
    {

        private readonly MySpendDbContext _context;

        public UserController(MySpendDbContext context)
        {
            _context = context;
        }
        //GET
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        //POST
        [HttpPost] //Only respons to POST requests
        public IActionResult Register(string name, string email, string password)
        {

            //Duplicates validation: Search in the table 'Users'  if a user exist with that email
            if (_context.Users.Any(u => u.Email == email))
            {
                
                //If the email exists, it adds an error tho the model state to show it in the view
                ModelState.AddModelError("", "Invalid email");
                return View();
            }


            //Map data: Create a object user
            var user = new User
            {
                Email = email,
                PasswordHash = PasswordHelper.Hash(password)
            };

         
            //Add the object to the following/steps of Entity Framework
            _context.Users.Add(user);

            //Saves the changes in the sql server
            _context.SaveChanges();

            return RedirectToAction("Login");



        }





    }
}
