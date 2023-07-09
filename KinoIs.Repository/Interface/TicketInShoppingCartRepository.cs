using KinoIS.Domain.Models;
using KinoIS.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface TicketInShoppingCartRepository
    {
        List<TicketInShoppingCart> findAll();
        List<TicketInShoppingCart> findAllByShoppingCartId(Guid id);
        TicketInShoppingCart add(Guid shoppingCartId, Guid ticketId);
        List<Ticket> findAllTicketsByShoppingCartId(Guid id);
        void removeTicket(Guid ticketId, Guid scId);
    }
}
