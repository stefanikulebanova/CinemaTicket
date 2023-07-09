using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface OrderRepository
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(Guid model);
        public Order Insert(Order order);
        public List<Order> getAllOrdersByUserId(string userId);
        public void deleteOrder(Guid orderId);
        Order findById(Guid orderId);
    }
}
