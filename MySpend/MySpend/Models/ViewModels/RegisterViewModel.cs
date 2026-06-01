using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.ViewModels
{
    public class RegisterViewModel
    {
        //Campo obligatorio si no esta manda este mensaje
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //Verifica formato de correo
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain one uppercase letter and one number")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordC { get; set; }
    }
}