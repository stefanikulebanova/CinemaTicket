using System;
using System.Collections.Generic;
using System.Text;
using KinoIS.Domain.Models;

namespace KinoIS.Service.Interface
{
    public interface CinemaUserService
    {
        CinemaUser findById(string id);
        CinemaUser findByEmail(string email);
        void Save (CinemaUser user);
    }
}
