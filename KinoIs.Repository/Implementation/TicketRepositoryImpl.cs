using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinoIs.Repository.Implementation
{
    public class TicketRepositoryImpl : TicketRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Ticket> entities;
        public TicketRepositoryImpl(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Ticket>();
        }
        public Ticket addTicket(Ticket ticket)
        {
            this.entities.Add(ticket);
            return ticket;
        }

        public void deleteTicket(Ticket ticket)
        {
            this.entities.Remove(ticket);
        }

        public List<Ticket> findAll()
        {
            return this.context.tickets.ToList();
        }

        public List<Ticket> findAllByDate(DateTime dateParam)
        {
            return this.context.tickets.Where(x => x.date.Date.Equals(dateParam)).ToList();
        }

        public List<Ticket> findByGenre(string genre)
        {
            return this.context.tickets.Where(x => x.Genre.Equals(genre)).ToList();
        }

        public Ticket findById(Guid id)
        {
            return this.context.tickets.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }
    }
}
