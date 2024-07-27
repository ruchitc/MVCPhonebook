using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCPhonebook.Models;
using MVCPhonebook.Services.Contract;
using MVCPhonebook.ViewModels;

namespace MVCPhonebook.Controllers
{
    public class PhonebookController : Controller
    {
        private readonly IContactService _contactService;

        public PhonebookController(IContactService contactService) 
        {
            _contactService = contactService;
        }

        public IActionResult Index(char? ch, int page = 1, int pageSize = 2)
        {
            ViewBag.CurrentPage = page;
            var totalCount = ch == null
                ? _contactService.TotalContacts()
                : _contactService.TotalContactsStartingWithLetter(ch);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var contacts = ch == null
                ? _contactService.GetPaginatedContacts(page, pageSize)
                : _contactService.GetPaginatedContactsStartingWithLetter(ch, page, pageSize);

            // Set ViewBag properties
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Ch = ch;

            return View(contacts);
        }

        public IActionResult Details(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            //Category category = new Category();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact()
                {
                    FirstName = contactViewModel.FirstName,
                    LastName = contactViewModel.LastName,
                    ContactNumber = contactViewModel.ContactNumber,
                    Email = contactViewModel.Email,
                    Company = contactViewModel.Company,
                };

                var result = _contactService.AddContact(contact, contactViewModel.File);
                if (result == "Contact number already exists." || result == "Something went wrong, please try after some time.")
                {
                    TempData["ErrorMessage"] = result;
                }
                else if (result == "Contact saved successfully.")
                {
                    TempData["SuccessMessage"] = result;
                    return RedirectToAction("Index");
                }
            }

            return View(contactViewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var contact = _contactService.GetContactById(id);
            if(contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if(ModelState.IsValid)
            {
                var message = _contactService.UpdateContact(contact);
                if (message == "Contact number already exists." || message == "Something went wrong, please try after some time.")
                {
                    TempData["ErrorMessage"] = message;
                }
                else
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }
            }
            return View(contact);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int contactId)
        {
            var result = _contactService.DeleteContact(contactId);
            if (result == "Contact deleted successfully.")
            {
                TempData["SuccessMessage"] = result;
            }
            else
            {
                TempData["ErrorMessage"] = result;
            }

            return RedirectToAction("Index");
        }
    }
}
