using LeetTranslator.Core.Models;

namespace LeetTranslator.Services.Interfaces
{
    public interface ITranslationDataServices
    {
        Task DeleteTranslationAsync(int translationId);
        Task<IEnumerable<Translation>> GetAllTranslationsAsync();
        Task<TranslationResult> GetAllTranslationsWithDetailsAsync ();
        Task<Translation> GetTranslationByIdAsync(int translationId);
        Task<int> InsertTranslationAsync(Translation translation);
        Task UpdateTranslationAsync(Translation translation);
    }
}