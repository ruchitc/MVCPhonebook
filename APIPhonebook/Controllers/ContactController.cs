using APIPhonebook.Dtos;
using APIPhonebook.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhonebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts(int page = 1, int page_size = 10, string? search_string = null, string sort_dir = "default", bool show_favourites = false)
        {
            var response = _contactService.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetContactById/{contactId}")]
        public IActionResult GetContactById(int contactId)
        {
            var response = _contactService.GetContactById(contactId);
            if(!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetTotalContacts")]
        public IActionResult GetTotalContacts(string? search_string, bool show_favourites)
        {
            var response = _contactService.TotalContacts(search_string, show_favourites);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("AddContact")]
        public IActionResult AddContact([FromForm] AddContactDto contactDto)
        {
            var response = _contactService.AddContact(contactDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPut("UpdateContact")]
        public IActionResult UpdateContact([FromForm] UpdateContactDto contactDto)
        {
            var response = _contactService.UpdateContact(contactDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DeleteContact/{contactId}")]
        public IActionResult DeleteContact(int contactId)
        {
            if (contactId > 0)
            {
                var response = _contactService.DeleteContact(contactId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                return BadRequest("Please enter proper data");
            }
        }
    }
}
