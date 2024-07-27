using APIPhonebook.Data.Contract;
using APIPhonebook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APIPhonebook.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly IAppDbContext _appDbContext;

        public ContactRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Contact> GetAllContacts(int page, int page_size, string? search_string, string sort_dir, bool show_favourites)
        {
            var contacts = _appDbContext.Contacts
                                        .Include(c => c.Country)
                                        .Include(c => c.State)
                                        .AsQueryable();
            if (show_favourites)
            {
                contacts = contacts.Where(c => c.IsFavourite);
            }

            if (search_string != null)
            {
                if(search_string.StartsWith('*') && search_string.EndsWith('*'))
                {
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c => c.FirstName.ToLower().Contains(search_string.ToLower())
                                                || c.LastName.ToLower().Contains(search_string.ToLower())
                                                || c.ContactNumber.Contains(search_string));
                }
                else if(search_string.StartsWith('*'))
                {
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c => c.FirstName.ToLower().EndsWith(search_string.ToLower())
                                                || c.LastName.ToLower().EndsWith(search_string.ToLower())
                                                || c.ContactNumber.Contains(search_string));
                }
                else
                {
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c => c.FirstName.ToLower().StartsWith(search_string.ToLower())
                                                || c.LastName.ToLower().StartsWith(search_string.ToLower()) 
                                                || c.ContactNumber.Contains(search_string));
                }
            }

            if(sort_dir == "asc")
            {
                contacts = contacts.OrderBy(c => c.FirstName);
            }
            else if(sort_dir == "desc")
            {
                contacts = contacts.OrderByDescending(c => c.FirstName);
            } 
            else
            {
                contacts = contacts.OrderBy(c => c.ContactId);
            }

            int skip = (page - 1) * page_size;

            return contacts
                .Skip(skip)
                .Take(page_size)
                .ToList();
        }

        public Contact GetContactById(int contactId)
        {
            List<Contact> contacts = _appDbContext.Contacts.Include(c => c.Country).Include(c => c.State).ToList();
            //Contact contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId == contactId);
            Contact contact = contacts.FirstOrDefault(c => c.ContactId == contactId);
            return contact;
        }

        public int TotalContacts(string? search_string, bool show_favourites)
        {
            var contacts = _appDbContext.Contacts
                                        .Include(c => c.Country)
                                        .Include(c => c.State)
                                        .AsQueryable();
            if (show_favourites)
            {
                contacts = contacts.Where(c => c.IsFavourite);
            }

            if (search_string != null)
            {
                if (search_string.StartsWith('*') && search_string.EndsWith('*'))
                {
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c => c.FirstName.ToLower().Contains(search_string.ToLower())
                                                || c.LastName.ToLower().Contains(search_string.ToLower())
                                                || c.ContactNumber.Contains(search_string));
                }
                else if (search_string.StartsWith('*'))
                {
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c => c.FirstName.ToLower().EndsWith(search_string.ToLower())
                                                || c.LastName.ToLower().EndsWith(search_string.ToLower())
                                                || c.ContactNumber.Contains(search_string));
                }
                else
                {
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c => c.FirstName.ToLower().StartsWith(search_string.ToLower())
                                                || c.LastName.ToLower().StartsWith(search_string.ToLower())
                                                || c.ContactNumber.Contains(search_string));
                }
            }

            return contacts.Count();
        }

        public bool AddContact(Contact contact)
        {
            bool result = false;
            if (contact != null)
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
            if (contact != null)
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
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactNumber == contactNumber);
            if (contact != null)
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
