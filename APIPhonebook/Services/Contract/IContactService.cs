using APIPhonebook.Dtos;
using APIPhonebook.Models;

namespace APIPhonebook.Services.Contract
{
    public interface IContactService
    {
        PaginationServiceResponse<IEnumerable<ContactDto>> GetAllContacts(int page, int page_size, string? search_string, string sort_dir, bool show_favourites);
        ServiceResponse<ContactDto> GetContactById(int contactId);
        ServiceResponse<int> TotalContacts(string? search_string, bool show_favourites);
        ServiceResponse<string> AddContact(AddContactDto contactDto);
        ServiceResponse<string> UpdateContact(UpdateContactDto contactDto);
        ServiceResponse<string> DeleteContact(int contactId);
    }
}
