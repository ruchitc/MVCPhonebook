﻿using MVCPhonebook.Data.Contract;
using MVCPhonebook.Models;

namespace MVCPhonebook.Data.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IAppDbContext _appDbContext;

        public AuthRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool RegisterUser(User user)
        {
            var result = false;
            if (user != null)
            {
                _appDbContext.Users.Add(user);
                _appDbContext.SaveChanges();

                result = true;
            }

            return result;
        }

        public User ValidateUser(string username)
        {
            User user = _appDbContext.Users.FirstOrDefault(c => c.LoginId.ToLower() == username.ToLower() || c.Email == username.ToLower());
            return user;
        }

        public bool UserExists(string loginId, string email)
        {
            if (_appDbContext.Users.Any(c => c.LoginId.ToLower() == loginId.ToLower() || c.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}