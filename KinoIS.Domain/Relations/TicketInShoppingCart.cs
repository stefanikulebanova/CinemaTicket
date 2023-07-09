using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Domain.Relations
{
    public class TicketInShoppingCart
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }    
        public virtual Ticket CurrentTicket { get; set; }
        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart UserCart { get; set; }

        public TicketInShoppingCart(Guid ShoppingCartId, Guid TicketId)
        {
            this.ShoppingCartId = ShoppingCartId;
            this.TicketId = TicketId;
        }

    }
}
