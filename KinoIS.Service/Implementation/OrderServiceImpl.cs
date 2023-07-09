using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Implementation
{
    public class OrderServiceImpl : OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderServiceImpl(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public List<Order> getAllOrders()
        {
            return this._orderRepository.getAllOrders();
        }

        public List<Order> getAllOrdersByUserId(string userId)
        {
            return this._orderRepository.getAllOrdersByUserId(userId);
        }

        public Order getOrderDetails(Guid id)
        {
            return this._orderRepository.getOrderDetails(id);
        }
        public void deleteOrder(Guid orderId)
        {
           this._orderRepository.deleteOrder(orderId);
        }

        public Order findById(Guid orderId)
        {
            return this._orderRepository.findById(orderId);
        }
    }
}
