using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoIS.Domain.Models;
using KinoIS.Service.Interface;
using System.Security.Claims;
using KinoIS.Domain.Relations;
using Stripe;
using DocumentFormat.OpenXml.Spreadsheet;
using KinoIs.Repository;

namespace KinoIS.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService shoppingCartService;
        private readonly TicketInShoppingCartService ticketInShoppingCartService;
        private readonly CinemaUserService cinemaUserService;
        private readonly TicketService ticketService;

        public ShoppingCartsController(ShoppingCartService shoppingCartService, TicketInShoppingCartService ticketInShoppingCartService,
            ApplicationDbContext context, TicketService ticketService)
        {
            this.ticketInShoppingCartService = ticketInShoppingCartService;
            this.shoppingCartService = shoppingCartService;
            _context = context;
            this.ticketService = ticketService;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.shoppingCarts.Include(s => s.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Details (string email)
        {
            CinemaUser user = this._context.users.Where(x => x.Email.Equals(email)).FirstOrDefault();
            ShoppingCart shoppingCart = this.shoppingCartService.findByOwnerId(user.Id);

            List<TicketInShoppingCart> idsOfTicketsInShoppingCart = this.ticketInShoppingCartService.findAllByShoppingCartId(shoppingCart.Id);
            ViewBag.idsOftTicketsInShoppingCart = idsOfTicketsInShoppingCart;

            List<Ticket> tickets = new List<Ticket>();
            foreach(var item in idsOfTicketsInShoppingCart)
            {
                tickets.Add(this.ticketService.findById(item.TicketId));
            }

            ViewBag.tickets = tickets;
            return View(shoppingCart);

        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.shoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,OwnerId")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.shoppingCarts
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shoppingCart = await _context.shoppingCarts.FindAsync(id);
            _context.shoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(Guid id)
        {
            return _context.shoppingCarts.Any(e => e.Id == id);
        }

        public Boolean Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this.shoppingCartService.order(userId);

            return result;
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this.shoppingCartService.findByOwnerId(userId); 
            

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100), 
                Description = "KinoIS Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.Order();

                if (result)
                {
                    var orderCompleted = true;
                    return RedirectToAction( "Index", "Tickets", new {orderCompleted});
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            return RedirectToAction("Index", "Ticket");
        }
        public IActionResult RemoveTicket(Guid ticketId, Guid scId)
        {
            this.ticketInShoppingCartService.removeTicket(ticketId, scId);
            return RedirectToAction("Details", "ShoppingCarts", new { email = User.Identity.Name });
        }
    }
}
