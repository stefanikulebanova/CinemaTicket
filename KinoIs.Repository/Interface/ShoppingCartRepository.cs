using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface ShoppingCartRepository
    {
        List<ShoppingCart> findAll();
        ShoppingCart findById(Guid id);
        ShoppingCart findByOwnerId(string ownerId);
        void deleteById(Guid id);
        ShoppingCart create(string ownerId);
        void removeTicketById(Guid ticketId);
    }
}
