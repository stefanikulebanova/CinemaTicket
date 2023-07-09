using KinoIS.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public CinemaUser User { get; set; }

        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
