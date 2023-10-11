using LeetTranslator.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LeetTranslator.Services.Interfaces;
using LeetTranslator.Services;
using LeetTranslator.ApiIntegration;
using LeetTranslator.Core.Models.ViewModels;
using LeetTranslator.Core.Models;
using RegisterViewModel = LeetTranslator.Core.Models.RegisterViewModel;

namespace LeetTranslator.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserDataServices _userDataServices;
        private readonly ITranslationDataServices _translationDataServices; // Added this line
        private readonly ITranslationTypeDataServices _translationTypeDataServices;
        private readonly IFunTranslationsService _funTranslationsService;
        private readonly ITranslationTypeDataServices _service;



        public HomeController (
            IUserDataServices userDataServices,
            ITranslationDataServices translationDataServices,
            /*IFunTranslationsService funTranslationsService, */
            ITranslationTypeDataServices service,
            ITranslationTypeDataServices translationTypeDataServices) // Inject the service here
        {
            _userDataServices = userDataServices ?? throw new ArgumentNullException(nameof(userDataServices));
            _translationDataServices = translationDataServices ?? throw new ArgumentNullException(nameof(translationDataServices));
            _translationTypeDataServices = translationTypeDataServices ?? throw new ArgumentNullException(nameof(translationTypeDataServices));
            //_funTranslationsService = funTranslationsService ?? throw new ArgumentNullException(nameof(funTranslationsService));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        public async Task<IActionResult> Index ()
        {
            var viewModel = new HomeIndexViewModel();

            if ( User.Identity.IsAuthenticated )
            {
                viewModel.Translations = await _translationDataServices.GetAllTranslationsAsync();
                viewModel.TranslationTypes = await _translationTypeDataServices.GetAllTranslationTypesAsync();
                return View(viewModel);
            }
            return View(viewModel); // We can still pass an empty viewModel to the view
        }
        [HttpPost]
        public async Task<IActionResult> SaveTranslation (Translation translation)
        {
            // 1. Model Validation
            if ( !ModelState.IsValid )
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return Json(new { success = false, message = "There were validation errors.", errors = errors });
            }

            // 2. Saving Data
            try
            {
                translation.UserId = int.Parse(User.FindFirstValue("UserId"));
                translation.Date= DateTime.Now;
                translation.IsDeleted = false;
                int newTranslationId = await _translationDataServices.InsertTranslationAsync(translation);

                // Handle success
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                // Handle exceptions
                return Json(new { success = false, message = $"An error occurred while saving the translation. Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll ()
        {
            var types = await _translationTypeDataServices.GetAllTranslationTypesAsync();
            return Json(types);
        }
        [HttpPost]
        public async Task<IActionResult> SaveTranslationType (TranslationType model)
        {
            if ( ModelState.IsValid )
            {
                await _translationTypeDataServices.InsertTranslationTypeAsync(model);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> TranslateMethod (string textToTranslate, string typeId)
        {
            try
            {
                var translationType = await _translationTypeDataServices.GetTranslationTypeByIdAsync(int.Parse(typeId));
                var apiKey = translationType.ApiKey;
                var apiUrl = translationType.ApiUrl;

                var funTranslationsService = new FunTranslationsService(apiKey, apiUrl);
                var translationResponse = await funTranslationsService.Translate(textToTranslate);

                if ( translationResponse != null && translationResponse.Success.Total > 0 )
                {
                    var translatedText = translationResponse.Contents.Translated;
                    return Ok(new { success = true, translatedText = translatedText });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Translation failed or the response is invalid." });
                }
            }
            catch ( Exception ex )
            {
                // Log the exception (if you have a logging mechanism)
                return StatusCode(500, new { success = false, message = "Internal server error. Please try again later." });
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAllTranslationsForDataTable ()
        {
            var translations = await _translationDataServices.GetAllTranslationsWithDetailsAsync(); // Use 'await' here
            return Json(translations);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl ?? Url.Content("~/") });
        }

        public ClaimsIdentity CreateClaimsIdentity (UserAccount user)
        {
            var claims = new List<Claim>
    {
        new Claim("UserId", user.UserId.ToString()),
        new Claim("FirstName", user.FirstName ?? string.Empty),
        new Claim("LastName", user.LastName ?? string.Empty),
        new Claim("UserName", user.UserName ?? string.Empty),
        new Claim("UserRole", user.UserRole ?? string.Empty),
        new Claim("Email", user.Email ?? string.Empty),
        new Claim("DateCreated", user.DateCreated.ToString("o")),
        new Claim("LastLogin", user.LastLogin?.ToString("o") ?? string.Empty),
        new Claim("IsDeleted", user.IsDeleted.ToString())
    };

            return new ClaimsIdentity(claims, "custom", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userDataServices.GetUserByUserNameAsync(model.Username);

                    if (user != null && _userDataServices.HashPassword(model.Password, user.UserName) == user.PasswordHash)
                    {
                        var identity = CreateClaimsIdentity(user);

                        var principal = new ClaimsPrincipal(identity);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Name,$"{ user.LastName} { user.FirstName}"),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while logging in. Please try again later.");
            }

            return View(model);
      }


        [HttpGet]
       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new UserAccount
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName= model.FirstName,
                    DateCreated= DateTime.UtcNow,
                    IsDeleted=false,
                    LastLogin=DateTime.UtcNow,
                    LastName= model.LastName,
                    PasswordHash = model.Password,
                    UserRole="User" 
            };

                var result = await _userDataServices.InsertUserAsync(newUser);

                if (result > 0)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed. Please try again or contact support.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
            //return RedirectToAction("Index");
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
