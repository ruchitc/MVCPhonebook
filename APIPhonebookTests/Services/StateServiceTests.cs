using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;
using APIPhonebook.Models;
using APIPhonebook.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIPhonebookTests.Services
{
    public class StateServiceTests
    {
        // GetStatesByCountryId()
        [Fact]
        public void GetStatesByCountryId_ReturnsStates_WhenStatesExist()
        {
            // Arrange
            int countryId = 1;
            var states = new List<State>()
            {
                new State()
                {
                    StateId = 1,
                    StateName = "State 1",
                    CountryId = 1,
                },
                new State()
                {
                    StateId = 2,
                    StateName = "State 2",
                    CountryId = 1,
                },
            };

            var expectedResponse = new ServiceResponse<IEnumerable<StateDto>>()
            {
                Success = true,
                Message = "Success",
            };

            var mockStateRepository = new Mock<IStateRepository>();

            mockStateRepository.Setup(c => c.GetStatesByCountryId(countryId)).Returns(states);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetStatesByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockStateRepository.Verify(c => c.GetStatesByCountryId(countryId), Times.Once);
        }

        [Fact]
        public void GetStatesByCountryId_ReturnsFalse_WhenStatesAreNull()
        {
            // Arrange
            int countryId = 1;
            List<State> states = null;

            var expectedResponse = new ServiceResponse<IEnumerable<StateDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockStateRepository = new Mock<IStateRepository>();

            mockStateRepository.Setup(c => c.GetStatesByCountryId(countryId)).Returns(states);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetStatesByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockStateRepository.Verify(c => c.GetStatesByCountryId(countryId), Times.Once);
        }

        [Fact]
        public void GetStatesByCountryId_ReturnsFalse_WhenNoStatesExist()
        {
            // Arrange
            int countryId = 1;
            List<State> states = new List<State>();

            var expectedResponse = new ServiceResponse<IEnumerable<StateDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockStateRepository = new Mock<IStateRepository>();

            mockStateRepository.Setup(c => c.GetStatesByCountryId(countryId)).Returns(states);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetStatesByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockStateRepository.Verify(c => c.GetStatesByCountryId(countryId), Times.Once);
        }

        // GetStateById()
        [Fact]
        public void GetStateById_ReturnsState_WhenStateIsFound()
        {
            // Arrange
            int stateId = 1;
            var state = new State()
            {
                StateId = stateId,
                StateName = "State 1",
                CountryId = 1,
            };
            
            var expectedResponse = new ServiceResponse<StateDto>()
            {
                Success = true,
                Message = "Success",
            };

            var mockStateRepository = new Mock<IStateRepository>();

            mockStateRepository.Setup(c => c.GetStateById(stateId)).Returns(state);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetStateById(stateId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockStateRepository.Verify(c => c.GetStateById(stateId), Times.Once);
        }

        [Fact]
        public void GetStateById_ReturnsFalse_WhenStateIsNotFound()
        {
            // Arrange
            int stateId = 1;
            State state = null;

            var expectedResponse = new ServiceResponse<StateDto>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockStateRepository = new Mock<IStateRepository>();

            mockStateRepository.Setup(c => c.GetStateById(stateId)).Returns(state);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetStateById(stateId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockStateRepository.Verify(c => c.GetStateById(stateId), Times.Once);
        }
    }
}
