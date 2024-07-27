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
    public class ContactControllerTests
    {
        // GetAllContacts()

        [Fact]
        public void GetAllContacts_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<ContactDto>()
            {
                new ContactDto()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
                new ContactDto()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
            };

            var response = new PaginationServiceResponse<IEnumerable<ContactDto>>()
            {
                Data = contacts,
                Total = 10,
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            var response = new PaginationServiceResponse<IEnumerable<ContactDto>>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites), Times.Once);
        }

        // GetContactById()

        [Fact]
        public void GetContactById_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int contactId = 1;
            var contact = new ContactDto()
            {
                ContactId = contactId,
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
            };
                
            var response = new ServiceResponse<ContactDto>()
            {
                Data = contact,
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.GetContactById(contactId)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetContactById(contactId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContactById(contactId), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int contactId = 1;

            var response = new ServiceResponse<ContactDto>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.GetContactById(contactId)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetContactById(contactId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContactById(contactId), Times.Once);
        }

        // GetTotalContacts()

        [Fact]
        public void GetTotalContacts_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            string search_string = "";
            bool show_favourites = false;

            var response = new ServiceResponse<int>()
            {
                Data = 10,
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.TotalContacts(search_string, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetTotalContacts(search_string, show_favourites) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.TotalContacts(search_string, show_favourites), Times.Once);
        }

        [Fact]
        public void GetTotalContacts_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            string search_string = "";
            bool show_favourites = false;

            var response = new ServiceResponse<int>()
            {
                Data = 0,
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.TotalContacts(search_string, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetTotalContacts(search_string, show_favourites) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.TotalContacts(search_string, show_favourites), Times.Once);
        }

        // AddContact()

        [Fact]
        public void AddContact_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.AddContact(contactDto)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.AddContact(contactDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(contactDto), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.AddContact(contactDto)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.AddContact(contactDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(contactDto), Times.Once);
        }

        // UpdateContact()

        [Fact]
        public void UpdateContact_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<UpdateContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.UpdateContact(contactDto)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.UpdateContact(contactDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.UpdateContact(contactDto), Times.Once);
        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<UpdateContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.UpdateContact(contactDto)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.UpdateContact(contactDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.UpdateContact(contactDto), Times.Once);
        }

        // DeleteContact()

        [Fact]
        public void DeleteContact_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int contactId = 1;

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.DeleteContact(contactId)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.DeleteContact(contactId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.DeleteContact(contactId), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            int contactId = 1;

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.DeleteContact(contactId)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.DeleteContact(contactId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.DeleteContact(contactId), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsBadRequest_WhenIdIsLessThanOne()
        {
            // Arrange
            int contactId = 0;

            var mockContactService = new Mock<IContactService>();

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.DeleteContact(contactId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
        }
    }
}
