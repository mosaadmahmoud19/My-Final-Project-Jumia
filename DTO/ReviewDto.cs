using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class ReviewDto
    {
        public int ReviewID { get; set; }
        public string UserID { get; set; }
        public int ProductID { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime DatePosted { get; set; }
    }
}
