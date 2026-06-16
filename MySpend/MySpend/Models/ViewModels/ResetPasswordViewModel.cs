using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength =8, ErrorMessage="Password mus be at least 8 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain one uppercase letter and one number")]
        
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
