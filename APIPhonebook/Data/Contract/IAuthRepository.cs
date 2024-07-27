using APIPhonebook.Models;

namespace APIPhonebook.Data.Contract
{
    public interface IAuthRepository
    {
        IEnumerable<SecurityQuestion> GetSecurityQuestions();
        bool RegisterUser(User user);
        User GetUserById(int userId);
        User GetUserDetails(string loginId);
        User GetUserSecurityQuestions(string username);
        bool UpdateUser(User user);
        User ValidateUser(string username);
        bool UsernameExists(string loginId);
        bool UsernameExists(int userId, string loginId);
        bool EmailExists(string email);
        bool ContactNumberExists(string contactNumber);
        bool ContactNumberExists(int userId, string contactNumber);
        //string UserExists(int userId, string loginId, string email, string contactNumber);
    }
}
