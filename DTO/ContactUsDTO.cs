using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class ContactUsDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(40, MinimumLength = 10)]
        public string Email { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 20)]
        public string Message { get; set; }
    }

}