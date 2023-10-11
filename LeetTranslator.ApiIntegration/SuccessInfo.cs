namespace LeetTranslator.ApiIntegration
{
    public class SuccessInfo
    {
        public int Total { get; set; }
    }
    public enum UserRole
    {
        Joseph,
        James,
        Unknown
    }
    public class TranslatorViewModel
    {
        public string UserName { get; set; }
        public UserRole UserRole { get; set; }
        public string Phrase { get; set; }
        public string LeetTranslation { get; set; }
    }
}
