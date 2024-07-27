using MVCPhonebook.Data.Contract;
using MVCPhonebook.Models;

namespace MVCPhonebook.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly IAppDbContext _appDbContext;

        public ContactRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            List<Contact> contacts = _appDbContext.Contacts.ToList();
            return contacts;
        }

        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            return _appDbContext.Contacts
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Contact> GetPaginatedContactsStartingWithLetter(char? ch, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            var contacts = _appDbContext.Contacts.Where(c => c.FirstName.StartsWith(ch.ToString().ToLower()));
            return contacts
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public Contact GetContactById(int contactId)
        {
            Contact contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId == contactId);
            return contact;
        }

        public int TotalContacts()
        {
            return _appDbContext.Contacts.Count();
        }

        public int TotalContactsStartingWithLetter(char? ch)
        {
            return _appDbContext.Contacts.Where(c => c.FirstName.StartsWith(ch.ToString().ToLower())).Count();
        }

        public bool AddContact(Contact contact)
        {
            bool result = false;
            if(contact != null)
            {
                _appDbContext.Contacts.Add(contact);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool UpdateContact(Contact contact)
        {
            var result = false;
            if(contact != null)
            {
                _appDbContext.Contacts.Update(contact);
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool DeleteContact(int contactId)
        {
            var result = false;
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId == contactId);
            if (contact != null)
            {
                _appDbContext.Contacts.Remove(contact);
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool ContactExists(string contactNumber)
        {
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactNumber ==  contactNumber);
            if(contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContactExists(int contactId, string contactNumber)
        {
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId != contactId && c.ContactNumber == contactNumber);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
