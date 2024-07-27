using MVCPhonebook.Data.Contract;
using MVCPhonebook.Models;
using MVCPhonebook.Services.Contract;

namespace MVCPhonebook.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IFileService _fileService;

        public ContactService(IContactRepository contactRepository, IFileService fileService)
        {
            _contactRepository = contactRepository;
            _fileService = fileService;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            var contacts = _contactRepository.GetAllContacts();
            if (contacts != null && contacts.Any())
            {
                foreach(var contact in contacts.Where(c => c.FileName == string.Empty))
                {
                    contact.FileName = "default-image.png";
                }

                return contacts;
            }

            return new List<Contact>();
        }

        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize)
        {
            var contacts = _contactRepository.GetPaginatedContacts(page, pageSize);
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts.Where(c => c.FileName == string.Empty))
                {
                    contact.FileName = "default-image.png";
                }

                return contacts;
            }

            return new List<Contact>();
        }

        public IEnumerable<Contact> GetPaginatedContactsStartingWithLetter(char? ch, int page, int pageSize)
        {
            var contacts = _contactRepository.GetPaginatedContactsStartingWithLetter(ch, page, pageSize);
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts.Where(c => c.FileName == string.Empty))
                {
                    contact.FileName = "default-image.png";
                }

                return contacts;
            }

            return new List<Contact>();
        }

        public Contact? GetContactById(int contactId)
        {
            var contact = _contactRepository.GetContactById(contactId);
            return contact;
        }

        public int TotalContacts()
        {
            return _contactRepository.TotalContacts();
        }

        public int TotalContactsStartingWithLetter(char? ch)
        {
            return _contactRepository.TotalContactsStartingWithLetter(ch);
        }

        public string AddContact(Contact contact, IFormFile file)
        {
            if (_contactRepository.ContactExists(contact.ContactNumber))
            {
                return "Contact number already exists.";
            }
            
            contact.FileName = _fileService.AddFileToUploads(file);
            var result = _contactRepository.AddContact(contact);
            return result ? "Contact saved successfully." : "Something went wrong, please try after some time.";
        }

        public string UpdateContact(Contact contact)
        {
            var message = string.Empty;
            if (_contactRepository.ContactExists(contact.ContactId, contact.ContactNumber))
            {
                message = "Contact number already exists.";
                return message;
            }

            var existingContact = _contactRepository.GetContactById(contact.ContactId);

            var result = false;
            if (existingContact!= null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.ContactNumber = contact.ContactNumber;
                existingContact.Email = contact.Email;
                existingContact.Company = contact.Company;
                /*existingContact.FileName = contact.FileName;
                existingContact.FileName = "default-image.png";*/
                result = _contactRepository.UpdateContact(existingContact);
            }

            message = result ? "Contact updated successfully." : "Something went wrong, please try after some time.";

            return message;
        }

        public string DeleteContact(int contactId)
        {
            var result = _contactRepository.DeleteContact(contactId);
            if (result)
            {
                return "Contact deleted successfully.";
            }
            else
            {
                return "Something went wrong, please try after some time.";
            }
        }
    }
}
