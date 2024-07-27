using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;
using APIPhonebook.Models;
using APIPhonebook.Services.Contract;
using APIPhonebook.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIPhonebookTests.Services
{
    public class AuthServiceTests
    {
        // GetSecurityQuestions()
        [Fact]
        public void GetSecurityQuestions_ReturnsQuestions_WhenQuestionsExist()

        {
            // Arrange
            var questions = new List<SecurityQuestion>()
            {
                new SecurityQuestion()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                new SecurityQuestion()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var expectedResponse = new ServiceResponse<IEnumerable<SecurityQuestionDto>>()
            {
                Success = true,
                Message = "Success",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(c => c.GetSecurityQuestions()).Returns(questions);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.GetSecurityQuestions();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(c => c.GetSecurityQuestions(), Times.Once);
        }

        [Fact]
        public void GetSecurityQuestions_ReturnsFalse_WhenSecurityQuestionsAreNull()
        {
            // Arrange
            List<SecurityQuestion> questions = null;

            var expectedResponse = new ServiceResponse<IEnumerable<SecurityQuestionDto>>()
            {
                Success = false,
                Message = "Failed to retrieve security questions",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(c => c.GetSecurityQuestions()).Returns(questions);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.GetSecurityQuestions();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(c => c.GetSecurityQuestions(), Times.Once);
        }

        [Fact]
        public void GetSecurityQuestions_ReturnsFalse_WhenNoQuestionsExist()
        {
            // Arrange
            List<SecurityQuestion> questions = new List<SecurityQuestion>();

            var expectedResponse = new ServiceResponse<IEnumerable<SecurityQuestionDto>>()
            {
                Success = false,
                Message = "Failed to retrieve security questions",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(c => c.GetSecurityQuestions()).Returns(questions);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.GetSecurityQuestions();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(c => c.GetSecurityQuestions(), Times.Once);
        }

        // GetUserDetails()
        [Fact]
        public void GetUserDetails_ReturnsUser_WhenUserIsFound()
        {
            // Arrange
            var loginId = "test";

            var user = new User()
            {
                UserId = 1,
                LoginId = "test",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@example.com",
                ContactNumber = "1234567890",
            };

            var expectedResponse = new ServiceResponse<UserDetailsDto>()
            {
                Success = true,
                Message = "Success",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(c => c.GetUserDetails(loginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.GetUserDetails(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(c => c.GetUserDetails(loginId), Times.Once);
        }

        [Fact]
        public void GetUserDetails_ReturnsFalse_WhenNoUserFound()
        {
            // Arrange
            var loginId = "test";

            User user = null;

            var expectedResponse = new ServiceResponse<UserDetailsDto>()
            {
                Success = false,
                Message = "Error fetching user details",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(c => c.GetUserDetails(loginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.GetUserDetails(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(c => c.GetUserDetails(loginId), Times.Once);
        }

        // GetUserSecurityQuestions()
        [Fact]
        public void GetUserSecurityQuestions_ReturnsQuestions_WhenUserIsFound()
        {
            // Arrange
            var loginId = "test";

            var user = new User()
            {
                UserId = 1,
                LoginId = "test",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@example.com",
                ContactNumber = "1234567890",
                SecurityQuestion_1 = new SecurityQuestion()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                SecurityQuestion_2 = new SecurityQuestion()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var expectedResponse = new ServiceResponse<UserSecurityQuestionsDto>()
            {
                Success = true,
                Message = "Success",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(c => c.GetUserSecurityQuestions(loginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.GetUserSecurityQuestions(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(c => c.GetUserSecurityQuestions(loginId), Times.Once);
        }

        [Fact]
        public void GetUserSecurityQuestions_ReturnsFalse_WhenNoUserIsFound()
        {
            // Arrange
            var loginId = "test";

            User user = null;

            var expectedResponse = new ServiceResponse<UserSecurityQuestionsDto>()
            {
                Success = false,
                Message = "No user with that username/email exists",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(c => c.GetUserSecurityQuestions(loginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.GetUserSecurityQuestions(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(c => c.GetUserSecurityQuestions(loginId), Times.Once);
        }

        // RegisterUserService()
        [Fact]
        public void RegisterUserService_ReturnsSuccess_WhenValidRegistration()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2"
            };
            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = string.Empty,
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(repo => repo.UsernameExists(registerDto.LoginId)).Returns(false);
            mockAuthRepository.Setup(repo => repo.EmailExists(registerDto.Email)).Returns(false);
            mockAuthRepository.Setup(repo => repo.ContactNumberExists(registerDto.ContactNumber)).Returns(false);

            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(repo => repo.UsernameExists(registerDto.LoginId), Times.Once);
            mockAuthRepository.Verify(repo => repo.EmailExists(registerDto.Email), Times.Once);
            mockAuthRepository.Verify(repo => repo.ContactNumberExists(registerDto.ContactNumber), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void RegisterUserService_ReturnsError_WhenRegistrationFails()
        {
            var registerDto = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2"
            };
            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(repo => repo.UsernameExists(registerDto.LoginId)).Returns(false);
            mockAuthRepository.Setup(repo => repo.EmailExists(registerDto.Email)).Returns(false);
            mockAuthRepository.Setup(repo => repo.ContactNumberExists(registerDto.ContactNumber)).Returns(false);

            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(repo => repo.UsernameExists(registerDto.LoginId), Times.Once);
            mockAuthRepository.Verify(repo => repo.EmailExists(registerDto.Email), Times.Once);
            mockAuthRepository.Verify(repo => repo.ContactNumberExists(registerDto.ContactNumber), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void RegisterUserService_ReturnsError_WhenPasswordIsLessThan8Characters()
        {
            // Arrange
            var registerViewModel = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Test1!",
            };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = stringBuilder.ToString(),
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }

        [Fact]
        public void RegisterUserService_ReturnsError_WhenPasswordDoesNotContainAlphanumeric()
        {
            // Arrange
            var registerViewModel = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "!!!!!!!!",
            };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = stringBuilder.ToString(),
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }

        [Fact]
        public void RegisterUserService_ReturnsError_WhenPasswordDoesNotContainSpecialCharacters()
        {
            // Arrange
            var registerViewModel = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password1",
            };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = stringBuilder.ToString(),
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }

        [Fact]
        public void RegisterUserService_ReturnsError_WhenUsernameIsNotAvailable()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2"
            };
            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Username is not available",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(repo => repo.UsernameExists(registerDto.LoginId)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.UsernameExists(registerDto.LoginId), Times.Once);
        }

        [Fact]
        public void RegisterUserService_ReturnsError_WhenEmailIsNotAvailable()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2"
            };
            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Email is already in use",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(repo => repo.UsernameExists(registerDto.LoginId)).Returns(false);
            mockAuthRepository.Setup(repo => repo.EmailExists(registerDto.Email)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.UsernameExists(registerDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.EmailExists(registerDto.Email), Times.Once);
        }

        [Fact]
        public void RegisterUserService_ReturnsError_WhenContactNumberIsNotAvailable()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "test@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2"
            };
            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Contact number is already in use",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(repo => repo.UsernameExists(registerDto.LoginId)).Returns(false);
            mockAuthRepository.Setup(repo => repo.EmailExists(registerDto.Email)).Returns(false);
            mockAuthRepository.Setup(repo => repo.ContactNumberExists(registerDto.ContactNumber)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.UsernameExists(registerDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.EmailExists(registerDto.Email), Times.Once);
            mockAuthRepository.Verify(o => o.ContactNumberExists(registerDto.ContactNumber), Times.Once);
        }

        [Fact]
        public void RegisterUserService_ReturnsEmptyString_WhenViewModelIsNull()
        {
            // Arrange
            RegisterDto registerViewModel = null;
            var response = new ServiceResponse<string>();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
        }

        // LoginUserService()
        [Fact]
        public void LoginUserService_ReturnsErrorMessage_WhenLoginIsNull()
        {
            // Arrange
            LoginDto loginDto = null;
            var errorMessage = "Something went wrong, please try after some time";

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, actual.Message);
        }

        [Fact]
        public void LoginUserService_ReturnsError_WhenUsernameIsInvalid()
        {
            // Arrange
            LoginDto loginDto = new LoginDto()
            {
                Username = "username",
                Password = "Password@123",
            };
            var errorMessage = "Invalid username or password";

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage,
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(loginDto.Username)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(loginDto.Username), Times.Once);
        }

        [Fact]
        public void LoginUserService_ReturnsError_WhenPasswordIsInvalid()
        {
            // Arrange
            LoginDto loginViewModel = new LoginDto()
            {
                Username = "username",
                Password = "Password@123",
            };
            var errorMessage = "Invalid username or password";

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage,
            };

            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(loginViewModel.Username)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(loginViewModel.Username), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void LoginUserService_ReturnsToken_WhenLoginSuccessful()
        {
            // Arrange
            LoginDto loginViewModel = new LoginDto()
            {
                Username = "username",
                Password = "Password@123",
            };
            var token = "fakeToken";

            var response = new ServiceResponse<string>
            {
                Data = token,
                Success = true,
            };
            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(loginViewModel.Username)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockTokenService.Setup(o => o.CreateToken(user)).Returns(token);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(token, actual.Data);
            mockTokenService.Verify(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockTokenService.Verify(o => o.CreateToken(user), Times.Once);
        }

        // UpdateUserDetails()
        [Fact]
        public void UpdateUserDetails_ReturnsToken_WhenUserDetailsAreUpdatedSuccessfully()
        {
            // Arrange
            UpdateUserDetailsDto updateDto = new UpdateUserDetailsDto()
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "newLoginId",
                ContactNumber = "0987654321",
            };
            var token = "fakeToken";

            var response = new ServiceResponse<string>
            {
                Data = token,
                Success = true,
                Message = "Success",
            };
            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.UsernameExists(updateDto.userId, updateDto.LoginId)).Returns(false);
            mockAuthRepository.Setup(o => o.ContactNumberExists(updateDto.userId, updateDto.ContactNumber)).Returns(false);
            mockAuthRepository.Setup(o => o.GetUserById(updateDto.userId)).Returns(user);
            mockAuthRepository.Setup(o => o.UpdateUser(It.IsAny<User>())).Returns(true);
            mockTokenService.Setup(o => o.CreateToken(It.IsAny<User>())).Returns(token);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.UpdateUserDetails(updateDto);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(token, actual.Data);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.UsernameExists(updateDto.userId, updateDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.ContactNumberExists(updateDto.userId, updateDto.ContactNumber), Times.Once);
            mockAuthRepository.Verify(o => o.GetUserById(updateDto.userId), Times.Once);
            mockAuthRepository.Verify(o => o.UpdateUser(It.IsAny<User>()), Times.Once);
            mockTokenService.Verify(o => o.CreateToken(user), Times.Once);
        }

        [Fact]
        public void UpdateUserDetails_ReturnsFalse_WhenUsernameIsNotAvailable()
        {
            // Arrange
            UpdateUserDetailsDto updateDto = new UpdateUserDetailsDto()
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "newLoginId",
                ContactNumber = "0987654321",
            };

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = "Username is not available",
            };
            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.UsernameExists(updateDto.userId, updateDto.LoginId)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.UpdateUserDetails(updateDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.UsernameExists(updateDto.userId, updateDto.LoginId), Times.Once);
        }

        [Fact]
        public void UpdateUserDetails_ReturnsFalse_WhenContactNumberIsUnavailable()
        {
            // Arrange
            UpdateUserDetailsDto updateDto = new UpdateUserDetailsDto()
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "newLoginId",
                ContactNumber = "0987654321",
            };

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = "Contact number is already in use",
            };
            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.UsernameExists(updateDto.userId, updateDto.LoginId)).Returns(false);
            mockAuthRepository.Setup(o => o.ContactNumberExists(updateDto.userId, updateDto.ContactNumber)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.UpdateUserDetails(updateDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.UsernameExists(updateDto.userId, updateDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.ContactNumberExists(updateDto.userId, updateDto.ContactNumber), Times.Once);
        }

        [Fact]
        public void UpdateUserDetails_ReturnsFalse_WhenUpdationFails()
        {
            // Arrange
            UpdateUserDetailsDto updateDto = new UpdateUserDetailsDto()
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "newLoginId",
                ContactNumber = "0987654321",
            };

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };
            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.UsernameExists(updateDto.userId, updateDto.LoginId)).Returns(false);
            mockAuthRepository.Setup(o => o.ContactNumberExists(updateDto.userId, updateDto.ContactNumber)).Returns(false);
            mockAuthRepository.Setup(o => o.GetUserById(updateDto.userId)).Returns(user);
            mockAuthRepository.Setup(o => o.UpdateUser(It.IsAny<User>())).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.UpdateUserDetails(updateDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.UsernameExists(updateDto.userId, updateDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.ContactNumberExists(updateDto.userId, updateDto.ContactNumber), Times.Once);
            mockAuthRepository.Verify(o => o.GetUserById(updateDto.userId), Times.Once);
            mockAuthRepository.Verify(o => o.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        // ChangePassword()
        [Fact]
        public void ChangePassword_ReturnsTrue_WhenPasswordIsChangedSuccessfully()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Password@123",
                NewPassword = "Password@1234",
                ConfirmNewPassword = "Password@1234",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Success",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(o => o.UpdateUser(user)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockAuthRepository.Verify(o => o.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsFalse_WhenChangePasswordFails()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Password@123",
                NewPassword = "Password@1234",
                ConfirmNewPassword = "Password@1234",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(o => o.UpdateUser(user)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockAuthRepository.Verify(o => o.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsFalse_WhenNewPasswordIsInvalid()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Password@123",
                NewPassword = "Pass1!",
                ConfirmNewPassword = "Pass1!",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = stringBuilder.ToString(),
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsFalse_WhenOldPasswordIsIncorrect()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Password@123",
                NewPassword = "Password@1234",
                ConfirmNewPassword = "Password@1234",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Old password is incorrect",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsFalse_WhenOldPasswordMatchesNewPassword()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Password@123",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "New password cannot match old password",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsFalse_WhenUserIsNotFound()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto()
            {
                LoginId = "Test",
                OldPassword = "Password@123",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            User user = null;

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsFalse_WhenDtoIsNull()
        {
            // Arrange
            ChangePasswordDto changePasswordDto = null;

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }

        // ResetPassword()
        [Fact]
        public void ResetPassword_ReturnsTrue_WhenPasswordIsResetSuccessfully()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityQuestionId_2 = 2,
                SecurityAnswerHash_1 = new byte[2],
                SecurityAnswerSalt_1 = new byte[2],
                SecurityAnswerHash_2 = new byte[2],
                SecurityAnswerSalt_2 = new byte[2],
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Success",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(resetPasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(o => o.UpdateUser(user)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(resetPasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Exactly(2));
            mockAuthRepository.Verify(o => o.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void ResetPassword_ReturnsFalse_WhenResetPasswordFails()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityQuestionId_2 = 2,
                SecurityAnswerHash_1 = new byte[2],
                SecurityAnswerSalt_1 = new byte[2],
                SecurityAnswerHash_2 = new byte[2],
                SecurityAnswerSalt_2 = new byte[2],
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(resetPasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(o => o.UpdateUser(user)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(resetPasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Exactly(2));
            mockAuthRepository.Verify(o => o.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void ResetPassword_ReturnsFalse_WhenNewPasswordIsInvalid()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Pass1!",
                ConfirmNewPassword = "Pass1!",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityQuestionId_2 = 2,
                SecurityAnswerHash_1 = new byte[2],
                SecurityAnswerSalt_1 = new byte[2],
                SecurityAnswerHash_2 = new byte[2],
                SecurityAnswerSalt_2 = new byte[2],
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = stringBuilder.ToString(),
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(resetPasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(resetPasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Exactly(2));
        }

        [Fact]
        public void ResetPassword_ReturnsFalse_WhenSecurityAnswersAreIncorrect()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityQuestionId_2 = 2,
                SecurityAnswerHash_1 = new byte[2],
                SecurityAnswerSalt_1 = new byte[2],
                SecurityAnswerHash_2 = new byte[2],
                SecurityAnswerSalt_2 = new byte[2],
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Verification failed",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(resetPasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(resetPasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Exactly(1));
        }

        [Fact]
        public void ResetPassword_ReturnsFalse_WhenSecurityQuestionIdsAreWrong()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            var user = new User()
            {
                UserId = 1,
                LoginId = "Test",
                SecurityQuestionId_1 = 3,
                SecurityQuestionId_2 = 3,
                SecurityAnswerHash_1 = new byte[2],
                SecurityAnswerSalt_1 = new byte[2],
                SecurityAnswerHash_2 = new byte[2],
                SecurityAnswerSalt_2 = new byte[2],
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2],
            };

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(resetPasswordDto.LoginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(resetPasswordDto.LoginId), Times.Once);
        }

        [Fact]
        public void ResetPassword_ReturnsFalse_WhenUserValidationFails()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto()
            {
                LoginId = "Test",
                SecurityQuestionId_1 = 1,
                SecurityAnswer_1 = "Answer 1",
                SecurityQuestionId_2 = 2,
                SecurityAnswer_2 = "Answer 2",
                NewPassword = "Password@123",
                ConfirmNewPassword = "Password@123",
            };

            User user = null;

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(resetPasswordDto.LoginId)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(resetPasswordDto.LoginId), Times.Once);
        }

        [Fact]
        public void ResetPassword_ReturnsFalse_WhenDtoIsNull()
        {
            // Arrange
            ResetPasswordDto resetPasswordDto = null;
            
            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong.",
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ResetPassword(resetPasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }
    }
}
