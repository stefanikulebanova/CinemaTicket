using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinoIs.Repository.Implementation
{
    public class TicketInShoppingCartRepositoryImpl : TicketInShoppingCartRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<TicketInShoppingCart> entities;
        public TicketInShoppingCartRepositoryImpl(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TicketInShoppingCart>();
        }

        public TicketInShoppingCart add(Guid shoppingCartId, Guid ticketId)
        {
            TicketInShoppingCart ticketInShoppingCart = new TicketInShoppingCart(shoppingCartId, ticketId);
            context.ticketInShoppingCarts.Add(ticketInShoppingCart);
            context.SaveChanges();
            return ticketInShoppingCart;
        }

        public List<TicketInShoppingCart> findAll()
        {
            return this.context.ticketInShoppingCarts.ToList();
        }

        public List<TicketInShoppingCart> findAllByShoppingCartId(Guid id)
        {
            return this.context.ticketInShoppingCarts.Where(x => x.ShoppingCartId.Equals(id)).ToList();
        }

        public List<Ticket> findAllTicketsByShoppingCartId(Guid id)
        {
            List<TicketInShoppingCart> ticketInShoppingCarts = context.ticketInShoppingCarts.Where(x => x.ShoppingCartId.Equals(id)).ToList();
            List<Ticket> tickets = new List<Ticket>();
            foreach (var item in ticketInShoppingCarts)
            {
                Ticket t = this.context.tickets.Where(x => x.Id == item.TicketId).FirstOrDefault();
                tickets.Add(t);
            }
            return tickets;
        }

        public void removeTicket(Guid ticketId, Guid scId)
        {
            TicketInShoppingCart ticketInShoppingCart = this.context.ticketInShoppingCarts
                .Where(x => x.TicketId.Equals(ticketId)).FirstOrDefault();
            this.context.ticketInShoppingCarts.Remove(ticketInShoppingCart);
            this.context.SaveChanges();
        }
    }
}
