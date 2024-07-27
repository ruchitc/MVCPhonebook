using APIPhonebook.Models;

namespace APIPhonebook.Services.Contract
{
    public interface ITokenService
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
