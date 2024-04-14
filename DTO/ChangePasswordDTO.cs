using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}