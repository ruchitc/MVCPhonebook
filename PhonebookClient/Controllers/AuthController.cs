using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhonebookClient.Infrastructure;
using PhonebookClient.ViewModels;

namespace PhonebookClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;

        public AuthController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:PhonebookApi"];
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var apiUrl = $"{endPoint}Auth/GetUserDetails/"
                + User.Identity.Name;

            ServiceResponse<UserDetailsViewModel> response = new ServiceResponse<UserDetailsViewModel>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>(apiUrl, HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new UserDetailsViewModel());
        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdateUserDetails()
        {
            var apiUrl = $"{endPoint}Auth/GetUserDetails/" + User.Identity.Name;

            var response = _httpClientService.GetHttpResponseMessage<UserDetailsViewModel>
                (apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UserDetailsViewModel>>(data);

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
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UserDetailsViewModel>>(errorData);

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
        public IActionResult UpdateUserDetails(UpdateUserDetailsViewModel updateUserDetailsViewModel)
        {
            if(ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/UpdateUserDetails";

                var response = _httpClientService.PutHttpResponseMessage<UpdateUserDetailsViewModel>
                    (apiUrl, updateUserDetailsViewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    string token = serviceResponse.Data;

                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Profile");
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
            return View();

        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdatePassword(UpdatePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ChangePassword";

                var response = _httpClientService.PutHttpResponseMessage<UpdatePasswordViewModel>
                    (apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Profile");
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
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegisterUser()
        {
            IEnumerable<SecurityQuestionViewModel> securityQuestions = GetSecurityQuestions();
            ViewBag.SecurityQuestions = securityQuestions;
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterUser(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/Register";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, registerViewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(data);
                        
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("RegisterSuccess");
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
            return View(registerViewModel);
        }

        [AllowAnonymous]
        public IActionResult RegisterSuccess()
        {
            return View();
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginUser()
        {
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginUser(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = $"{endPoint}Auth/Login";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    string token = serviceResponse.Data;
                    
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
                    return RedirectToAction("Index", "Phonebook");
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
            return View(viewModel);
        }
        
        [AllowAnonymous]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string loginId)
        {
            if(loginId == null)
            {
                return RedirectToAction("ForgotPassword");
            }

            var apiUrl = $"{endPoint}Auth/GetUserSecurityQuestions/" + loginId;

            var response = _httpClientService.GetHttpResponseMessage<UserSecurityQuestionsViewModel>
                (apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UserSecurityQuestionsViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    ResetPasswordViewModel viewModel = new ResetPasswordViewModel();
                    viewModel.LoginId = serviceResponse.Data.LoginId;
                    viewModel.SecurityQuestionId_1 = serviceResponse.Data.SecurityQuestion_1.QuestionId;
                    viewModel.SecurityQuestion_1 = serviceResponse.Data.SecurityQuestion_1.Question;
                    viewModel.SecurityQuestionId_2 = serviceResponse.Data.SecurityQuestion_2.QuestionId;
                    viewModel.SecurityQuestion_2 = serviceResponse.Data.SecurityQuestion_1.Question;
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return View(loginId);
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UserSecurityQuestionsViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("ForgotPassword");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ResetPassword";

                var response = _httpClientService.PutHttpResponseMessage<ResetPasswordViewModel>
                    (apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("LoginUser");
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
            return View(viewModel);
        }

        private IEnumerable<SecurityQuestionViewModel> GetSecurityQuestions()
        {
            var apiUrl = $"{endPoint}Auth/GetSecurityQuestions";

            ServiceResponse<IEnumerable<SecurityQuestionViewModel>> response = new ServiceResponse<IEnumerable<SecurityQuestionViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<SecurityQuestionViewModel>>>(apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }
    }
}
