﻿using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface TicketRepository
    {
        Ticket addTicket(Ticket ticket);
        void deleteTicket(Ticket ticket);
        List<Ticket> findAll();
        List<Ticket> findAllByDate(DateTime date);
        Ticket findById(Guid id);
        List<Ticket> findByGenre(string genre);
    }
}
