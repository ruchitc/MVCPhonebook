using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;
using APIPhonebook.Models;
using APIPhonebook.Services.Contract;

namespace APIPhonebook.Services.Implementation
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

        public PaginationServiceResponse<IEnumerable<ContactDto>> GetAllContacts(int page, int page_size, string? search_string, string sort_dir, bool show_favourites)
        {
            var response = new PaginationServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAllContacts(page, page_size, search_string, sort_dir, show_favourites);
            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtoList = new List<ContactDto>();
                foreach (var contact in contacts)
                {
                    ContactDto contactDto = new ContactDto();
                    contactDto.ContactId = contact.ContactId;
                    contactDto.FirstName = contact.FirstName;
                    contactDto.LastName = contact.LastName;
                    contactDto.ContactNumber = contact.ContactNumber;
                    contactDto.Gender = contact.Gender;
                    contactDto.CountryId = contact.CountryId;
                    contactDto.StateId = contact.StateId;
                    contactDto.IsFavourite = contact.IsFavourite;
                    contactDto.Email = contact.Email;
                    contactDto.Company = contact.Company;
                    contactDto.FileName = contact.FileName;
                    contactDto.ImageBytes = contact.Image;
                    contactDto.Country = new CountryDto()
                    {
                        CountryId = contact.Country.CountryId,
                        CountryName = contact.Country.CountryName,
                    };
                    contactDto.State = new StateDto()
                    {
                        StateId = contact.State.StateId,
                        StateName = contact.State.StateName,
                        CountryId = contact.State.CountryId,
                    };
                    contactDtoList.Add(contactDto);
                }

                response.Data = contactDtoList;
                response.Total = _contactRepository.TotalContacts(search_string, show_favourites);
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

        public ServiceResponse<ContactDto> GetContactById(int contactId)
        {
            var response = new ServiceResponse<ContactDto>();
            var contact = _contactRepository.GetContactById(contactId);

            if (contact != null)
            {
                var contactDto = new ContactDto()
                {
                    ContactId = contact.ContactId,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    ContactNumber = contact.ContactNumber,
                    Gender = contact.Gender,
                    CountryId = contact.CountryId,
                    StateId = contact.StateId,
                    IsFavourite = contact.IsFavourite,
                    Email = contact.Email,
                    Company = contact.Company,
                    FileName = contact.FileName,
                    ImageBytes = contact.Image,
                    Country = new CountryDto()
                    {
                        CountryId = contact.Country.CountryId,
                        CountryName = contact.Country.CountryName,
                    },
                    State = new StateDto()
                    {
                        StateId = contact.State.StateId,
                        StateName = contact.State.StateName,
                        CountryId = contact.State.CountryId,
                    },
                };

                response.Data = contactDto;
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

        public ServiceResponse<int> TotalContacts(string? search_string, bool show_favourites)
        {
            var response = new ServiceResponse<int>();
            int count = _contactRepository.TotalContacts(search_string, show_favourites);

            response.Data = count;
            response.Success = true;

            return response;
        }

        public ServiceResponse<string> AddContact(AddContactDto contactDto)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contactDto.ContactNumber))
            {
                response.Success = false;
                response.Message = "Contact number already exists.";
                return response;
            }

            var contact = new Contact()
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                ContactNumber = contactDto.ContactNumber,
                Gender = contactDto.Gender,
                CountryId = contactDto.CountryId,
                StateId = contactDto.StateId,
                IsFavourite = contactDto.IsFavourite,
                Email = contactDto.Email,
                Company = contactDto.Company,
            };

            byte[] imageBytes;
            // Get bytes and file name if image is not null
            if(contactDto.Image != null)
            {
                imageBytes = _fileService.ToByteArray(contactDto.Image);
                contact.Image = imageBytes;

                // Getting the file name
                contact.FileName = contactDto.Image.FileName;
            }

            var result = _contactRepository.AddContact(contact);
            if(result)
            {
                response.Success = true;
                response.Message = "Contact saved successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, Please try after some time";
            }

            return response;
        }

        public ServiceResponse<string> UpdateContact(UpdateContactDto contactDto)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contactDto.ContactId, contactDto.ContactNumber))
            {
                response.Success = false;
                response.Message = "Contact number already exists.";
                return response;
            }

            var existingContact = _contactRepository.GetContactById(contactDto.ContactId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = contactDto.FirstName;
                existingContact.LastName = contactDto.LastName;
                existingContact.ContactNumber = contactDto.ContactNumber;
                existingContact.Gender = contactDto.Gender;
                existingContact.CountryId = contactDto.CountryId;
                existingContact.StateId = contactDto.StateId;
                existingContact.IsFavourite = contactDto.IsFavourite;
                existingContact.Email = contactDto.Email;
                existingContact.Company = contactDto.Company;
                
                byte[] imageBytes;
                // Get bytes and file name if image is not null
                if (contactDto.Image != null)
                {
                    imageBytes = _fileService.ToByteArray(contactDto.Image);
                    existingContact.Image = imageBytes;

                    // Getting the file name
                    existingContact.FileName = contactDto.Image.FileName;
                }

                result = _contactRepository.UpdateContact(existingContact);
            }

            if(result)
            {
                response.Success = true;
                response.Message = "Contact updated successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, Please try after some time";
            }

            return response;
        }

        public ServiceResponse<string> DeleteContact(int contactId)
        {
            var response = new ServiceResponse<string>();
            var result = _contactRepository.DeleteContact(contactId);
            if (result)
            {
                response.Success = true;
                response.Message = "Contact deleted successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after some time.";
            }

            return response;
        }
    }
}
