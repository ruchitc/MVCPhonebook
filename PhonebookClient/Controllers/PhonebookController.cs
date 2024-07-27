using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhonebookClient.Infrastructure;
using PhonebookClient.ViewModels;

namespace PhonebookClient.Controllers
{
    
    public class PhonebookController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly string endPoint;

        public PhonebookController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:PhonebookApi"];
        }

        [AllowAnonymous]
        public IActionResult Index(int page = 1, int pageSize = 8, string searchString = "", string sortDir = "default", bool showFavourites = false)
        {
            if(searchString == null)
            {
                searchString = "";
            }
            var apiUrl = $"{endPoint}Contact/GetAllContacts"
                + "?page=" + page
                + "&page_size=" + pageSize
                + "&search_string=" + searchString
                + "&sort_dir=" + sortDir
                + "&show_favourites=" + showFavourites;

            PaginationServiceResponse<IEnumerable<ContactViewModel>> response = new PaginationServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<PaginationServiceResponse<IEnumerable<ContactViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            var totalCount = response.Total;

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.SortDir = sortDir;
            ViewBag.ShowFavourites = showFavourites;

            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");

            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<CountryViewModel> countries = GetCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(AddContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                string apiUrl = $"{endPoint}Contact/AddContact";

                var response = _httpClientService.PostHttpResponseMessageFormData<AddContactViewModel>
                    (apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong, please try after some time.";
                    }
                }
            }

            IEnumerable<CountryViewModel> countries = GetCountries();
            ViewBag.Countries = countries;
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;

            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>
                (apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    IEnumerable<CountryViewModel> countries = GetCountries();
                    ViewBag.Countries = countries;
                    ViewBag.StateId = serviceResponse.Data.StateId;
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(UpdateContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var apiUrl = $"{endPoint}Contact/UpdateContact";

                var response = _httpClientService.PutHttpResponseMessageFormData<UpdateContactViewModel>
                    (apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                }
            }

            IEnumerable<CountryViewModel> countries = GetCountries();
            ViewBag.Countries = countries;
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;

            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>
                (apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteConfirmed(int contactId)
        {
            var apiUrl = $"{endPoint}Contact/DeleteContact/" + contactId;

            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>
                ($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);

            if (response != null && response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong";
                return RedirectToAction("Index");
            }
        }

        private IEnumerable<CountryViewModel> GetCountries()
        {
            var apiUrl = $"{endPoint}Country/GetAllCountries";

            ServiceResponse<IEnumerable<CountryViewModel>> response = new ServiceResponse<IEnumerable<CountryViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }
    }
}
