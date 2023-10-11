using LeetTranslator.Models;

public interface IUserNotificationDataServices
{
    Task DeleteUserNotificationAsync (int userNotificationId);
    Task<IEnumerable<UserNotification>> GetAllUserNotificationsAsync ();
    Task<UserNotification> GetUserNotificationByIdAsync (int userNotificationId);
    Task<IEnumerable<UserNotification>> GetUserNotificationsByUserIdAsync (int userId);
    Task<int> InsertUserNotificationAsync (UserNotification userNotification);
    Task MarkNotificationAsReadAsync (int userNotificationId);
    Task UpdateUserNotificationAsync (UserNotification userNotification);
}