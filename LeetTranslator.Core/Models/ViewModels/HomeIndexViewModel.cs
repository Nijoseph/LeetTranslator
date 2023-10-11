namespace LeetTranslator.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Translation> Translations { get; set; }
        public IEnumerable<TranslationType> TranslationTypes { get; set; }
    }
}
