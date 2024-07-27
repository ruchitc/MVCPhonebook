using APIPhonebook.Data.Contract;
using APIPhonebook.Models;

namespace APIPhonebook.Data.Implementation
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IAppDbContext _appDbContext;

        public CountryRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            List<Country> countries = _appDbContext.Countries.ToList();
            return countries;
        }

        public Country GetCountryById(int countryId)
        {
            return _appDbContext.Countries.FirstOrDefault(c => c.CountryId == countryId);
        }
    }
}
