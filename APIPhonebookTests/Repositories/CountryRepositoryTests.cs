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
    public class CountryRepositoryTests
    {
        
        // GetAllCountries()
        [Fact]
        public void GetAllCountries_ReturnsCountries_WhenCountriesExist()
        {
            // Arrange
            var countries = new List<Country>()
            {
                new Country { CountryId = 1, CountryName = "Country 1"},
                new Country { CountryId = 2, CountryName = "Country 2"}
            };

            var mockDbSet = new Mock<DbSet<Country>>();
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countries.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllCountries();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(countries.Count(), actual.Count());
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once());
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
        }

        [Fact]
        public void GetAllCountries_ReturnsNull_WhenNoCountriesExist()
        {
            // Arrange
            var countries = new List<Country>().AsQueryable();

            var mockDbSet = new Mock<DbSet<Country>>();
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countries.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllCountries();

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(countries.Count(), actual.Count());
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once());
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
        }

        // GetCountryById()
        [Fact]
        public void GetCountryById_ReturnsCountry_WhenCountryFound()
        {
            // Arrange
            int countryId = 1;
            Country country = new Country
            {
                CountryId = countryId,
                CountryName = "Country 1"
            };

            var countries = new List<Country>() { country }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Country>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Provider).Returns(countries.Provider);
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Expression).Returns(countries.Expression);
            mockAppDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetCountryById(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(country, actual);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
        }

        [Fact]
        public void GetCountryById_ReturnsNull_WhenCountryDoesntExist()
        {
            // Arrange
            int countryId = 2;
            Country country = new Country
            {
                CountryId = 1,
                CountryName = "Country 1"
            };

            var countries = new List<Country>() { country }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Country>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Provider).Returns(countries.Provider);
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Expression).Returns(countries.Expression);
            mockAppDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetCountryById(countryId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
        }
    }
}
