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
    public class CountryServiceTests
    {
        // GetAllCountries()
        [Fact]
        public void GetAllCountries_ReturnsCountries_WhenCountriesExist()
        {
            // Arrange
            var countries = new List<Country>()
            {
                new Country()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new Country()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var expectedResponse = new ServiceResponse<IEnumerable<CountryDto>>()
            {
                Success = true,
                Message = "Success",
            };

            var mockCountryRepository = new Mock<ICountryRepository>();

            mockCountryRepository.Setup(c => c.GetAllCountries()).Returns(countries);

            var target = new CountryService(mockCountryRepository.Object);

            // Act
            var actual = target.GetAllCountries();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockCountryRepository.Verify(c => c.GetAllCountries(), Times.Once);
        }

        [Fact]
        public void GetAllCountries_ReturnsFalse_WhenCountriesAreNull()
        {
            // Arrange
            List<Country> countries = null;

            var expectedResponse = new ServiceResponse<IEnumerable<CountryDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockCountryRepository = new Mock<ICountryRepository>();

            mockCountryRepository.Setup(c => c.GetAllCountries()).Returns(countries);

            var target = new CountryService(mockCountryRepository.Object);

            // Act
            var actual = target.GetAllCountries();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockCountryRepository.Verify(c => c.GetAllCountries(), Times.Once);
        }

        [Fact]
        public void GetAllCountries_ReturnsFalse_WhenNoCountriesExist()
        {
            // Arrange
            var countries = new List<Country>();

            var expectedResponse = new ServiceResponse<IEnumerable<CountryDto>>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockCountryRepository = new Mock<ICountryRepository>();

            mockCountryRepository.Setup(c => c.GetAllCountries()).Returns(countries);

            var target = new CountryService(mockCountryRepository.Object);

            // Act
            var actual = target.GetAllCountries();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockCountryRepository.Verify(c => c.GetAllCountries(), Times.Once);
        }

        // GetCountryById()
        [Fact]
        public void GetCountryById_ReturnsCountry_WhenCountryIsFound()
        {
            // Arrange
            int countryId = 1;
            var country = new Country()
            {
                CountryId = 1,
                CountryName = "Country 1",
            };

            var expectedResponse = new ServiceResponse<CountryDto>()
            {
                Success = true,
                Message = "Success",
            };

            var mockCountryRepository = new Mock<ICountryRepository>();

            mockCountryRepository.Setup(c => c.GetCountryById(countryId)).Returns(country);

            var target = new CountryService(mockCountryRepository.Object);

            // Act
            var actual = target.GetCountryById(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockCountryRepository.Verify(c => c.GetCountryById(countryId), Times.Once);
        }

        [Fact]
        public void GetCountryById_ReturnsFalse_WhenCountryIsNotFound()
        {
            // Arrange
            int countryId = 1;
            Country country = null;

            var expectedResponse = new ServiceResponse<CountryDto>()
            {
                Success = false,
                Message = "No record found",
            };

            var mockCountryRepository = new Mock<ICountryRepository>();

            mockCountryRepository.Setup(c => c.GetCountryById(countryId)).Returns(country);

            var target = new CountryService(mockCountryRepository.Object);

            // Act
            var actual = target.GetCountryById(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockCountryRepository.Verify(c => c.GetCountryById(countryId), Times.Once);
        }
    }
}
