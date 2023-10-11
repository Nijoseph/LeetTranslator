namespace LeetTranslator.Core.Models
{
    public class TranslationView
    {
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TypeName { get; set; }
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public int TranslationId { get; set; }
    }
    public class TranslationResult
    {
        public IEnumerable<TranslationView> ListOfTranslations { get; set; }
        public int TotalCount { get; set; }
    }

}
