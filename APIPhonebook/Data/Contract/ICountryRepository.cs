using APIPhonebook.Models;

namespace APIPhonebook.Data.Contract
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAllCountries();
        Country GetCountryById(int countryId);
    }
}
