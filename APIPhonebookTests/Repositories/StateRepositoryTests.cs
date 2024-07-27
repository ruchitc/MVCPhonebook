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
    public class StateRepositoryTests
    {
        // GetStatesByCountryId()
        [Fact]
        public void GetStatesByCountryId_ReturnsStates_WhenStatesExist()
        {
            // Arrange
            int countryId = 1;
            var states = new List<State>()
            {
                new State { StateId = 1, StateName = "State 1", CountryId = 1 },
                new State { StateId = 2, StateName = "State 2", CountryId = 1 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Expression).Returns(states.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(s => s.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetStatesByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(states, actual);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Expression, Times.Once);
            mockAppDbContext.Verify(s => s.States, Times.Once);
        }

        [Fact]
        public void GetStatesByCountryId_ReturnsNull_WhenNoStatesExist()
        {
            // Arrange
            int countryId = 2;
            var states = new List<State>()
            {
                new State { StateId = 1, StateName = "State 1", CountryId = 1 },
                new State { StateId = 2, StateName = "State 2", CountryId = 1 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Expression).Returns(states.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(s => s.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetStatesByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Expression, Times.Once);
            mockAppDbContext.Verify(s => s.States, Times.Once);
        }

        // GetStateById()
        [Fact]
        public void GetStateById_ReturnsState_WhenStateFound()
        {
            // Arrange
            int stateId = 1;
            var state = new State()
            {
                StateId = 1,
                StateName = "State 1",
                CountryId = 1,
            };

            var states = new List<State>()
            {
                state,
                new State { StateId = 2, StateName = "State 2", CountryId = 1 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Expression).Returns(states.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(s => s.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetStateById(stateId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(state, actual);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Expression, Times.Once);
            mockAppDbContext.Verify(s => s.States, Times.Once);
        }

        [Fact]
        public void GetStateById_ReturnsNull_WhenStateNotFound()
        {
            // Arrange
            int stateId = 3;

            var states = new List<State>()
            {
                new State { StateId = 1, StateName = "State 1", CountryId = 1 },
                new State { StateId = 2, StateName = "State 2", CountryId = 1 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(s => s.Expression).Returns(states.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(s => s.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetStateById(stateId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(s => s.Expression, Times.Once);
            mockAppDbContext.Verify(s => s.States, Times.Once);
        }
    }
}
