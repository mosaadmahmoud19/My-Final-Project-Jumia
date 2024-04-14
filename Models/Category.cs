using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "CategoryName is required")]
        [MaxLength(50)]
        [MinLength(3)]
        public string CategoryName { get; set; }

        public List<Products> Products { get; set; }=new List<Products>();
    }
}
