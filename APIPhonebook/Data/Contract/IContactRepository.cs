using APIPhonebook.Dtos;
using APIPhonebook.Models;

namespace APIPhonebook.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContacts(int page, int page_size, string? search_string, string sort_dir, bool show_favourites);
        Contact GetContactById(int contactId);
        int TotalContacts(string? search_string, bool show_favourites);
        bool AddContact(Contact contact);
        bool UpdateContact(Contact contact);
        bool DeleteContact(int contactId);
        bool ContactExists(string contactNumber);
        bool ContactExists(int contactId, string contactNumber);
    }
}
