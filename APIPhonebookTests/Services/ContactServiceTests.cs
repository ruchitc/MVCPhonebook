using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;
using APIPhonebook.Models;
using APIPhonebook.Services.Contract;
using APIPhonebook.Services.Implementation;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;

namespace APIPhonebookTests.Services
{
    public class ContactServiceTests
    {
        // GetAllContacts()

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenContactsExist()

        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                    Country = new Country()
                    {
                        CountryId = 1,
                        CountryName = "Country 1",
                    },
                    State = new State()
                    {
                        StateId = 1,
                        StateName = "State 1",
                        CountryId = 1,
                    },
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567891",
                    Country = new Country()
                    {
                        CountryId = 1,
                        CountryName = "Country 1",
                    },
                    State = new State()
                    {
                        StateId = 1,
                        StateName = "State 1",
                        CountryId = 1,
                    },
                },
            };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactDto>>()
            {
                Success = true,
                Message = "Success",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites)).Returns(contacts);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsFalse_WhenContactsAreNull()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            List<Contact> contacts = null;

            var expectedResponse = new ServiceResponse<IEnumerable<ContactDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites)).Returns(contacts);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsFalse_WhenNoContactsExist()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            List<Contact> contacts = new List<Contact>();

            var expectedResponse = new ServiceResponse<IEnumerable<ContactDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites)).Returns(contacts);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites), Times.Once);
        }

        // GetContactById()

        [Fact]
        public void GetContactsById_ReturnsContact_WhenContactIsFound()

        {
            // Arrange
            int contactId = 1;

            var contact = new Contact()
            {
                ContactId = 1,
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
                Country = new Country()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                State = new State()
                {
                    StateId = 1,
                    StateName = "State 1",
                    CountryId = 1,
                },
            };

            var expectedResponse = new ServiceResponse<ContactDto>()
            {
                Success = true,
                Message = "Success",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.GetContactById(contactId)).Returns(contact);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.GetContactById(contactId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.GetContactById(contactId), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsFalse_WhenContactIsNotFound()
        {
            // Arrange
            int contactId = 1;

            Contact contact = null;

            var expectedResponse = new ServiceResponse<ContactDto>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.GetContactById(contactId)).Returns(contact);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.GetContactById(contactId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.GetContactById(contactId), Times.Once);
        }

        // TotalContacts()
        [Fact]
        public void TotalContacts_ReturnsCount()
        {
            // Arrange
            string search_string = "";
            bool show_favourites = false;

            int count = 10;

            var expectedResponse = new ServiceResponse<int>()
            {
                Data = count,
                Success = true,
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.TotalContacts(search_string, show_favourites)).Returns(count);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.TotalContacts(search_string, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.TotalContacts(search_string, show_favourites), Times.Once);
        }

        // AddContact()

        [Fact]
        public void AddContact_ReturnsFalse_WhenContactAlreadyExists()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Contact number already exists.",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contactDto.ContactNumber)).Returns(true);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.AddContact(contactDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contactDto.ContactNumber), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsTrue_WhenContactWithoutImageIsAddedSuccessfully()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Contact saved successfully",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contactDto.ContactNumber)).Returns(false);

            mockContactRepository.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(true);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.AddContact(contactDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contactDto.ContactNumber), Times.Once);
            mockContactRepository.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsTrue_WhenContactWithImageIsAddedSuccessfully()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();
            contactDto.Image = new Mock<IFormFile>().Object;

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Contact saved successfully",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contactDto.ContactNumber)).Returns(false);
            mockFileService.Setup(c => c.ToByteArray(contactDto.Image)).Returns(new byte[0]);

            mockContactRepository.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(true);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.AddContact(contactDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contactDto.ContactNumber), Times.Once);
            mockContactRepository.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsFalse_WhenContactInsertionFails()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, Please try after some time",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contactDto.ContactNumber)).Returns(false);

            mockContactRepository.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(false);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.AddContact(contactDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contactDto.ContactNumber), Times.Once);
            mockContactRepository.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);
        }

        // UpdateContact()

        [Fact]
        public void UpdateContact_ReturnsFalse_WhenContactAlreadyExists()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<UpdateContactDto>().Without(c => c.Image).Create();

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Contact number already exists.",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contactDto.ContactId, contactDto.ContactNumber)).Returns(true);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.UpdateContact(contactDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contactDto.ContactId, contactDto.ContactNumber), Times.Once);
        }

        [Fact]
        public void UpdateContact_ReturnsTrue_WhenContactWithImageIsUpdatedSuccessfully()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<UpdateContactDto>().Without(c => c.Image).Create();
            contactDto.Image = new Mock<IFormFile>().Object;

            Contact contact = new Contact()
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                ContactNumber = contactDto.ContactNumber,
                Gender = contactDto.Gender,
                CountryId = contactDto.CountryId,
                StateId = contactDto.StateId,
                IsFavourite = contactDto.IsFavourite,
                Email = contactDto.Email,
                Company = contactDto.Company,
            };

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Contact updated successfully",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contactDto.ContactId, contactDto.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContactById(contactDto.ContactId)).Returns(contact);
            mockContactRepository.Setup(c => c.UpdateContact(contact)).Returns(true);

            mockFileService.Setup(c => c.ToByteArray(contactDto.Image)).Returns(new byte[0]);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.UpdateContact(contactDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contactDto.ContactId, contactDto.ContactNumber), Times.Once);
            mockContactRepository.Verify(c => c.GetContactById(contactDto.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(contact), Times.Once);
            mockFileService.Verify(c => c.ToByteArray(contactDto.Image), Times.Once);
        }

        [Fact]
        public void UpdateContact_ReturnsFalse_WhenContactModificationFails()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<UpdateContactDto>().Without(c => c.Image).Create();
            contactDto.Image = new Mock<IFormFile>().Object;

            Contact contact = new Contact()
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                ContactNumber = contactDto.ContactNumber,
                Gender = contactDto.Gender,
                CountryId = contactDto.CountryId,
                StateId = contactDto.StateId,
                IsFavourite = contactDto.IsFavourite,
                Email = contactDto.Email,
                Company = contactDto.Company,
            };

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, Please try after some time",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contactDto.ContactId, contactDto.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContactById(contactDto.ContactId)).Returns(contact);
            mockContactRepository.Setup(c => c.UpdateContact(contact)).Returns(false);

            mockFileService.Setup(c => c.ToByteArray(contactDto.Image)).Returns(new byte[0]);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.UpdateContact(contactDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contactDto.ContactId, contactDto.ContactNumber), Times.Once);
            mockContactRepository.Verify(c => c.GetContactById(contactDto.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(contact), Times.Once);
            mockFileService.Verify(c => c.ToByteArray(contactDto.Image), Times.Once);
        }

        // DeleteContact()

        [Fact]
        public void DeleteContact_ReturnsTrue_WhenContactDeletedSuccessfully()
        {
            // Arrange
            int contactId = 1;

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = true,
                Message = "Contact deleted successfully.",
            };

            mockContactRepository.Setup(c => c.DeleteContact(contactId)).Returns(true);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.DeleteContact(contactId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.DeleteContact(contactId), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsFalse_WhenContactDeletionFails()
        {
            // Arrange
            int contactId = 1;

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            var expectedResponse = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Something went wrong, please try after some time.",
            };

            mockContactRepository.Setup(c => c.DeleteContact(contactId)).Returns(false);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.DeleteContact(contactId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockContactRepository.Verify(c => c.DeleteContact(contactId), Times.Once);
        }
    }
}
