namespace LeetTranslator.Core.Models
{
    public class UserActivity
    {
        public int UserActivityId { get; set; }
        public int UserId { get; set; }
        public string ActivityType { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime ActivityDate { get; set; }
        public bool IsDeleted { get; set; }
    }

}