using APIPhonebook.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhonebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [AllowAnonymous]
        [HttpGet("GetStatesByCountryId/{countryId}")]
        public IActionResult GetStatesByCountryId(int countryId)
        {
            var response = _stateService.GetStatesByCountryId(countryId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetStateById/{stateId}")]
        public IActionResult GetStateById(int stateId)
        {
            var response = _stateService.GetStateById(stateId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
