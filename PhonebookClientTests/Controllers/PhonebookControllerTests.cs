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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookClientTests.Controllers
{
    public class PhonebookControllerTests
    {
        // Index
        [Fact]
        public void Index_ReturnsView_WhenContactsExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string searchString = null;
            string sortDir = "default";
            bool showFavourites = false;

            var contacts = new List<ContactViewModel>()
            {
                new ContactViewModel()
                {
                    ContactId = 1,
                    ContactNumber = "1234567890",
                    FirstName = "Test",
                },
                new ContactViewModel()
                {
                    ContactId = 2,
                    ContactNumber = "1234567892",
                    FirstName = "Test",
                },
            };

            var response = new PaginationServiceResponse<IEnumerable<ContactViewModel>>()
            {
                Data = contacts,
                Total = contacts.Count(),
                Success = true,
                Message = "Success",
            };
            
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<PaginationServiceResponse<IEnumerable<ContactViewModel>>>
                (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(response);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Index(page, pageSize, searchString, sortDir, showFavourites) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            Assert.Equal(contacts, actual.Model);
            mockHttpClientService.Verify(o => o.ExecuteApiRequest<PaginationServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Index_ReturnsEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string searchString = "";
            string sortDir = "default";
            bool showFavourites = false;

            var contacts = new List<ContactViewModel>();

            var response = new PaginationServiceResponse<IEnumerable<ContactViewModel>>()
            {
                Data = contacts,
                Total = contacts.Count(),
                Success = false,
                Message = "No record found",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<PaginationServiceResponse<IEnumerable<ContactViewModel>>>
                (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(response);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Index(page, pageSize, searchString, sortDir, showFavourites) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            Assert.Equal(contacts, actual.Model);
            mockHttpClientService.Verify(o => o.ExecuteApiRequest<PaginationServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }

        // Details
        [Fact]
        public void Details_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            int id = 1;
            var errorMessage = "Failed to retrieve contact";
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
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Details(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Details_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            int id = 1;
            var errorMessage = "Something went wrong please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Details(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Details_SetsErrorMessage_WhenServiceResponseSuccessIsFalse()
        {
            // Arrange
            int id = 1;
            var errorMessage = "Something went wrong please try after some time.";
            ServiceResponse<ContactViewModel> expectedServiceResponse = new ServiceResponse<ContactViewModel>()
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
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Details(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Details_SetsErrorMessage_WhenServiceResponseIsNull()
        {
            // Arrange
            int id = 1;
            var errorMessage = "Something went wrong please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Details(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Details_ReturnsView_WhenServiceResponseIsTrue()
        {
            // Arrange
            var contact = new ContactViewModel()
            {
                ContactId = 1,
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
                Email = "test@example.com",
            };

            int id = 1;
            var successMessage = "";
            ServiceResponse<ContactViewModel> expectedServiceResponse = new ServiceResponse<ContactViewModel>()
            {
                Data = contact,
                Success = true,
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Details(id) as ViewResult;

            var resultContact = actual.Model as ContactViewModel;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contact.ContactId, resultContact.ContactId);
            Assert.Equal(contact.FirstName, resultContact.FirstName);
            Assert.Equal(contact.LastName, resultContact.LastName);
            Assert.Equal(contact.ContactNumber, resultContact.ContactNumber);
            Assert.Equal(contact.Email, resultContact.Email);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        // CreateGet
        [Fact]
        public void CreateGet_ReturnsView()
        {
            // Arrange
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var response = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };

            var httpContext = new DefaultHttpContext();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(response);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Create() as ViewResult;

            // Assert
            Assert.NotNull(actual);
        }

        // CreatePost
        [Fact]
        public void CreatePost_ReturnsEmptyView_WhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new AddContactViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
            };

            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var httpContext = new DefaultHttpContext();

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            target.ModelState.AddModelError("Email", "Email is required.");

            // Act
            var actual = target.Create(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void CreatePost_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var viewModel = new AddContactViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
            };
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var errorMessage = "Creation Failed";
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
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.PostHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var actual = target.Create(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void CreatePost_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var viewModel = new AddContactViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
            };
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };
            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var errorMessage = "Something went wrong, please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);
            mockHttpClientService.Setup(c => c.PostHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Create(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void CreatePost_ReturnsRedirectToAction_WhenCategorySavedSuccessfully()
        {
            // Arrange
            var viewModel = new AddContactViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
            };
            var successMessage = "Contact saved successfully";

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:Phonebook"]).Returns("endPoint");

            var expectedServiceResponse = new ServiceResponse<string>()
            {
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var httpContext = new DefaultHttpContext();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        // EditGet
        [Fact]
        public void EditGet_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            int id = 1;
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var errorMessage = "Something went wrong please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void EditGet_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            int id = 1;
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var errorMessage = "Retrieval of contact by id failed";
            ServiceResponse<UpdateContactViewModel> expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>()
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
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void EditGet_SetsErrorMessage_WhenServiceResponseSuccessIsFalse()
        {
            // Arrange
            int id = 1;
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var errorMessage = "Retrieval of contact by id failed";
            ServiceResponse<UpdateContactViewModel> expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>()
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
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void EditGet_ReturnsView_WhenCategoryRetrievedSuccessfully()
        {
            // Arrange
            var viewModel = new UpdateContactViewModel()
            {
                ContactId = 1,
                FirstName = "Test",
                ContactNumber = "1234567890",
            };
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            int id = 1;
            var errorMessage = "Contact updated successfully";
            ServiceResponse<UpdateContactViewModel> expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>()
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
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        // EditPost
        [Fact]
        public void EditPost_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var viewModel = new UpdateContactViewModel()
            {
                FirstName = "Test",
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var httpContext = new DefaultHttpContext();
            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            target.ModelState.AddModelError("ContactNumber", "Contact number is required.");

            // Act
            var actual = target.Edit(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void EditPost_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var viewModel = new UpdateContactViewModel()
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
            mockHttpClientService.Setup(c => c.PutHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Edit(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void EditPost_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var countries = new List<CountryViewModel>()
            {
                new CountryViewModel()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryViewModel()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var countriesResponse = new ServiceResponse<IEnumerable<CountryViewModel>>()
            {
                Data = countries,
                Success = true,
            };
            var viewModel = new UpdateContactViewModel()
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
            mockHttpClientService.Setup(c => c.PutHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
               (It.IsAny<string>(), HttpMethod.Get, httpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(countriesResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Edit(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void EditPost_RedirectToActionIndex_WhenSuccessStatusCode()
        {
            // Arrange
            var viewModel = new UpdateContactViewModel()
            {
                FirstName = "Test",
            };

            var successMessage = "Contact updated successfully";
            var expectedServiceResponse = new ServiceResponse<string>()
            {
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
            mockHttpClientService.Setup(c => c.PutHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            // Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessageFormData(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        // DeleteGet
        [Fact]
        public void DeleteGet_SetsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            int id = 1;
            var errorMessage = "Something went wrong please try after some time.";

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Delete(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void DeleteGet_SetsErrorMessage_WhenErrorResponseIsNotNull()
        {
            // Arrange
            int id = 1;
            var errorMessage = "Retrieval of contact by id failed";
            ServiceResponse<ContactViewModel> expectedServiceResponse = new ServiceResponse<ContactViewModel>()
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
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Delete(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void DeleteGet_SetsErrorMessage_WhenServiceResponseSuccessIsFalse()
        {
            // Arrange
            int id = 1;
            var errorMessage = "Retrieval of contact by id failed";
            ServiceResponse<ContactViewModel> expectedServiceResponse = new ServiceResponse<ContactViewModel>()
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
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Delete(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void DeleteGet_ReturnsView_WhenCategoryRetrievedSuccessfully()
        {
            // Arrange
            var viewModel = new ContactViewModel()
            {
                ContactNumber = "1234567890",
                FirstName = "Test",
            };
            int id = 1;
            var successMessage = "Contact deleted successfully";
            ServiceResponse<ContactViewModel> expectedServiceResponse = new ServiceResponse<ContactViewModel>()
            {
                Data = viewModel,
                Success = true,
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse)) { }
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        // DeleteConfirmed
        [Fact]
        public void DeleteConfirmed_SetsErrorMessage_WhenResponseIsNull()
        {
            // Arrange
            int id = 1;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<string>>
                (It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), It.IsAny<object>(), It.IsAny<int>())).Returns<ServiceResponse<string>>(null);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var actual = target.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(o => o.ExecuteApiRequest<ServiceResponse<string>>
                (It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void DeleteConfirmed_SetsErrorMessage_WhenDeletionFails()
        {
            // Arrange
            int id = 1;

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<string>>
                (It.IsAny<string>(), HttpMethod.Delete, mockHttpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(response);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var actual = target.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(response.Message, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(o => o.ExecuteApiRequest<ServiceResponse<string>>
                (It.IsAny<string>(), HttpMethod.Delete, mockHttpContext.Request, It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void DeleteConfirmed_RedirectsToActionIndex_WhenDeletedSuccessfully()
        {
            // Arrange
            int id = 1;

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Contact deleted successfully",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockConfiguration.Setup(c => c["EndPoint:PhonebookApi"]).Returns("endPoint");
            mockHttpClientService.Setup(o => o.ExecuteApiRequest<ServiceResponse<string>>
                (It.IsAny<string>(), HttpMethod.Delete, mockHttpContext.Request, It.IsAny<object>(), It.IsAny<int>())).Returns(response);

            var target = new PhonebookController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var actual = target.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(response.Message, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:PhonebookApi"], Times.Once);
            mockHttpClientService.Verify(o => o.ExecuteApiRequest<ServiceResponse<string>>
                (It.IsAny<string>(), HttpMethod.Delete, mockHttpContext.Request, It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }
    }
}
