using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeetTranslator.Services.Interfaces;
using LeetTranslator.Core.Models;

namespace LeetTranslator.Controllers
{
    public class TranslationController : Controller
    {
        private readonly ITranslationDataServices _translationDataService;

        public TranslationController(ITranslationDataServices translationDataService)
        {
            _translationDataService = translationDataService;
        }

        public async Task<IActionResult> Index()
        {
            var translations = await _translationDataService.GetAllTranslationsAsync();
            return View(translations);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Translation translation)
        {
            if (ModelState.IsValid)
            {
                await _translationDataService.InsertTranslationAsync(translation);
                return RedirectToAction("Index");
            }

            return View(translation);
        }

        [HttpGet("api/translations")]
        public async Task<IActionResult> GetTranslationsForDataTable()
        {
            try
            {
                var translations = await _translationDataService.GetAllTranslationsAsync();
                return Json(new { data = translations });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
