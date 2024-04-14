using My_Final_Project.Models;

namespace My_Final_Project.DTO
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public int TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public int CartItemCount { get; set; }
        public virtual List<CartItemsDTO> CartItems { get; set; } = new List<CartItemsDTO>();
    }
}
