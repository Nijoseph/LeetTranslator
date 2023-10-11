namespace LeetTranslator.ApiIntegration
{
    public interface IFunTranslationsService
    {
        Task<FunTranslationsResponse> Translate(string text);
    }
}