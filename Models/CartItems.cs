using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Final_Project.Models
{
    public class CartItems
    {
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        [ForeignKey("Cart")]
        public int CartID { get; set; }
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
       /* public string Size { get; set; }

        public string Color { get; set; }*/
        public Products Product {  get; set; }
        public Carts Cart { get; set; }
    }
}
