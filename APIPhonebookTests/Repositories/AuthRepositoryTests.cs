using APIPhonebook.Data.Implementation;
using APIPhonebook.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIPhonebookTests.Repositories
{
    public class AuthRepositoryTests
    {
        // GetSecurityQuestions()
        [Fact]
        public void GetSecurityQuestions_ReturnsQuestions_WhenQuestionsExist()
        {
            // Arrange
            var questions = new List<SecurityQuestion>()
            {
                new SecurityQuestion {QuestionId = 1, Question = "Question 1"},
                new SecurityQuestion {QuestionId = 2, Question = "Question 2"},
            };

            var mockDbSet = new Mock<DbSet<SecurityQuestion>>();
            mockDbSet.As<IQueryable<SecurityQuestion>>().Setup(c => c.GetEnumerator()).Returns(questions.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.SecurityQuestions).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetSecurityQuestions();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(questions.Count(), actual.Count());
            mockDbSet.As<IQueryable<SecurityQuestion>>().Verify(c => c.GetEnumerator(), Times.Once());
            mockAppDbContext.Verify(c => c.SecurityQuestions, Times.Once);
        }

        [Fact]
        public void GetSecurityQuestions_ReturnsNull_WhenNoQuestionsExist()
        {
            // Arrange
            var questions = new List<SecurityQuestion>().AsQueryable();

            var mockDbSet = new Mock<DbSet<SecurityQuestion>>();
            mockDbSet.As<IQueryable<SecurityQuestion>>().Setup(c => c.GetEnumerator()).Returns(questions.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.SecurityQuestions).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetSecurityQuestions();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(questions.Count(), actual.Count());
            mockDbSet.As<IQueryable<SecurityQuestion>>().Verify(c => c.GetEnumerator(), Times.Once());
            mockAppDbContext.Verify(c => c.SecurityQuestions, Times.Once);
        }

        // RegisterUser()
        [Fact]
        public void RegisterUser_ReturnsTrue()
        {
            // Arrange
            var user = new User()
            {
                UserId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "LoginId",
                Email = "test@example.com"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(o => o.SaveChanges()).Returns(1);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.RegisterUser(user);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(o => o.Add(user), Times.Once());
            mockAppDbContext.Verify(o => o.SaveChanges(), Times.Once);
        }

        [Fact]
        public void RegisterUser_ReturnsFalse()
        {
            // Arrange
            User user = null;

            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.RegisterUser(user);

            // Assert
            Assert.False(actual);
        }

        // GetUserById()
        [Fact]
        public void GetUserById_ReturnsUser_WhenUserIsFound()
        {
            // Arrange
            int userId = 1;
            User user = new User
            {
                UserId = 1,
                LoginId = "test",
            };

            var users = new List<User>() { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserById(userId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        [Fact]
        public void GetUserById_ReturnsNull_WhenNoUserFound()
        {
            // Arrange
            int userId = 2;
            User user = new User
            {
                UserId = 1,
                LoginId = "test",
            };

            var users = new List<User>() { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserById(userId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        // GetUserDetails()
        [Fact]
        public void GetDetails_ReturnsUser_WhenUserIsFound()
        {
            // Arrange
            var loginId = "test";
            User user = new User
            {
                UserId = 1,
                LoginId = "test",
                Email = "test@example.com"
            };

            var users = new List<User>() { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserDetails(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        [Fact]
        public void GetDetails_ReturnsNull_WhenNoUserFound()
        {
            // Arrange
            var loginId = "test2";
            User user = new User
            {
                UserId = 1,
                LoginId = "test",
                Email = "test@example.com"
            };

            var users = new List<User>() { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserDetails(loginId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        // GetUserSecurityQuestions
        [Fact]
        public void GetUserSecurityQuestions_ReturnsUser_WhenUserIsFound()
        {
            // Arrange
            var loginId = "test";
            User user = new User
            {
                UserId = 1,
                LoginId = "test",
                Email = "test@example.com"
            };

            var users = new List<User>() { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserSecurityQuestions(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        [Fact]
        public void GetUserSecurityQuestions_ReturnsNull_WhenNoUserFound()
        {
            // Arrange
            var loginId = "test2";
            User user = new User
            {
                UserId = 1,
                LoginId = "test",
                Email = "test@example.com"
            };

            var users = new List<User>() { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserSecurityQuestions(loginId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Users, Times.Once);
        }

        // UpdateUser()
        [Fact]
        public void UpdateUser_ReturhsTrue_WhenUserIsUpdatedSuccessfully()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                LoginId = "Test",
                Email = "test@example.com",
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUser(user);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(user), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateUser_ReturnsFalse_WhenModificationFails()
        {
            // Arrange
            User user = null;

            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUser(user);

            // Assert
            Assert.False(actual);
        }

        // ValidateUser()
        [Fact]
        public void ValidateUser_ReturnsUser_WhenLoginIdIsGiven()
        {
            // Arrange
            var loginId = "loginId";
            var user = new User()
            {
                LoginId = loginId,
            };
            var users = new List<User> { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ValidateUser(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void ValidateUser_ReturnsUser_WhenEmailIsGiven()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User()
            {
                LoginId = "login",
                Email = email,
            };
            var users = new List<User> { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ValidateUser(email);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void ValidateUser_ReturnsNull()
        {
            // Arrange
            var loginId = "loginId";
            var user = new User()
            {
                LoginId = "temp",
            };
            var users = new List<User> { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ValidateUser(loginId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        // UsernameExists()
        [Fact]
        public void UsernameExists_ReturnsTrue_WhenUsernameIsNotAvailable()
        {
            // Arrange
            var loginId = "test";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UsernameExists(loginId);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        public void UsernameExists_ReturnsFalse_WhenUsernameIsAvailable()
        {
            // Arrange
            var loginId = "test3";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UsernameExists(loginId);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        // UsernameExists() - For update
        [Fact]
        public void UsernameExists_ForUpdate_ReturnsTrue_WhenUsernameIsNotAvailable()
        {
            // Arrange
            int userId = 2;
            var loginId = "test";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UsernameExists(userId, loginId);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        public void UsernameExists_ForUpdate_ReturnsFalse_WhenUsernameIsAvailable()
        {
            // Arrange
            int userId = 1;
            var loginId = "test3";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UsernameExists(userId, loginId);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        // EmailExists()
        [Fact]
        public void EmailExists_ReturnsTrue_WhenEmailIsNotAvailable()
        {
            // Arrange
            var email = "test@example.com";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                    Email = "test@example.com",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                    Email = "test2@example.com",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.EmailExists(email);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        public void EmailExists_ReturnsFalse_WhenEmailIsAvailable()
        {
            // Arrange
            var email = "test3@example.com";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                    Email = "test@example.com",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                    Email = "test2@example.com",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.EmailExists(email);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        // ContactNumberExists()
        [Fact]
        public void ContactNumberExists_ReturnsTrue_WhenContactNumberIsNotAvailable()
        {
            // Arrange
            var contactNumber = "1234567890";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                    Email = "test@example.com",
                    ContactNumber = "1234567890",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                    Email = "test2@example.com",
                    ContactNumber = "1234567891",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactNumberExists(contactNumber);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        public void ContactNumberExists_ReturnsFalse_WhenContactNumberIsAvailable()
        {
            // Arrange
            var contactNumber = "1234567892";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                    Email = "test@example.com",
                    ContactNumber = "1234567890",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                    Email = "test2@example.com",
                    ContactNumber = "1234567891",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactNumberExists(contactNumber);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        // UsernameExists() - For update
        [Fact]
        public void ContactNumberExists_ForUpdate_ReturnsTrue_WhenContactNumberIsNotAvailable()
        {
            // Arrange
            int userId = 2;
            var contactNumber = "1234567890";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                    Email = "test@example.com",
                    ContactNumber = "1234567890",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                    Email = "test2@example.com",
                    ContactNumber = "1234567891",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactNumberExists(userId, contactNumber);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        [Fact]
        public void ContactNumberExists_ForUpdate_ReturnsTrue_WhenContactNumberIsAvailable()
        {
            // Arrange
            int userId = 1;
            var contactNumber = "1234567892";
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    LoginId = "test",
                    Email = "test@example.com",
                    ContactNumber = "1234567890",
                },
                new User()
                {
                    UserId = 2,
                    LoginId = "test2",
                    Email = "test2@example.com",
                    ContactNumber = "1234567891",
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ContactNumberExists(userId, contactNumber);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Users, Times.Once);
        }

        //[Fact]
        //public void UserExists_ReturnsTrue_WhenLoginIdIsGiven()
        //{
        //    // Arrange
        //    var loginId = "loginId";
        //    var email = "test@example.com";

        //    var user = new User()
        //    {
        //        LoginId = loginId,
        //        Email = "email@example.com",
        //    };
        //    var users = new List<User>()
        //    {
        //        user,
        //    }.AsQueryable();

        //    var mockDbSet = new Mock<DbSet<User>>();
        //    var mockAppDbContext = new Mock<IAppDbContext>();

        //    mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
        //    mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

        //    mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

        //    var target = new AuthRepository(mockAppDbContext.Object);

        //    // Act
        //    var actual = target.UserExists(loginId, email);

        //    // Assert
        //    Assert.True(actual);
        //    mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
        //    mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
        //    mockAppDbContext.Verify(o => o.Users, Times.Once);
        //}

        //[Fact]
        //public void UserExists_ReturnsTrue_WhenEmailIsGiven()
        //{
        //    // Arrange
        //    var loginId = "loginId";
        //    var email = "test@example.com";

        //    var user = new User()
        //    {
        //        LoginId = "login",
        //        Email = email,
        //    };
        //    var users = new List<User>()
        //    {
        //        user,
        //    }.AsQueryable();

        //    var mockDbSet = new Mock<DbSet<User>>();
        //    var mockAppDbContext = new Mock<IAppDbContext>();

        //    mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
        //    mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

        //    mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

        //    var target = new AuthRepository(mockAppDbContext.Object);

        //    // Act
        //    var actual = target.UserExists(loginId, email);

        //    // Assert
        //    Assert.True(actual);
        //    mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
        //    mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
        //    mockAppDbContext.Verify(o => o.Users, Times.Once);
        //}

        //[Fact]
        //public void UserExists_ReturnsFalse()
        //{
        //    // Arrange
        //    var loginId = "loginId";
        //    var email = "test@example.com";

        //    var user = new User()
        //    {
        //        LoginId = "temp",
        //        Email = "email@example.com",
        //    };
        //    var users = new List<User>()
        //    {
        //        user,
        //    }.AsQueryable();

        //    var mockDbSet = new Mock<DbSet<User>>();
        //    var mockAppDbContext = new Mock<IAppDbContext>();

        //    mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
        //    mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

        //    mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

        //    var target = new AuthRepository(mockAppDbContext.Object);

        //    // Act
        //    var actual = target.UserExists(loginId, email);

        //    // Assert
        //    Assert.False(actual);
        //    mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
        //    mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
        //    mockAppDbContext.Verify(o => o.Users, Times.Once);
        //}
    }
}
