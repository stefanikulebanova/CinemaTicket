using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace KinoIs.Repository.Implementation
{
    public class ShoppingCartRepositoryImpl : ShoppingCartRepository
    {
        private readonly ApplicationDbContext context;
        public ShoppingCartRepositoryImpl (ApplicationDbContext context)
        {
            this.context = context; 
        }
        public List<ShoppingCart> findAll()
        {
            return this.context.shoppingCarts.ToList();
        }

        public ShoppingCart findById(Guid id)
        {
            return this.context.shoppingCarts.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public ShoppingCart findByOwnerId(string ownerId)
        {
            return this.context.shoppingCarts.Where(x => x.OwnerId.Equals(ownerId)).FirstOrDefault();
        }
        public void deleteById(Guid id)
        {
            ShoppingCart cart = this.findById(id);
            this.context.shoppingCarts.Remove(cart);
            this.context.SaveChanges();
        } 
        public ShoppingCart create(string ownerId)
        {
            ShoppingCart cart = new ShoppingCart(ownerId);
            this.context.shoppingCarts.Add(cart);
            this.context.SaveChanges();
            return cart;
        }

        public void removeTicketById(Guid ticketId)
        {
            return;
        }
    }
}
