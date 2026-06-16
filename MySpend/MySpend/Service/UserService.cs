
using Microsoft.EntityFrameworkCore;
using MySpend.Data;
using MySpend.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.RateLimiting;
using MySpend.Models.ViewModels;
using MySpend.Service;


namespace MySpend.Service
{
    public class UserService
    {
        private readonly MySpendDbContext _context;
        private readonly EmailService _emailService;
        
        public UserService(MySpendDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;

        }

        
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;

            return VerifyPassword(password, user.PasswordHash)
                ? user
                : null;
        }

       
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

     
        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.EmailToken == token &&
                    u.EmailTokenExpiresAt > DateTime.UtcNow);

            if (user == null) return false;

            user.EmailConfirmed = true;
            user.EmailToken = null;

            await _context.SaveChangesAsync();
            return true;
        }



       
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.ResetToken == token &&
                    u.ResetTokenExpiresAt > DateTime.UtcNow);

            if (user == null) return false;

            user.PasswordHash = HashPassword(newPassword);
            user.ResetToken = null;

            await _context.SaveChangesAsync();
            return true;
        }

         
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public async Task<bool> RegisterAndSendConfirmationAsync(User user)
        {
            // Generar token único
            user.EmailToken = Guid.NewGuid().ToString();
            user.EmailTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(1);
            user.EmailConfirmed = false;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
