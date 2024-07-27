using APIPhonebook.Controllers;
using APIPhonebook.Dtos;
using APIPhonebook.Services.Contract;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIPhonebookTests.Controllers
{
    public class AuthControllerTests
    {
        // GetUserDetails()
        [Fact]
        public void GetUserDetails_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            var loginId = "test";
            var user = new UserDetailsDto()
            {
                LoginId = "test",
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
                Email = "test@example.com",
            };

            var response = new ServiceResponse<UserDetailsDto>()
            {
                Data = user,
                Success = true,
                Message = "",
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(c => c.GetUserDetails(loginId)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.GetUserDetails(loginId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUserDetails(loginId), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            var loginId = "test";

            var response = new ServiceResponse<UserDetailsDto>()
            {
                Success = false,
                Message = "",
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(c => c.GetUserDetails(loginId)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.GetUserDetails(loginId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUserDetails(loginId), Times.Once);
        }

        // UpdateUserDetails()
        [Fact]
        public void UpdateUserDetails_ReturnsOk_WhenResponseIsSuccess()
        {
            // Arrange
            var fixture = new Fixture();
            var updateDto = fixture.Create<UpdateUserDetailsDto>();
            var response = new ServiceResponse<string>()
            {
                Success = true,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.UpdateUserDetails(updateDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.UpdateUserDetails(updateDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            mockAuthService.Verify(o => o.UpdateUserDetails(updateDto), Times.Once);
        }

        [Fact]
        public void UpdateUserDetails_ReturnsBadRequest_WhenResponseIsFailure()
        {
            // Arrange
            var fixture = new Fixture();
            var updateDto = fixture.Create<UpdateUserDetailsDto>();
            var response = new ServiceResponse<string>()
            {
                Success = false,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.UpdateUserDetails(updateDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.UpdateUserDetails(updateDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            mockAuthService.Verify(o => o.UpdateUserDetails(updateDto), Times.Once);
        }

        // ChangePassword()
        [Fact]
        public void ChangePassword_ReturnsOk_WhenResponseIsSuccess()
        {
            // Arrange
            var fixture = new Fixture();
            var updateDto = fixture.Create<ChangePasswordDto>();
            var response = new ServiceResponse<string>()
            {
                Success = true,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.ChangePassword(updateDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ChangePassword(updateDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            mockAuthService.Verify(o => o.ChangePassword(updateDto), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsBadRequest_WhenResponseIsFailure()
        {
            // Arrange
            var fixture = new Fixture();
            var updateDto = fixture.Create<ChangePasswordDto>();
            var response = new ServiceResponse<string>()
            {
                Success = false,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.ChangePassword(updateDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ChangePassword(updateDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            mockAuthService.Verify(o => o.ChangePassword(updateDto), Times.Once);
        }

        // GetSecurityQuestions()
        [Fact]
        public void GetSecurityQuestions_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            var questions = new List<SecurityQuestionDto>()
            {
                new SecurityQuestionDto()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                new SecurityQuestionDto()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var response = new ServiceResponse<IEnumerable<SecurityQuestionDto>>()
            {
                Data = questions,
                Success = true,
                Message = "",
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(c => c.GetSecurityQuestions()).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.GetSecurityQuestions() as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetSecurityQuestions(), Times.Once);
        }

        [Fact]
        public void GetSecurityQuestions_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            var response = new ServiceResponse<IEnumerable<SecurityQuestionDto>>()
            {
                Success = false,
                Message = "",
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(c => c.GetSecurityQuestions()).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.GetSecurityQuestions() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetSecurityQuestions(), Times.Once);
        }

        // GetUserSecurityQuestions()
        [Fact]
        public void GetUserSecurityQuestions_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            var loginId = "test";
            var user = new UserSecurityQuestionsDto()
            {
                LoginId = "test",
                SecurityQuestion_1 = new SecurityQuestionDto()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                SecurityQuestion_2 = new SecurityQuestionDto()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var response = new ServiceResponse<UserSecurityQuestionsDto>()
            {
                Data = user,
                Success = true,
                Message = "",
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(c => c.GetUserSecurityQuestions(loginId)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.GetUserSecurityQuestions(loginId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUserSecurityQuestions(loginId), Times.Once);
        }

        [Fact]
        public void GetUserSecurityQuestions_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            var loginId = "test";

            var response = new ServiceResponse<UserSecurityQuestionsDto>()
            {
                Success = false,
                Message = "",
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(c => c.GetUserSecurityQuestions(loginId)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.GetUserSecurityQuestions(loginId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUserSecurityQuestions(loginId), Times.Once);
        }

        // Register()
        [Fact]
        public void Register_ReturnsOk_WhenResponseIsSuccess()
        {
            // Arrange
            var fixture = new Fixture();
            var registerDto = fixture.Create<RegisterDto>();
            var response = new ServiceResponse<string>()
            {
                Success = true,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.RegisterUserService(registerDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            mockAuthService.Verify(o => o.RegisterUserService(registerDto), Times.Once);
        }

        [Fact]
        public void Register_ReturnsBadRequest_WhenResponseIsFailure()
        {
            // Arrange
            var fixture = new Fixture();
            var registerDto = fixture.Create<RegisterDto>();
            var response = new ServiceResponse<string>()
            {
                Success = false,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.RegisterUserService(registerDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            mockAuthService.Verify(o => o.RegisterUserService(registerDto), Times.Once);
        }

        // Login()
        [Fact]
        public void Login_ReturnsOk_WhenResponseIsSuccess()
        {
            // Arrange
            var fixture = new Fixture();
            var loginDto = fixture.Create<LoginDto>();
            var response = new ServiceResponse<string>()
            {
                Success = true,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.LoginUserService(loginDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            mockAuthService.Verify(o => o.LoginUserService(loginDto), Times.Once);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenResponseIsFailure()
        {
            // Arrange
            var fixture = new Fixture();
            var loginDto = fixture.Create<LoginDto>();
            var response = new ServiceResponse<string>()
            {
                Success = false,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.LoginUserService(loginDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            mockAuthService.Verify(o => o.LoginUserService(loginDto), Times.Once);
        }

        // ResetPassword()
        [Fact]
        public void ResetPassword_ReturnsOk_WhenResponseIsSuccess()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            var response = new ServiceResponse<string>()
            {
                Success = true,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.ResetPassword(resetPasswordDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            mockAuthService.Verify(o => o.ResetPassword(resetPasswordDto), Times.Once);
        }

        [Fact]
        public void ResetPassword_ReturnsBadRequest_WhenResponseIsFailure()
        {
            // Arrange
            var loginId = "test";
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };
            var response = new ServiceResponse<string>()
            {
                Success = false,
            };

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(o => o.ResetPassword(resetPasswordDto)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            mockAuthService.Verify(o => o.ResetPassword(resetPasswordDto), Times.Once);
        }
    }
}
