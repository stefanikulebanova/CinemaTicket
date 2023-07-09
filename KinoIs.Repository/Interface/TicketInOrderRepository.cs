using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface TicketInOrderRepository
    {
        TicketInOrder Insert(TicketInOrder ticketInOrder);
        List<Ticket> ticketsInOrder(Guid orderId);
        List<TicketInOrder> findAll();
    }
}
