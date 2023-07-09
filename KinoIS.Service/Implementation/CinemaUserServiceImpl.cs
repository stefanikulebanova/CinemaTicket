using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Implementation
{
    public class CinemaUserServiceImpl : CinemaUserService
    {
        private readonly CinemaUserRepository kinoUserRepository;
        public CinemaUserServiceImpl(CinemaUserRepository kinoUserRepository)
        {
            this.kinoUserRepository = kinoUserRepository;
        }
        public CinemaUser findById(string id)
        {
            return this.kinoUserRepository.findById(id);
        }
        public CinemaUser findByEmail(string email)
        {
            return this.kinoUserRepository.findByEmail(email);
        }

        public void Save(CinemaUser user)
        {
            this.kinoUserRepository.Save(user);
        }
    }
}
