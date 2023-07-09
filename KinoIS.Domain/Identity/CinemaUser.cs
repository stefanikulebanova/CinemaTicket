using KinoIS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Domain.Models 
{
    public class CinemaUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public virtual ShoppingCart UserCart { get; set; }
    }
}
