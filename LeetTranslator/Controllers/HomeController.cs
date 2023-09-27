using LeetTranslator.Models;
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

namespace LeetTranslator.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserDataServices _userDataServices;
        private readonly ITranslationDataServices _translationDataServices; // Added this line
        private readonly ITranslationTypeDataServices _translationTypeDataServices;
        private readonly IFunTranslationsService _funTranslationsService;


        public HomeController (
            IUserDataServices userDataServices,
            ITranslationDataServices translationDataServices,
            IFunTranslationsService funTranslationsService,
            ITranslationTypeDataServices translationTypeDataServices) // Inject the service here
        {
            _userDataServices = userDataServices ?? throw new ArgumentNullException(nameof(userDataServices));
            _translationDataServices = translationDataServices ?? throw new ArgumentNullException(nameof(translationDataServices));
            _translationTypeDataServices = translationTypeDataServices ?? throw new ArgumentNullException(nameof(translationTypeDataServices));
            _funTranslationsService = funTranslationsService ?? throw new ArgumentNullException(nameof(funTranslationsService));
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
        public async Task<IActionResult> TranslateMethod (string textToTranslate)
        {
            try
            {
                var translationResponse = await _funTranslationsService.Translate(textToTranslate);

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
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

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
        [Authorize]
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
                };

                newUser.PasswordHash = _userDataServices.HashPassword(model.Password, newUser.UserName);
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
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
