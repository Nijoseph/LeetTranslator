using LeetTranslator.Core.Models;

namespace LeetTranslator.Services.Interfaces
{
    public interface ITranslationTypeDataServices
    {
        Task DeleteTranslationTypeAsync(int translationTypeId);
        Task<IEnumerable<TranslationType>> GetAllTranslationTypesAsync();
        Task<TranslationType> GetTranslationTypeByIdAsync(int translationTypeId);
        Task<int> InsertTranslationTypeAsync(TranslationType translationType);
        Task UpdateTranslationTypeAsync(TranslationType translationType);
    }
}