using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "The Password and confirmation password donot Match")]
        public string confirmPassword { get; set; } = null!;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;

    }
}