namespace LeetTranslator.Core.Models
{
    public class TranslationType
    {
        public int TranslationTypeId { get; set; }
        public string TypeName { get; set; }
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class TranslationType4View : TranslationType
    {
        public ICollection<Translation> Translations { get; set; }
        public TranslationType4View()
        {
            Translations = new List<Translation>();
        }
    }

}