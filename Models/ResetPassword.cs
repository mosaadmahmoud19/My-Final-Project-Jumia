using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; } = null!;
        public string confirmPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;

    }
}