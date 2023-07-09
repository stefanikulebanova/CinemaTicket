using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinoIs.Repository.Implementation
{
    public class OrderRepositoryImpl : OrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepositoryImpl(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getAllOrders()
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.TicketInOrders)
                .Include("TicketInOrders.Ticket")
                .ToListAsync().Result;
        }

        public List<Order> getAllOrdersByUserId(string userId)
        {
            List<Order> orders = this.context.orders.Where(x => x.UserId.Equals(userId)).ToList();
            return orders;
        }

        public Order getOrderDetails(Guid id)
        {
            Order order = this.context.orders.Where(x => x.Id.Equals(id)).FirstOrDefault();
            return order;
        }

        public Order Insert(Order order)
        {
            this.context.orders.Add(order);
            this.context.SaveChanges();
            return order;
        }
        public void deleteOrder(Guid orderId)
        {
            Order order = this.context.orders.Where(x => x.Id.Equals(orderId)).FirstOrDefault();
            this.context.orders.Remove(order);
            this.context.SaveChanges();
        }

        public Order findById(Guid orderId)
        {
            Order order = this.context.orders.Where(x => x.Id == orderId).FirstOrDefault();
            return order;
        }
    }
}
