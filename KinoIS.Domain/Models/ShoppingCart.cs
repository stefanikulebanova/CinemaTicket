
using KinoIS.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Domain.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public double TotalPrice { get; set; }
        public virtual CinemaUser Owner { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }

        public ShoppingCart(string OwnerId)
        {
            this.OwnerId = OwnerId;
        }

        public ShoppingCart()
        {
        }
    }
}
