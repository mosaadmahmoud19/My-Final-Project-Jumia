namespace My_Final_Project.DTO
{
    public class CreateOrderDTO
    {
        public string UserID { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}