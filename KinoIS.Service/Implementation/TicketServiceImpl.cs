using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Implementation
{
    public class TicketServiceImpl : TicketService
    {
        public TicketRepository ticketRepository;
        public TicketServiceImpl(TicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }
        public Ticket addTicket(Ticket ticket)
        {
            return this.ticketRepository.addTicket(ticket);
        }

        public void deleteTicket(Ticket ticket)
        {
            this.ticketRepository.deleteTicket(ticket);
        }

        public List<Ticket> findAll()
        {
            return this.ticketRepository.findAll();
        }

        public List<Ticket> findAllByDate(DateTime date)
        {
            return this.ticketRepository.findAllByDate(date);
        }

        public List<Ticket> findByGenre(string genre)
        {
            return this.ticketRepository.findByGenre(genre);
        }

        public Ticket findById(Guid id)
        {
            return this.ticketRepository.findById(id);
        }
    }
}
