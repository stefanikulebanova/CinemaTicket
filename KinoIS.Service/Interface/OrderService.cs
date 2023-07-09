using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Interface
{
    public interface OrderService
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(Guid id);
        public List<Order> getAllOrdersByUserId(string userId);
        void deleteOrder(Guid orderId);
        Order findById(Guid orderId);
    }
}
