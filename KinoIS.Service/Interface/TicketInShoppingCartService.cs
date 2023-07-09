using KinoIS.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Interface
{
    public interface TicketInShoppingCartService
    {
        List<TicketInShoppingCart> findAll();
        List<TicketInShoppingCart> findAllByShoppingCartId(Guid id);
        TicketInShoppingCart add(Guid shoppingCartId, Guid ticketId);
        void removeTicket(Guid ticketId, Guid scId);
    }
}
