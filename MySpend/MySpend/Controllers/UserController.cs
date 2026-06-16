using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MySpend.Data;
using MySpend.Models.Entities;
using MySpend.Models.ViewModels;
using MySpend.Service;
using System;
using System.Net;
using System.Net.Mail;

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

        private readonly UserService _userService;
        private readonly EmailService _emailService;

        public UserController(MySpendDbContext context, EmailService emailService, UserService userService)
        {
            _context = context;
            _emailService = emailService;
            _userService = userService;
        }


        //Return View() look fot a view with the same name as the function
        //GET
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
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
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "If the email exists, a reset link was sent.");
                return View();
            }

            var token = Guid.NewGuid().ToString();

            user.ResetToken = token;
            user.ResetTokenExpiresAt = DateTimeOffset.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            var resetLink = Url.Action(
                "ResetPassword",
                "User",
                new { token },
                Request.Scheme
            ); 

            _emailService.Send(
                user.Email,
                "Reset your password",
                $"Click here to reset your password:\n{resetLink}"
            );

            TempData["Message"] = "Email to change the password sended.";
            return RedirectToAction("Login");
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.ResetPasswordAsync(model.Token, model.Password);

            if (!result)
            {
                ModelState.AddModelError("", "Invalid or expired token");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        //POST
        [HttpPost] //Only respons to POST requests, a label to tell the program that is going to receive data
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                PasswordHash = UserService.HashPassword(model.Password),
                EmailConfirmed = false
            };

            user.EmailToken = Guid.NewGuid().ToString();
            user.EmailTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(1);


            //Add the object to the following/steps of Entity Framework
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var confirmLink = Url.Action(
                "ConfirmEmail",
                "User",
                new { token = user.EmailToken },
                Request.Scheme
            );

            _emailService.Send(
                  user.Email,
                  "Confirm your email",
                  $"Click here to confirm your email: {confirmLink}\n\nThis link expires in 24 hours."
              );

            TempData["Message"] = "Registration successful! Please check your email to confirm your account.";
            return RedirectToAction("Login");

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Invalid confirmation token";
                return RedirectToAction("Login");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailToken == token);

            if (user == null)
            {
                TempData["Error"] = "Invalid confirmation token";
                return RedirectToAction("Login");
            }

            if (user.EmailTokenExpiresAt < DateTimeOffset.UtcNow)
            {
                TempData["Error"] = "Confirmation link has expired";
                return RedirectToAction("Login");
            }

            user.EmailConfirmed = true;
            user.EmailToken = null;
            user.EmailTokenExpiresAt = null;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Email confirmed successfully! You can now log in.";
            return RedirectToAction("Login");
        }


        [HttpPost]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userService.AuthenticateAsync(email, password);

            if (user == null)
            {
                ModelState.AddModelError("", "Email or password are incorrects");
                return View();
            }


            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email before logging in. Check your inbox.");
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
