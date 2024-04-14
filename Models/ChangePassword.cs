using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class ChangePassword
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}