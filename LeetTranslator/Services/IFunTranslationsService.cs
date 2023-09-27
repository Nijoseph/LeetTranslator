using LeetTranslator.Models;

namespace LeetTranslator.Services
{
    public interface IFunTranslationsService
    {
        Task<FunTranslationsResponse> Translate (string text);
    }
}