using MVCPhonebook.Models;

namespace MVCPhonebook.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContacts();
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize);
        IEnumerable<Contact> GetPaginatedContactsStartingWithLetter(char? ch, int page, int pageSize);
        Contact GetContactById(int contactId);
        int TotalContacts();
        int TotalContactsStartingWithLetter(char? ch);
        bool AddContact(Contact contact);
        bool UpdateContact(Contact contact);
        bool DeleteContact(int contactId);
        bool ContactExists(string contactNumber);
        bool ContactExists(int contactId, string contactNumber);
    }
}
