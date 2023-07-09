using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Interface
{
    public interface ShoppingCartService
    {
        List<ShoppingCart> findAll();
        ShoppingCart findById(Guid id);
        ShoppingCart findByOwnerId(string ownerId);
        void deleteById(Guid id);
        ShoppingCart create(string ownerId);
        bool order(string userId);
        void removeTicketById(Guid ticketId);

    }
}
