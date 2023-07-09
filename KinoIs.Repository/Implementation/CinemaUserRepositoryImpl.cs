using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinoIs.Repository.Implementation
{
    public class CinemaUserRepositoryImpl : CinemaUserRepository
        
    {
        private readonly ApplicationDbContext context;
        public CinemaUserRepositoryImpl (ApplicationDbContext context)
        {
            this.context = context;
        }

        public CinemaUser findById(string id)
        {
            return this.context.users.Where(x => x.Id == id).FirstOrDefault();
        }

        public CinemaUser Update(CinemaUser user)
        {
            this.context.users.Update(user);
            this.context.SaveChanges();
            return user;
        }
        public CinemaUser findByEmail(string email)
        {
            return this.context.users.Where(x => x.Email.Equals(email)).FirstOrDefault();
        }

        public void Save(CinemaUser user)
        {
            this.context.users.Add(user);
            this.context.SaveChanges();
        }
    }
}
