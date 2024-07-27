using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;
using APIPhonebook.Services.Contract;

namespace APIPhonebook.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public ServiceResponse<IEnumerable<CountryDto>> GetAllCountries()
        {
            var response = new ServiceResponse<IEnumerable<CountryDto>>();
            var countries = _countryRepository.GetAllCountries();

            if(countries != null && countries.Any())
            {
                List<CountryDto> countryDtoList = new List<CountryDto>();
                foreach(var country in countries)
                {
                    CountryDto countryDto = new CountryDto();
                    countryDto.CountryId = country.CountryId;
                    countryDto.CountryName = country.CountryName;

                    countryDtoList.Add(countryDto);
                }

                response.Data = countryDtoList;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }

        public ServiceResponse<CountryDto> GetCountryById(int countryId)
        {
            var response = new ServiceResponse<CountryDto>();
            var country = _countryRepository.GetCountryById(countryId);

            if (country != null)
            {
                var countryDto = new CountryDto()
                {
                    CountryId = country.CountryId,
                    CountryName = country.CountryName,
                };

                response.Data = countryDto;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
    }
}
