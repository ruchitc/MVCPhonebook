using APIPhonebook.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhonebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllCountries")]
        public IActionResult GetAllCountries()
        {
            var response = _countryService.GetAllCountries();
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetCountryById/{countryId}")]
        public IActionResult GetCountryById(int countryId)
        {
            var response = _countryService.GetCountryById(countryId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
