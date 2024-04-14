using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Final_Project.Models
{
    public class Carts
    {
        [Key]
        public int CartID { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public int TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public virtual List<CartItems> CartItems { get; set; } = new List<CartItems>();
        public ApplicationUser ApplicationUser { get; set; }

    }
}
