using APIPhonebook.Dtos;

namespace APIPhonebook.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<IEnumerable<SecurityQuestionDto>> GetSecurityQuestions();
        ServiceResponse<UserDetailsDto> GetUserDetails(string loginId);
        ServiceResponse<UserSecurityQuestionsDto> GetUserSecurityQuestions(string username);
        ServiceResponse<string> RegisterUserService(RegisterDto register);
        ServiceResponse<string> LoginUserService(LoginDto login);
        ServiceResponse<string> UpdateUserDetails(UpdateUserDetailsDto updateUserDetailsDto);
        ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto);
        ServiceResponse<string> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
