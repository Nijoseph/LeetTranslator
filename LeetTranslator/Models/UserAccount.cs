using System.ComponentModel.DataAnnotations.Schema;

namespace LeetTranslator.Models
{

    public class UserAccount
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class UserAccountDetails : UserAccount
    {

        public ICollection<Translation> Translations { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }
        public ICollection<UserActivity> UserActivities { get; set; }
        public UserAccountDetails ()
        {
            Translations = new List<Translation>();
            UserNotifications = new List<UserNotification>();
            UserActivities = new List<UserActivity>();
        }

    }
}