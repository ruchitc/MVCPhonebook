using APIPhonebook.Dtos;

namespace APIPhonebook.Services.Contract
{
    public interface ICountryService
    {
        ServiceResponse<IEnumerable<CountryDto>> GetAllCountries();
        ServiceResponse<CountryDto> GetCountryById(int countryId);
    }
}
