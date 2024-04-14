using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public List<OrderItemDTO2> OrderItems { get; set; }

    }
}
