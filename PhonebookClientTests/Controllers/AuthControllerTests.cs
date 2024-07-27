using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using PhonebookClient.Controllers;
using PhonebookClient.Infrastructure;
using PhonebookClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookClientTests.Controllers
{
    public class AuthControllerTests
    {
        // Profile
        [Fact]
        public void Profile_ReturnsView_WhenUserDetailsAreFound()
        {
            // Arrange
            var userDetails = new UserDetailsViewModel()
            {
                LoginId = "test",
                FirstName = "Test",
                LastName = "Test",
                Email = "test@example.com",
                ContactNumber = "1234567890",
            };

            var response = new ServiceResponse<UserDetailsViewModel>()
            {
                Data = userDetails,
                Success = true,
                Message = "Success",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(response);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Profile() as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            Assert.Equal(userDetails, actual.Model);
            mockHttpClientService.Verify(o => o.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Profile_ReturnsEmptyList_WhenUserDetailsAreNotFound()
        {
            // Arrange
            UserDetailsViewModel userDetails = new UserDetailsViewModel();

            var response = new ServiceResponse<UserDetailsViewModel>()
            {
                Data = userDetails,
                Success = false,
                Message = "Cannot retrieve user details",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(response);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Profile() as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            mockHttpClientService.Verify(o => o.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }

        // UpdateUserDetailsGet
        [Fact]
        public void UpdateUserDetailsGet_SetsErrorMessage_WhenErrorMessageIsNull()
        {
            // Arrange
            var errorMessage = "Something went wrong please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdateUserDetails() as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdateUserDetailsGet_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var errorMessage = "Retrieval of user details failed";
            ServiceResponse<UserDetailsViewModel> expectedServiceResponse = new ServiceResponse<UserDetailsViewModel>()
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdateUserDetails() as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdateUserDetailsGet_SetsErrorMessage_WhenServiceResponseSuccessIsFalse()
        {
            // Arrange
            var errorMessage = "Retrieval of user details failed";
            ServiceResponse<UserDetailsViewModel> expectedServiceResponse = new ServiceResponse<UserDetailsViewModel>()
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdateUserDetails() as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdateUserDetailsGet_ReturnsView_WhenUserDetailsRetrievedSuccessfully()
        {
            // Arrange
            var viewModel = new UserDetailsViewModel()
            {
                userId = 1,
                FirstName = "Test",
                LastName = "Test",
                LoginId = "test",
                Email = "test@example.com",
                ContactNumber = "1234567890",
            };
            var errorMessage = "User details updated successfully";
            ServiceResponse<UserDetailsViewModel> expectedServiceResponse = new ServiceResponse<UserDetailsViewModel>()
            {
                Data = viewModel,
                Success = true,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdateUserDetails() as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserDetailsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        // UpdateUserDetailsPost
        [Fact]
        public void UpdateUserDetailsPost_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new UpdateUserDetailsViewModel()
            {
                FirstName = "Test",
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            target.ModelState.AddModelError("ContactNumber", "Contact number is required.");

            // Act
            var actual = target.UpdateUserDetails(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void UpdateUserDetailsPost_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var viewModel = new UpdateUserDetailsViewModel()
            {
                FirstName = "Test",
            };

            var errorMessage = "Modification failed";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdateUserDetails(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdateUserDetailsPost_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var viewModel = new UpdateUserDetailsViewModel()
            {
                FirstName = "Test",
            };

            var errorMessage = "Something went wrong please try after some time.";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdateUserDetails(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdateUserDetailsPost_RedirectToActionIndex_WhenSuccessStatusCode()
        {
            // Arrange
            var viewModel = new UpdateUserDetailsViewModel()
            {
                FirstName = "Test",
            };

            var successMessage = "User details updated successfully";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Data = "fakeToken",
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdateUserDetails(viewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Profile", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        // UpdatePasswordGet
        [Fact]
        public void UpdatePassword_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);

            // Act
            var actual = target.UpdatePassword();

            // Assert
            Assert.NotNull(actual);
        }

        // UpdatePasswordPost
        [Fact]
        public void UpdatePassword_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new UpdatePasswordViewModel()
            {
                OldPassword = "Password@123",
                NewPassword = "NewPassword@123",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            target.ModelState.AddModelError("ConfirmNewPassword", "Confirm new password is required.");

            // Act
            var actual = target.UpdatePassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void UpdatePassword_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var viewModel = new UpdatePasswordViewModel()
            {
                OldPassword = "Password@123",
                NewPassword = "NewPassword@123",
                ConfirmNewPassword = "NewPassword@123",
            };

            var errorMessage = "Modification failed";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdatePassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdatePassword_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var viewModel = new UpdatePasswordViewModel()
            {
                OldPassword = "Password@123",
                NewPassword = "NewPassword@123",
                ConfirmNewPassword = "NewPassword@123",
            };

            var errorMessage = "Something went wrong please try after some time.";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdatePassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdatePassword_RedirectToActionIndex_WhenSuccessStatusCode()
        {
            // Arrange
            var viewModel = new UpdatePasswordViewModel()
            {
                OldPassword = "Password@123",
                NewPassword = "NewPassword@123",
                ConfirmNewPassword = "NewPassword@123",
            };

            var successMessage = "Password updated successfully";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Data = "fakeToken",
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.UpdatePassword(viewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Profile", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        // RegisterGet
        [Fact]
        public void RegisterGet_ReturnsView()
        {
            // Arrange
            var questions = new List<SecurityQuestionViewModel>
            {
                new SecurityQuestionViewModel()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                new SecurityQuestionViewModel()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var questionResponse = new ServiceResponse<IEnumerable<SecurityQuestionViewModel>>()
            {
                Data = questions,
                Success = true,
                Message = "",
            };

            var httpContext = new DefaultHttpContext();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<SecurityQuestionViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(questionResponse);
            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.RegisterUser() as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
        }

        // RegisterPost
        [Fact]
        public void RegisterPost_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var questions = new List<SecurityQuestionViewModel>
            {
                new SecurityQuestionViewModel()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                new SecurityQuestionViewModel()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var questionResponse = new ServiceResponse<IEnumerable<SecurityQuestionViewModel>>()
            {
                Data = questions,
                Success = true,
                Message = "",
            };
            var viewModel = new RegisterViewModel()
            {
                LoginId = "temp",
            };

            var httpContext = new DefaultHttpContext();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<SecurityQuestionViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(questionResponse);
            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            target.ModelState.AddModelError("Password", "Password is required");

            // Act
            var actual = target.RegisterUser(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
        }

        [Fact]
        public void RegisterPost_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var questions = new List<SecurityQuestionViewModel>
            {
                new SecurityQuestionViewModel()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                new SecurityQuestionViewModel()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var questionResponse = new ServiceResponse<IEnumerable<SecurityQuestionViewModel>>()
            {
                Data = questions,
                Success = true,
                Message = "",
            };
            var viewModel = new RegisterViewModel
            {
                LoginId = "temp",
                Password = "pass",
            };

            var httpContext = new DefaultHttpContext();

            var errorMessage = "Registration Failed";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<SecurityQuestionViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(questionResponse);
            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.RegisterUser(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void RegisterPost_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var questions = new List<SecurityQuestionViewModel>
            {
                new SecurityQuestionViewModel()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                new SecurityQuestionViewModel()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                },
            };

            var questionResponse = new ServiceResponse<IEnumerable<SecurityQuestionViewModel>>()
            {
                Data = questions,
                Success = true,
                Message = "",
            };
            var viewModel = new RegisterViewModel
            {
                LoginId = "temp",
                Password = "pass",
            };

            var httpContext = new DefaultHttpContext();

            var errorMessage = "Something went wrong please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<SecurityQuestionViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(questionResponse);
            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.RegisterUser(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void RegisterPost_ReturnsRedirectToAction_WhenRegisteredSuccessful()
        {
            // Arrange
            var viewModel = new RegisterViewModel
            {
                LoginId = "temp",
                Password = "pass",
            };
            var successMessage = "Category saved successfully";

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.RegisterUser(viewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("RegisterSuccess", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        // RegisterSuccess
        [Fact]
        public void RegisterSuccess_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);

            // Act
            var actual = target.RegisterSuccess() as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
        }

        // LoginGet
        [Fact]
        public void LoginGet_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);

            // Act
            var actual = target.LoginUser() as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
        }

        // LoginPost
        [Fact]
        public void LoginPost_SetsErrorMessage_WhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new LoginViewModel()
            {
                Username = "temp",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);

            target.ModelState.AddModelError("Password", "Password is required");

            // Act
            var actual = target.LoginUser(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
        }

        [Fact]
        public void LoginPost_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var viewModel = new LoginViewModel
            {
                Username = "temp",
                Password = "pass",
            };
            var errorMessage = "Registration Failed";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.LoginUser(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void LoginPost_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var viewModel = new LoginViewModel
            {
                Username = "temp",
                Password = "pass",
            };
            var errorMessage = "Something went wrong, please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.LoginUser(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void LoginPost_RedirectsToAction_WhenLoginSuccessful()
        {

            {
                // Arrange
                var viewModel = new LoginViewModel
                {
                    Username = "temp",
                    Password = "pass",
                };
                var successMessage = "Login successful";

                var mockHttpClientService = new Mock<IHttpClientService>();
                var mockConfiguration = new Mock<IConfiguration>();
                mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

                var expectedServiceResponse = new ServiceResponse<string>()
                {
                    Data = "fakeToken",
                    Message = successMessage,
                };
                var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
                };

                var mockHttpContext = new DefaultHttpContext();
                mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
                var mockTempDataProvider = new Mock<ITempDataProvider>();
                var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
                var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
                {
                    TempData = tempData,
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext,
                    }
                };

                // Act
                var actual = target.LoginUser(viewModel) as RedirectToActionResult;

                // Assert
                Assert.NotNull(actual);
                Assert.True(target.ModelState.IsValid);
                Assert.Equal("Phonebook", actual.ControllerName);
                Assert.Equal("Index", actual.ActionName);
                mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
                mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            }
        }

        // Logout
        [Fact]
        public void Logout_RedirectsToAction_WhenLogoutSuccessful()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var actual = target.Logout() as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
        }

        // ForgotPassword
        [Fact]
        public void ForgotPassword_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);

            // Act
            var actual = target.ForgotPassword() as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
        }

        // ResetPasswordGet
        [Fact]
        public void ResetPasswordGet_ReturnsRedirectToAction_WhenLoginIdIsNull()
        {
            // Arrange
            string loginId = null;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);

            // Act
            var actual = target.ResetPassword(loginId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ForgotPassword", actual.ActionName);
        }

        [Fact]
        public void ResetPasswordGet_SetsErrorMessage_WhenErrorMessageIsNull()
        {
            // Arrange
            var loginId = "test";
            var errorMessage = "Something went wrong please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.ResetPassword(loginId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void ResetPasswordGet_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var loginId = "test";
            var errorMessage = "Retrieval of user questions failed";
            ServiceResponse<UserSecurityQuestionsViewModel> expectedServiceResponse = new ServiceResponse<UserSecurityQuestionsViewModel>()
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.ResetPassword(loginId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void ResetPasswordGet_SetsErrorMessage_WhenServiceResponseSuccessIsFalse()
        {
            // Arrange
            var loginId = "test";
            var errorMessage = "Retrieval of user questions failed";
            ServiceResponse<UserSecurityQuestionsViewModel> expectedServiceResponse = new ServiceResponse<UserSecurityQuestionsViewModel>()
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.ResetPassword(loginId) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void ResetPasswordGet_ReturnsView_WhenUserDetailsRetrievedSuccessfully()
        {
            // Arrange
            var loginId = "test";
            var viewModel = new UserSecurityQuestionsViewModel()
            {
                LoginId = "test",
                SecurityQuestion_1 = new SecurityQuestionViewModel()
                {
                    QuestionId = 1,
                    Question = "Question 1",
                },
                SecurityQuestion_2 = new SecurityQuestionViewModel()
                {
                    QuestionId = 2,
                    Question = "Question 2",
                }
            };
            var errorMessage = "Password updated successfully";
            ServiceResponse<UserSecurityQuestionsViewModel> expectedServiceResponse = new ServiceResponse<UserSecurityQuestionsViewModel>()
            {
                Data = viewModel,
                Success = true,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.ResetPassword(loginId) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UserSecurityQuestionsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        // ResetPasswordPost
        [Fact]
        public void ResetPasswordPost_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel()
            {
                LoginId = "test",
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            target.ModelState.AddModelError("NewPassword", "New password is required.");

            // Act
            var actual = target.ResetPassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void ResetPasswordPost_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel()
            {
                LoginId = "test",
            };

            var errorMessage = "Reset password failed";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.ResetPassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void ResetPasswordPost_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel()
            {
                LoginId = "test",
            };

            var errorMessage = "Something went wrong please try after some time.";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.ResetPassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void ResetPasswordPost_RedirectToActionIndex_WhenSuccessStatusCode()
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel()
            {
                LoginId = "test",
            };

            var successMessage = "User details updated successfully";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Data = "fakeToken",
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.ResetPassword(viewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("LoginUser", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
    }
}
