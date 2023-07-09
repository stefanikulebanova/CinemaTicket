using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinoIs.Repository.Implementation
{
    public class TicketInOrderRepositoryImpl : TicketInOrderRepository
    {
        private readonly ApplicationDbContext context;
        public TicketInOrderRepositoryImpl(ApplicationDbContext context)
        {
            this.context = context; 
        }

        public List<TicketInOrder> findAll()
        {
            return this.context.TicketInOrders.ToList();
        }

        public TicketInOrder Insert(TicketInOrder ticketInOrder)
        {
            this.context.TicketInOrders.Add(ticketInOrder);
            return ticketInOrder;
        }

        public List<Ticket> ticketsInOrder(Guid orderId)
        {
            List<TicketInOrder> ticketsAndOrders = this.context.TicketInOrders.Where(x => x.OrderId.Equals(orderId)).ToList();
            List<Ticket> tickets = new List<Ticket>();
            foreach (var ticketAndOrder in ticketsAndOrders)
            {
                tickets.Add(this.context.tickets.Where(x => x.Id.Equals(ticketAndOrder.ProductId)).FirstOrDefault()); //Ticket e tuka null zaboraeno e da se dodeli vrednost!!
            }
            List<Ticket> test = tickets;

            return tickets;
        }
    }
}
