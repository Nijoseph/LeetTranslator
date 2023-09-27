namespace LeetTranslator.Models
{
    public class UserNotification
    {
        public int UserNotificationId { get; set; }
        public int UserId { get; set; }
        public int NotificationId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
    }
}