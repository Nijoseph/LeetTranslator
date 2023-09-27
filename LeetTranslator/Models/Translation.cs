namespace LeetTranslator.Models
{
    public class Translation
    {
        public int TranslationId { get; set; }
        public int UserId { get; set; }
        public int TranslationTypeId { get; set; }
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class Translation4View : Translation
    {
        public UserAccount UserAccount { get; set; }
        public TranslationType TranslationType { get; set; }
    }

}