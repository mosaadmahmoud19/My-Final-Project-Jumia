using System.ComponentModel.DataAnnotations;
namespace My_Final_Project.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive value")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive value")]
        public int Stock { get; set; }
        [Display(Name = "On Sale")]
        public bool OnSale {  get; set; }

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Images URL is required")]
        public List<string> Images { get; set; } = new List<string>();
        //public List<string> Colors { get; set; } = new List<string>();
        //public List<string> Sizes { get; set; } = new List<string>();
        public virtual Category category { get; set; }
        public virtual List<CartItems> CartItems { get; set; } = new List<CartItems>();
        public virtual List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        public List<Review> reviews { get; set; } = new List<Review>();
        public virtual List<WishListItems> WishListitems { get; set; }

    }
}
