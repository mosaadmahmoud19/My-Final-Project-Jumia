using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Final_Project.DTO
{
    public class RegisterUserDTO
    {
        // [Required]
        public string Name { get; set; }
        // [Required]
        public string Username { get; set; }
        //  [Required]
        // [RegularExpression(@"^(?=.[a-z])(?=.[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Invalid Password")]
        // [StringLength(50, MinimumLength = 9, ErrorMessage = "Minimum Length is 9 !!")]
        public string Password { get; set; }

        //[Compare("Password")]
        //[NotMapped]
        //public string? ConfirmPassword { get; set; }
        //  [Required]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Length(10, 50, ErrorMessage = "Email must be between 10 and 50 characters")]
        public string Email { get; set; }
        //[Compare("Email")]
        //[NotMapped]
        //public string? ConfirmEmail { get; set; }
        [Required]
        [Range(15, 100, ErrorMessage = "Age should be more than 15 Years and less than 100")]
        public int Age { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z]{5,20}$", ErrorMessage = "Nationality should contain only letters and be between 5 and 20 characters")]
        public string Nationality { get; set; }
        [RegularExpression("^01[0125][0-9]{8}$", ErrorMessage = "Phone should start with 010 or 011 or 012 or 015 then 8 digits ")]
        public string Phone { get; set; }

        //   [Required]
        public string Role { get; set; }
    }
}
