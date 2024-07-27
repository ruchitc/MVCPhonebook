using APIPhonebook.Dtos;
using APIPhonebook.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhonebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("GetUserDetails/{loginId}")]
        public IActionResult GetUserDetails(string loginId)
        {
            var response = _authService.GetUserDetails(loginId);
            if(!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPut("UpdateUserDetails")]
        public IActionResult UpdateUserDetails(UpdateUserDetailsDto updateUserDetailsDto)
        {
            var response = _authService.UpdateUserDetails(updateUserDetailsDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = _authService.ChangePassword(changePasswordDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetSecurityQuestions")]
        public IActionResult GetSecurityQuestions()
        {
            var response = _authService.GetSecurityQuestions();
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetUserSecurityQuestions/{username}")]
        public IActionResult GetUserSecurityQuestions(string username)
        {
            var response = _authService.GetUserSecurityQuestions(username);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var response = _authService.RegisterUserService(registerDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var response = _authService.LoginUserService(loginDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [AllowAnonymous]
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = _authService.ResetPassword(resetPasswordDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
    }
}
