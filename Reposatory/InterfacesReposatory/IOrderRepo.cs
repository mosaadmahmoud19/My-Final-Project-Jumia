using My_Final_Project.Models;

namespace My_Final_Project.Reposatory.InterfacesReposatory
{
    public interface IOrderRepo
    {
        public Order GetOrderById(int orderId);
        public List<OrderItems> GetOrderItemsByOrderId(int orderId);
        public void CreateOrder(Order order);
        public void UpdateOrder(Order order);
        public void DeleteOrder(Order order);
        public Order GetOrderByUserId(string UserId);
        public List<Order> GetAllOrders();
        public void DeleteAllCartitems(List<CartItems> cartItems);
        public Order GetLastOrder();
    }
}
