using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain one uppercase letter and one number")]
        public string Password { get; set; }
    }
}