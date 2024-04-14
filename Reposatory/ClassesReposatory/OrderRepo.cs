using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;
using My_Final_Project.Reposatory.Interfaces;
using My_Final_Project.Reposatory.InterfacesReposatory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace My_Final_Project.Reposatory
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ITIStore _context;

        public OrderRepo(ITIStore context)
        {
            _context = context;
        }
        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Include(s=>s.ApplicationUser).ThenInclude(a=>a.Carts.CartItems).FirstOrDefault(o => o.OrderId == orderId);
        }
        public List<OrderItems> GetOrderItemsByOrderId(int orderId)
        {
            return _context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();
        }
        public void CreateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public void UpdateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        public void DeleteOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
        public List<Order> GetAllOrders()
        {
            return _context.Orders.Include(s=>s.ApplicationUser).ToList();
        }
        public Order GetOrderByUserId(string UserId)
        {
            return _context.Orders.Include(c => c.OrderItems).ThenInclude(s => s.Product).FirstOrDefault(o => o.UserID == UserId);
        }
        public void DeleteAllCartitems(List<CartItems> cartItems)
        {
            foreach (var item in cartItems)
            {
                _context.CartItems.Remove(item);
            }
            _context.SaveChanges();
        }



        public Order GetLastOrder()
        {
            return _context.Orders
        .OrderByDescending(o => o.OrderId)
        .Select(o => new Order
        {
            OrderId = o.OrderId,
            ApplicationUser = new ApplicationUser
            {
                Id = o.ApplicationUser.Id,
                UserName = o.ApplicationUser.UserName
            }
        })
        .FirstOrDefault();
        }
    }
}
