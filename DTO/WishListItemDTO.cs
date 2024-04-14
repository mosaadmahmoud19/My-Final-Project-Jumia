using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class WishListItemDTO
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Image URL is required")]
        public string Img { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive value")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive value")]
        public int UnitPrice { get; set; }

    }
}
