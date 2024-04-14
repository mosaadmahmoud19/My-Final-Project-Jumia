using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Final_Project.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        public string UserID { get; set; }
        public int ProductID { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime DatePosted { get; set; }
        public ApplicationUser User { get; set; }
        public Products Product { get; set; }
    }
}
