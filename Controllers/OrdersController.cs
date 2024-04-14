using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.DTO;
using My_Final_Project.Models;
using My_Final_Project.Reposatory;
using My_Final_Project.Reposatory.Interfaces;
using My_Final_Project.Reposatory.InterfacesReposatory;
using System;
using System.Collections.Generic;

namespace My_Final_Project.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IProducsRepo producsRepo;

        public OrderController(IOrderRepo orderRepo, IProducsRepo producsRepo)
        {
            _orderRepo = orderRepo;
            this.producsRepo = producsRepo;
        }
        [HttpGet("{userid}")]
        public ActionResult<OrderDTO> GetOrder(string userid)
        {
            var order = _orderRepo.GetOrderByUserId(userid);
            if (order == null) return NotFound("Order not found");
            var orderItemsDTO2 = new List<OrderItemDTO2>();
            foreach (var item in order.OrderItems)
            {
                var orderItemDTO = new OrderItemDTO2
                {
                    ProductImg = item.Product.Images[0],
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                orderItemsDTO2.Add(orderItemDTO);
            }
            var orderDTO = new OrderDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserID = order.UserID,
                OrderItems = orderItemsDTO2
            };
            return Ok(orderDTO);
        }

        [HttpPost("create")]
        public ActionResult CreateOrder([FromBody] CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserID = orderDTO.UserID,
                TotalAmount = 0,
                OrderItems = new List<OrderItems>()
            };
            int totalAmount = 0;
            foreach (var item in orderDTO.OrderItems)
            {
                var product = producsRepo.GetByID(item.ProductId);
                if (product == null)
                    return NotFound($"Product with ID {item.ProductId} not found.");
                var orderItem = new OrderItems
                {

                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                totalAmount += item.Quantity * item.UnitPrice;
                order.OrderItems.Add(orderItem);
            }
            order.TotalAmount = totalAmount;
            order.OrderDate = DateTime.Now;
            _orderRepo.CreateOrder(order);
            _orderRepo.GetOrderById(order.OrderId);
            _orderRepo.DeleteAllCartitems(order.ApplicationUser.Carts.CartItems);
            return Created("Order created successfully", order.OrderId);
        }
        [HttpPut("update/{orderId}")]
        public ActionResult UpdateOrder(int orderId, [FromBody] UpdateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingOrder = _orderRepo.GetOrderById(orderId);
            if (existingOrder == null)
                return NotFound("Order not found");
            _orderRepo.UpdateOrder(existingOrder);
            return Ok("Order updated successfully");
        }

        [HttpDelete("delete/{orderId}")]
        public ActionResult DeleteOrder(int orderId)
        {
            var existingOrder = _orderRepo.GetOrderById(orderId);
            if (existingOrder == null)
                return NotFound("Order not found");

            _orderRepo.DeleteOrder(existingOrder);
            return Ok("Order deleted successfully");
        }
    }
}
