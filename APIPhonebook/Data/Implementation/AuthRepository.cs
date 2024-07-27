using APIPhonebook.Data.Contract;
using APIPhonebook.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPhonebook.Data.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IAppDbContext _appDbContext;

        public AuthRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<SecurityQuestion> GetSecurityQuestions()
        {
            return _appDbContext.SecurityQuestions.ToList();
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

        public User GetUserById(int userId)
        {
            User? user = _appDbContext.Users.FirstOrDefault(c => c.UserId == userId);
            return user;
        }

        public User GetUserDetails(string loginId)
        {
            User? user = _appDbContext.Users.FirstOrDefault(c => c.LoginId.ToLower() == loginId.ToLower() || c.Email.ToLower() == loginId.ToLower());
            return user;
        }

        public User GetUserSecurityQuestions(string username)
        {
            var user = _appDbContext.Users
                        .Include(s => s.SecurityQuestion_1)
                        .Include(s => s.SecurityQuestion_2)
                        .FirstOrDefault(c => c.LoginId.ToLower() == username.ToLower() || c.Email.ToLower() == username.ToLower());

            return user;
        }

        public bool UpdateUser(User user)
        {
            var result = false;
            if (user != null)
            {
                _appDbContext.Users.Update(user);
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public User ValidateUser(string username)
        {
            User? user = _appDbContext.Users.FirstOrDefault(c => c.LoginId.ToLower() == username.ToLower() || c.Email == username.ToLower());
            return user;
        }

        public bool UsernameExists(string loginId)
        {
            if(_appDbContext.Users.Any(u => u.LoginId.ToLower() == loginId.ToLower()))
            {
                return true;
            }
            return false;
        }

        public bool UsernameExists(int userId, string loginId)
        {
            if (_appDbContext.Users.Any(c => c.UserId != userId && c.LoginId.ToLower() == loginId.ToLower()))
            {
                return true;
            }
            return false;
        }

        public bool EmailExists(string email)
        {
            if (_appDbContext.Users.Any(u => u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }

        public bool ContactNumberExists(string contactNumber)
        {
            if (_appDbContext.Users.Any(u => u.ContactNumber.ToLower() == contactNumber.ToLower()))
            {
                return true;
            }
            return false;
        }

        public bool ContactNumberExists(int userId, string contactNumber)
        {
            if (_appDbContext.Users.Any(c => c.UserId != userId && c.ContactNumber.ToLower() == contactNumber.ToLower()))
            {
                return true;
            }
            return false;
        }

        //public string UserExists(int userId, string loginId, string email, string contactNumber)
        //{
        //    if (_appDbContext.Users.Any(c => c.UserId != userId && c.LoginId.ToLower() == loginId.ToLower()))
        //    {
        //        return "Username is not available";
        //    }
        //    if (_appDbContext.Users.Any(c => c.UserId != userId && c.Email.ToLower() == email.ToLower()))
        //    {
        //        return "Email is already in use";
        //    }
        //    if (_appDbContext.Users.Any(c => c.UserId != userId && c.ContactNumber.ToLower() == contactNumber.ToLower()))
        //    {
        //        return "Contact number is already in use";
        //    }
        //    return "";
        //}
    }
}
