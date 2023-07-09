using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using KinoIS.Service.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace KinoIS.Service.Implementation
{
    public class ShoppingCartServiceImpl : ShoppingCartService
    {
        private readonly ShoppingCartRepository shoppingCartRepository;
        private readonly OrderRepository orderRepository;
        private readonly EmailMessageRepository mailRepository;
        private readonly TicketInOrderRepository ticketInOrderRepository;
        private readonly CinemaUserRepository userRepository;
        private readonly TicketInShoppingCartRepository ticketInShoppingCartRepository;
        private readonly TicketRepository ticketRepository;

        public ShoppingCartServiceImpl(ShoppingCartRepository shoppingCartRepository, CinemaUserRepository userRepository,
            EmailMessageRepository mailRepository, TicketInOrderRepository ticketInOrderRepository, OrderRepository orderRepository,
            TicketInShoppingCartRepository ticketInShoppingCartRepository, TicketRepository ticketRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.userRepository = userRepository;
            this.mailRepository = mailRepository;
            this.ticketInOrderRepository = ticketInOrderRepository;
            this.orderRepository = orderRepository;
            this.ticketInShoppingCartRepository = ticketInShoppingCartRepository;
            this.ticketRepository = ticketRepository;
        }
        public void deleteById(Guid id)
        {
            this.shoppingCartRepository.deleteById(id);
        }

        public List<ShoppingCart> findAll()
        {
            return this.shoppingCartRepository.findAll();
        }

        public ShoppingCart findById(Guid id)
        {
            return this.shoppingCartRepository.findById(id);
        }

        public ShoppingCart findByOwnerId(string ownerId)
        {
            var loggedInUser = this.userRepository.findById(ownerId);

            var userCard = this.shoppingCartRepository.findByOwnerId(ownerId);

            List<Ticket> allTickets = this.ticketInShoppingCartRepository.findAllTicketsByShoppingCartId(userCard.Id);

            double totalPrice = 0.00;
            foreach (var ticket in allTickets)
            {
                totalPrice += (ticket.Quantity * ticket.Price);
            }
            ShoppingCart shoppingCart = this.shoppingCartRepository.findByOwnerId(ownerId);
            shoppingCart.TotalPrice = totalPrice;
            return shoppingCart;
        }
        public ShoppingCart create(string ownerId)
        {
            return this.shoppingCartRepository.create(ownerId);
        }
        public bool order(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this.userRepository.findById(userId);
                //var userCard = loggedInUser.UserCart;
                var userCard = this.shoppingCartRepository.findByOwnerId(loggedInUser.Id);

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Sucessfuly created order!";
                mail.Status = false;


                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this.orderRepository.Insert(order);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result = new List<TicketInOrder>();
                List<Ticket> allTickets = this.ticketInShoppingCartRepository.findAllTicketsByShoppingCartId(userCard.Id);
                foreach (var item in allTickets)
                {
                    TicketInOrder ticketInOrder = new TicketInOrder(new Guid(), order.Id, item.Id, 1);
                    this.ticketInOrderRepository.Insert(ticketInOrder);
                    result.Add(ticketInOrder); 
                    
                }

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var currentItem = result[i - 1];
                    Ticket ticket = allTickets[i - 1];
                    currentItem.Ticket = ticket;
                    totalPrice += currentItem.Quantity * currentItem.Ticket.Price;
                    sb.AppendLine(i.ToString() + ". " + currentItem.Ticket.Movie + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Ticket.Price);
                }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString());

                mail.Content = sb.ToString();


                ticketInOrders.AddRange(result);

                foreach (var item in ticketInOrders)
                {
                    this.ticketInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.TicketInShoppingCarts.Clear();

                this.userRepository.Update(loggedInUser);
                this.mailRepository.Insert(mail);

                return true;
            }

            return false;
        }

        public void removeTicketById(Guid ticketId)
        {
            this.shoppingCartRepository.removeTicketById(ticketId);
        }
    }
}
