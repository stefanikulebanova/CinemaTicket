using ClosedXML.Excel;
using ExcelDataReader;
using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using KinoIS.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace KinoIS.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly CinemaUserService userService;
        private readonly TicketService ticketService;
        private readonly OrderService orderService;
        private readonly TicketInOrderService ticketInOrderService;
        public AdminController(CinemaUserService userService, TicketService ticketService, OrderService orderService,
            TicketInOrderService ticketInOrderService)
        {
            this.userService = userService; 
            this.ticketService = ticketService;
            this.orderService = orderService;
            this.ticketInOrderService = ticketInOrderService;
        }
        public IActionResult ExportAllOrders(string genre)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userService.findById(userId);
            if (user.Role == "Admin")
            {
                string fileName = "Orders.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                List<Order> result = new List<Order>();

                if (genre != null)
                {
                    foreach(TicketInOrder tio in this.ticketInOrderService.findAll())
                    {
                        Ticket ticket = this.ticketService.findById(tio.ProductId);
                        if (ticket.Genre == genre)
                        {
                            result.Add(this.orderService.findById(tio.OrderId));
                        }
                    }
                } 
                if (genre == "notSelected")
                {
                    foreach (TicketInOrder tio in this.ticketInOrderService.findAll())
                    {
                        result.Add(this.orderService.findById(tio.OrderId));
                    }
                }

                using (var workBook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workBook.Worksheets.Add("All Orders");

                    worksheet.Cell(1, 1).Value = "Order Id";
                    worksheet.Cell(1, 2).Value = "Costumer Name";
                    worksheet.Cell(1, 3).Value = "Costumer Last Name";
                    worksheet.Cell(1, 4).Value = "Costumer Email";

                    for (int i = 1; i <= result.Count(); i++)
                    {
                        var item = result[i - 1];

                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();

                        for (int p = 1; p <= item.TicketInOrders.Count(); p++)
                        {
                            worksheet.Cell(1, p + 4).Value = "Product-" + (p + 1);
                            worksheet.Cell(i + 1, p + 4).Value = item.TicketInOrders.ElementAt(p - 1).Ticket.Movie;
                        }

                    }

                    using (var stream = new MemoryStream())
                    {
                        workBook.SaveAs(stream);

                        var content = stream.ToArray();

                        return File(content, contentType, fileName);
                    }
                }
            }
            return RedirectToAction("Error", "Admin");
        }
        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ImportUsers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportUsers(IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (this.userService.findById(userId).Role == "Admin")
            {
                string pathToFile = $"{Directory.GetCurrentDirectory()}\\Files\\{file.FileName}";

                using (FileStream fileStream = System.IO.File.Create(pathToFile))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                List<CinemaUser> userList = new List<CinemaUser>();

                using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read())
                        {
                            var user = new CinemaUser
                            {
                                Email = reader.GetValue(0).ToString(),
                                Password = reader.GetValue(1).ToString(),
                                Role = reader.GetValue(2).ToString()
                            };
                            this.userService.Save(user);
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Error", "Admin");
        }
    }
}
