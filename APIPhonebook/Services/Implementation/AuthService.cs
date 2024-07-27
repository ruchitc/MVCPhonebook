using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;
using APIPhonebook.Models;
using APIPhonebook.Services.Contract;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace APIPhonebook.Services.Implementation
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

        public ServiceResponse<IEnumerable<SecurityQuestionDto>> GetSecurityQuestions()
        {
            var response = new ServiceResponse<IEnumerable<SecurityQuestionDto>>();
            var securityQuestions = _authRepository.GetSecurityQuestions();

            if(securityQuestions != null && securityQuestions.Any())
            {
                List<SecurityQuestionDto> securityQuestionDtoList = new List<SecurityQuestionDto>();
                foreach(var securityQuestion in securityQuestions)
                {
                    SecurityQuestionDto securityQuestionDto = new SecurityQuestionDto();
                    securityQuestionDto.QuestionId = securityQuestion.QuestionId;
                    securityQuestionDto.Question = securityQuestion.Question;

                    securityQuestionDtoList.Add(securityQuestionDto);
                }

                response.Data = securityQuestionDtoList;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to retrieve security questions";
            }

            return response;
        }

        public ServiceResponse<UserDetailsDto> GetUserDetails(string loginId)
        {
            var response = new ServiceResponse<UserDetailsDto>();
            User user = _authRepository.GetUserDetails(loginId);

            if (user != null)
            {
                UserDetailsDto userDto = new UserDetailsDto()
                {
                    userId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    LoginId = user.LoginId,
                    Email = user.Email,
                    ContactNumber = user.ContactNumber,
                };

                response.Success = true;
                response.Data = userDto;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "Error fetching user details";
            }

            return response;
        }

        public ServiceResponse<UserSecurityQuestionsDto> GetUserSecurityQuestions(string username)
        {
            var response = new ServiceResponse<UserSecurityQuestionsDto>();
            User user = _authRepository.GetUserSecurityQuestions(username);

            if(user != null)
            {
                UserSecurityQuestionsDto userSecurityQuestionsDto = new UserSecurityQuestionsDto()
                {
                    LoginId = user.LoginId,
                    SecurityQuestion_1 = new SecurityQuestionDto()
                    {
                        QuestionId = user.SecurityQuestion_1.QuestionId,
                        Question = user.SecurityQuestion_1.Question,
                    },
                    SecurityQuestion_2 = new SecurityQuestionDto()
                    {
                        QuestionId = user.SecurityQuestion_2.QuestionId,
                        Question = user.SecurityQuestion_2.Question,
                    },
                };

                response.Success = true;
                response.Data = userSecurityQuestionsDto;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No user with that username/email exists";
            }

            return response;
        }

        public ServiceResponse<string> RegisterUserService(RegisterDto register)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                else if (_authRepository.UsernameExists(register.LoginId))
                {
                    response.Success = false;
                    response.Message = "Username is not available";
                    return response;
                }
                else if (_authRepository.EmailExists(register.Email))
                {
                    response.Success = false;
                    response.Message = "Email is already in use";
                    return response;
                }
                else if (_authRepository.ContactNumberExists(register.ContactNumber))
                {
                    response.Success = false;
                    response.Message = "Contact number is already in use";
                    return response;
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
                        SecurityQuestionId_1 = register.SecurityQuestionId_1,
                        SecurityQuestionId_2 = register.SecurityQuestionId_2,
                    };

                    // Trimming and converting to lowercase before encryption
                    // so that they can be matched
                    register.SecurityAnswer_1 = register.SecurityAnswer_1.Trim().ToLower();
                    register.SecurityAnswer_2 = register.SecurityAnswer_2.Trim().ToLower();

                    CreatePasswordHash(register.SecurityAnswer_1, out byte[] securityAnswerHash_1, out byte[] securityAnswerSalt_1);
                    user.SecurityAnswerHash_1 = securityAnswerHash_1;
                    user.SecurityAnswerSalt_1 = securityAnswerSalt_1;

                    CreatePasswordHash(register.SecurityAnswer_2, out byte[] securityAnswerHash_2, out byte[] securityAnswerSalt_2);
                    user.SecurityAnswerHash_2 = securityAnswerHash_2;
                    user.SecurityAnswerSalt_2 = securityAnswerSalt_2;

                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    var result = _authRepository.RegisterUser(user);
                    response.Success = result;
                    response.Message = result ? string.Empty : "Something went wrong, please try after some time.";
                }
            }
            return response;
        }

        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();
            if (login != null)
            {
                var user = _authRepository.ValidateUser(login.Username);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                else if (!_tokenService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }

                string token = _tokenService.CreateToken(user);
                response.Success = true;
                response.Data = token;
                response.Message = "Success";
                return response;
            }

            response.Success = false;
            response.Message = "Something went wrong, please try after some time";
            return response;
        }

        public ServiceResponse<string> UpdateUserDetails(UpdateUserDetailsDto updateUserDetailsDto)
        {
            var response = new ServiceResponse<string>();
            if (updateUserDetailsDto != null)
            {
                if (_authRepository.UsernameExists(updateUserDetailsDto.userId, updateUserDetailsDto.LoginId))
                {
                    response.Success = false;
                    response.Message = "Username is not available";
                    return response;
                }
                else if (_authRepository.ContactNumberExists(updateUserDetailsDto.userId, updateUserDetailsDto.ContactNumber))
                {
                    response.Success = false;
                    response.Message = "Contact number is already in use";
                    return response;
                }
                else
                {
                    var existingUser = _authRepository.GetUserById(updateUserDetailsDto.userId);
                    var result = false;
                    if(existingUser != null)
                    {
                        existingUser.FirstName = updateUserDetailsDto.FirstName;
                        existingUser.LastName = updateUserDetailsDto.LastName;
                        existingUser.LoginId = updateUserDetailsDto.LoginId;
                        existingUser.ContactNumber = updateUserDetailsDto.ContactNumber;
                    }

                    result = _authRepository.UpdateUser(existingUser);
                    if(result)
                    {
                        string token = _tokenService.CreateToken(existingUser);
                        response.Success = true;
                        response.Data = token;
                        response.Message = "Success";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Something went wrong, please try after some time.";
                    }
                }
            }
            return response;
        }

        public ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = new ServiceResponse<string>();
            if (changePasswordDto != null)
            {
                var user = _authRepository.ValidateUser(changePasswordDto.LoginId);
                if(user == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time.";
                    return response;
                }
                if(changePasswordDto.OldPassword == changePasswordDto.NewPassword)
                {
                    response.Success = false;
                    response.Message = "New password cannot match old password";
                    return response;
                }
                if (!_tokenService.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Old password is incorrect";
                    return response;
                }

                var message = CheckPasswordStrength(changePasswordDto.NewPassword);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }

                CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var result = _authRepository.UpdateUser(user);
                response.Success = result;
                response.Message = result ? "Success" : "Something went wrong, please try after some time.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong.";
            }
            return response;
        }

        public ServiceResponse<string> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = new ServiceResponse<string>();
            if (resetPasswordDto != null)
            {
                var user = _authRepository.ValidateUser(resetPasswordDto.LoginId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time.";
                    return response;
                }
                if(resetPasswordDto.SecurityQuestionId_1 != user.SecurityQuestionId_1 || resetPasswordDto.SecurityQuestionId_2 != user.SecurityQuestionId_2)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time.";
                    return response;
                }

                // Trimming and converting to lowercase
                resetPasswordDto.SecurityAnswer_1 = resetPasswordDto.SecurityAnswer_1.Trim().ToLower();
                resetPasswordDto.SecurityAnswer_2 = resetPasswordDto.SecurityAnswer_2.Trim().ToLower();
                
                if (!_tokenService.VerifyPasswordHash(resetPasswordDto.SecurityAnswer_1, user.SecurityAnswerHash_1, user.SecurityAnswerSalt_1) ||
                    !_tokenService.VerifyPasswordHash(resetPasswordDto.SecurityAnswer_2, user.SecurityAnswerHash_2, user.SecurityAnswerSalt_2))
                {
                    response.Success = false;
                    response.Message = "Verification failed";
                    return response;
                }

                var message = CheckPasswordStrength(resetPasswordDto.NewPassword);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }

                CreatePasswordHash(resetPasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var result = _authRepository.UpdateUser(user);
                response.Success = result;
                response.Message = result ? "Success" : "Something went wrong, please try after some time.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong.";
            }
            return response;
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
