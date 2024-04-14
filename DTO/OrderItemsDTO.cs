using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class OrderItemDTO
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }
      
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

    }
}
