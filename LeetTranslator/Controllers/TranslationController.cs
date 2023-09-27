using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeetTranslator.Services.Interfaces;
using LeetTranslator.Models;

namespace LeetTranslator.Controllers
{
    public class TranslationController : Controller
    {
        private readonly ITranslationDataServices _translationDataService;

        public TranslationController (ITranslationDataServices translationDataService)
        {
            _translationDataService = translationDataService;
        }

        public async Task<IActionResult> Index ()
        {
            var translations = await _translationDataService.GetAllTranslationsAsync();
            return View(translations); // Ensure you have an Index view setup to display translations.
        }

        [HttpGet]
        public IActionResult Create ()
        {
            return View(); // Return a view to create a new translation
        }

        [HttpPost]
        public async Task<IActionResult> Create (Translation translation)
        {
            if ( ModelState.IsValid )
            {
                await _translationDataService.InsertTranslationAsync(translation);
                return RedirectToAction("Index");
            }

            return View(translation);
        }

        // Implement other necessary actions for update, delete, etc. as needed.

        // AJAX endpoint for DataTables
        [HttpGet("api/translations")]
        public async Task<IActionResult> GetTranslationsForDataTable ()
        {
            try
            {
                var translations = await _translationDataService.GetAllTranslationsAsync();
                return Json(new { data = translations });
            }
            catch ( Exception ex )
            {
                // Log or handle the exception as needed
                return BadRequest(ex.Message);
            }
        }
    }
}
