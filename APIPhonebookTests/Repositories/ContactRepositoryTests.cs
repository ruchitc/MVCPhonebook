using APIPhonebook.Data.Implementation;
using APIPhonebook.Models;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace APIPhonebookTests.Repositories
{
    public class ContactRepositoryTests
    {
        Fixture fixture;

        public ContactRepositoryTests()
        {
            fixture = new Fixture();    
        }

        // GetAllContacts()
        [Fact]
        public void GetAllContacts_ReturnsAllContacts_WhenShowFavouritesIsFalse()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsFavouriteContacts_WhenShowFavouritesIsTrue()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "";
            string sort_dir = "default";
            bool show_favourites = true;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            foreach(var contact in actual)
            {
                Assert.True(contact.IsFavourite);
            }
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsAllContacts_WhenSearchStringIsNull()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = null;
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenSearchStringIsGiven()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "Test";
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenSearchStringStartsWithAsterisk()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "*st";
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenSearchStringStartsAndEndsWithAsterisk()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "*e*";
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenSortDirIsNull()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "";
            string sort_dir = "";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenSortDirIsDesc()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "";
            string sort_dir = "desc";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenSortDirIsAsc()
        {
            // Arrange
            int page = 1;
            int page_size = 5;
            string? search_string = "";
            string sort_dir = "asc";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        // GetContactById()
        [Fact]
        public void GetContactById_ReturnsContact_WhenContactIsFound()
        {
            // Arrange
            int contactId = 1;

            var contact = new Contact()
            {
                ContactId = 1,
                FirstName = "Test 1",
                LastName = "Test 1",
                ContactNumber = "1234567890",
                IsFavourite = true,
            };

            var contacts = new List<Contact>()
            {
                contact,
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.GetEnumerator()).Returns(contacts.GetEnumerator());
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetContactById(contactId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contact, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.GetEnumerator(), Times.Once());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsNull_WhenContactNotFound()
        {
            // Arrange
            int contactId = 3;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.GetEnumerator()).Returns(contacts.GetEnumerator());
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetContactById(contactId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.GetEnumerator(), Times.Once());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        // TotalContacts()
        [Fact]
        public void TotalContacts_ReturnsCount_WhenShowFavoouritesIsFalse()
        {
            // Arrange
            string? search_string = "";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(search_string, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenShowFavouritesIsTrue()
        {
            // Arrange
            string? search_string = "";
            bool show_favourites = true;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(search_string, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenSearchStringisNull()
        {
            // Arrange
            string? search_string = null;
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(search_string, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenSearchStringIsGiven()
        {
            // Arrange
            string? search_string = "test";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(search_string, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenSearchStringStartsWithAsterisk()
        {
            // Arrange
            string? search_string = "*st 1";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(search_string, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenSearchStringStartsAndEndsWithAsterisk()
        {
            // Arrange
            string? search_string = "*st*";
            bool show_favourites = false;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test 1",
                    LastName = "Test 1",
                    ContactNumber = "1234567890",
                    IsFavourite = true,
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test 2",
                    LastName = "Test 2",
                    ContactNumber = "1234567891",
                    IsFavourite = false,
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(search_string, show_favourites);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        // AddContact()
        [Fact]
        public void AddContact_ReturnsTrue_WhenContactsIsAddedSuccessfully()
        {
            // Arrange
            var contact = new Contact
            {
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
            };

            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.AddContact(contact);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsFalse_WhenInsertionFails()
        {
            // Arrange
            Contact contact = null;

            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.AddContact(contact);

            // Assert
            Assert.False(actual);
        }

        // UpdateContact()
        [Fact]
        public void UpdateContact_ReturnsTrue_WhenContactIsUpdatedSuccessfully()
        {
            // Arrange
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890",
            };

            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateContact(contact);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateContact_ReturnsFalse_WhenContactModificationFails()
        {
            // Arrange
            Contact contact = null;

            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateContact(contact);

            // Assert
            Assert.False(actual);
        }

        // DeleteContact()
        [Fact]
        public void DeleteContact_ReturnsTrue_WhenContactIsDeletedSuccessfully()
        {
            // Arrange
            int contactId = 1;
            var contact = new Contact()
            {
                ContactId = contactId,
                FirstName = "Test",
                LastName = "Test",
                ContactNumber = "1234567890"
            };

            var contacts = new List<Contact> { contact }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.DeleteContact(contactId);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.Verify(c => c.Remove(contact), Times.Once);
            mockAppDbContext.VerifyGet(c => c.Contacts, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsFalse_WhenContactDeletionFails()
        {
            // Arrange
            int contactId = 1;

            var contacts = new List<Contact>().AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.DeleteContact(contactId);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Contacts, Times.Once);
        }

        // ContactExists()
        [Fact]
        public void ContactExists_ReturnsTrue_WhenContactExists()
        {
            // Arrange
            var contactNumber = "1234567890";
            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567891"
                },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactExists(contactNumber);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExists_ReturnsFalse_WhenContactDoesntExist()
        {
            // Arrange
            var contactNumber = "1234567892";
            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567891"
                },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactExists(contactNumber);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Contacts, Times.Once);
        }

        // ContactExists() with ContactId
        [Fact]
        public void ContactExists_WithContactId_ReturnsTrue_WhenContactExists()
        {
            // Arrange
            int contactId = 2;
            var contactNumber = "1234567890";
            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567891"
                },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactExists(contactId, contactNumber);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExists_WithContactId_ReturnsFalse_WhenContactDoesntExist()
        {
            // Arrange
            int contactId = 1;
            var contactNumber = "1234567890";
            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
                new Contact()
                {
                    ContactId = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567891"
                },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactExists(contactId, contactNumber);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Contacts, Times.Once);
        }
    }
}
