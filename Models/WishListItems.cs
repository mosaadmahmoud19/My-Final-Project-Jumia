using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class WishListItems
    {
        [ForeignKey("Products")]
        public int ProductID { get; set; }
        [ForeignKey("WishList")]
        public int WishlistID { get; set; }
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
        public Products Products { get; set; }
        public WishList WishList { get; set; }
    }
}
