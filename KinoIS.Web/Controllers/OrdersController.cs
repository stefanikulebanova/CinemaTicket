using GemBox.Document;
using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using KinoIS.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace KinoIS.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderService orderService;
        private readonly TicketService ticketService;
        private readonly TicketInOrderService ticketInOrderService;
        private readonly CinemaUserService cinemaUserService;
        public OrdersController(OrderService orderService, TicketService ticketService, 
            TicketInOrderService ticketInOrderService, CinemaUserService kinoUserService)
        {
            this.orderService = orderService;
            this.ticketService = ticketService;
            this.ticketInOrderService = ticketInOrderService;
            this.cinemaUserService = kinoUserService;
            ComponentInfo.SetLicense("FREE - LIMITED - KEY");
        }
        public IActionResult AllOrders(string email)
        {
            CinemaUser user = this.cinemaUserService.findByEmail(email);
            List<Order> orders = this.orderService.getAllOrdersByUserId(user.Id);
            ViewBag.Role = user.Role;
            List<Ticket> tickets = this.ticketService.findAll();
            List<string> genres = new List<string>();
            foreach(Ticket ticket in tickets)
            {
                genres.Add(ticket.Genre);
            }
            ViewBag.Genres = genres;
            return View(orders);
        }
        public IActionResult OrderDetails(Guid orderId)
        {
            List<Ticket> ticketInOrder = this.ticketInOrderService.ticketsInOrder(orderId);
            ViewBag.orderId = orderId;
            return View(ticketInOrder);
        }
        public IActionResult DeleteOrder(Guid orderId)
        {
            this.orderService.deleteOrder(orderId);
            return RedirectToAction("AllOrders", "Orders", new {email = User.Identity.Name});
        }

        public IActionResult CreateInvoice(Guid orderId)
        {

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

            var document = DocumentModel.Load(templatePath);

            var result = this.orderService.getOrderDetails(orderId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = this.cinemaUserService.findById(userId);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{CostumerEmail}}", user.Email);
            document.Content.Replace("{{CostumerInfo}}", (user.Name + " " +user.Surname));

            StringBuilder sb = new StringBuilder();

            var total = 0.0;

            List<Ticket> ticketInOrder = this.ticketInOrderService.ticketsInOrder(orderId);

            foreach (var item in ticketInOrder)
            {
                total += item.Quantity * item.Price;
                sb.AppendLine(item.Movie + " with quantity of: " + item.Quantity + " and price of: $" + item.Price);
            }

            document.Content.Replace("{{AllTickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", "$" + total.ToString());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());


            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
