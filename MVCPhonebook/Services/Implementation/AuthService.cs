using Microsoft.IdentityModel.Tokens;
using MVCPhonebook.Data.Contract;
using MVCPhonebook.Models;
using MVCPhonebook.Services.Contract;
using MVCPhonebook.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCPhonebook.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        public string RegisterUserService(RegisterViewModel register)
        {
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return message;
                }
                else if (_authRepository.UserExists(register.LoginId, register.Email))
                {
                    message = "User already exists.";
                    return message;
                }
                else
                {
                    User user = new User()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        LoginId = register.LoginId,
                        ContactNumber = register.ContactNumber,
                    };

                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    message = result ? string.Empty : "Something went wrong, please try after some time.";
                }
            }
            return message;
        }

        public string LoginUserService(LoginViewModel login)
        {
            string message;
            if (login != null)
            {
                message = "Invalid username or password";
                var user = _authRepository.ValidateUser(login.Username);
                if (user == null)
                {
                    return message;
                }
                else if (!_tokenService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return message;
                }

                string token = _tokenService.CreateToken(user);

                return token;
            }

            message = "Something went wrong, please try after some time";
            return message;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private string CheckPasswordStrength(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (password.Length < 8)
            {
                stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,*,(,),_,]"))
            {
                stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
    }
}
