using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Domain.Relations
{
    public class TicketInOrder
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Ticket Ticket { get; set; }
        public int Quantity { get; set; }
        public TicketInOrder(Guid id, Guid orderId, Guid productId, int Quantity)
        {
            this.Id = id;
            this.ProductId = productId;
            this.OrderId = orderId;
            this.Quantity = Quantity;
        }
    }
}
