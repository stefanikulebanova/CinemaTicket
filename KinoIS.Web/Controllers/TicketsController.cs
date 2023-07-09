using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoIS.Domain.Models;
using KinoIs.Repository.Interface;
using KinoIS.Service.Interface;
using System.Security.Claims;
using KinoIS.Domain.Relations;
using KinoIs.Repository;

namespace KinoIS.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketInShoppingCartService ticketInShoppingCartService;
        private readonly TicketRepository ticketRepository;
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService shoppingCartService;
        private readonly CinemaUserService cinemaUserService;

        public TicketsController(ApplicationDbContext context, TicketRepository ticketRepository, 
            TicketInShoppingCartService ticketInShoppingCartService, ShoppingCartService shoppingCartService, 
            CinemaUserService cinemaUserService)
        {
            _context = context;
            this.ticketRepository = ticketRepository;
            this.ticketInShoppingCartService = ticketInShoppingCartService;
            this.shoppingCartService = shoppingCartService;
            this.cinemaUserService = cinemaUserService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(bool? orderCompleted)
        {
            ViewBag.Role = this.cinemaUserService.findById(User.FindFirstValue(ClaimTypes.NameIdentifier)).Role;
            if (orderCompleted == true) ViewBag.OrderCompleted = true;
            return View(await _context.tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Movie,Quantity,Genre,date,Price")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                ticket.Quantity = 1;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Movie,Quantity,Genre,date,Price")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _context.tickets.FindAsync(id);
            _context.tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _context.tickets.Any(e => e.Id == id);
        }
        [HttpPost]
        public IActionResult SearchByDate(DateTime date) 
        {
            List<Ticket> tickets = this.ticketRepository.findAllByDate(date);
            return View("Index", tickets);
        }

        public IActionResult AddToCart(Guid ticketId)
        {
            ShoppingCart shoppingCart = this.shoppingCartService.findByOwnerId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            TicketInShoppingCart tisc = this.ticketInShoppingCartService.add(shoppingCart.Id, ticketId);
            shoppingCart.TicketInShoppingCarts.Add(tisc);
            return RedirectToAction(nameof(Index));
        }
    }
}
