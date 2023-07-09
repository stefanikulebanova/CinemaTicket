using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface CinemaUserRepository
    {
        CinemaUser findById(string id);
        CinemaUser Update(CinemaUser user);
        CinemaUser findByEmail(string email);
        void Save(CinemaUser user);
    }
}
