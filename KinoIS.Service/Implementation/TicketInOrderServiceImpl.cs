using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using KinoIS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Implementation
{
    public class TicketInOrderServiceImpl : TicketInOrderService
    {
        private readonly TicketInOrderRepository ticketInOrderRepository;
        public TicketInOrderServiceImpl(TicketInOrderRepository ticketInOrderRepository)
        {
            this.ticketInOrderRepository = ticketInOrderRepository;
        }

        public List<TicketInOrder> findAll()
        {
            return this.ticketInOrderRepository.findAll();
        }

        public List<Ticket> ticketsInOrder(Guid orderId)
        {
            return this.ticketInOrderRepository.ticketsInOrder(orderId);
        }
    }
}
