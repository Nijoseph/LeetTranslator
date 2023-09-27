using LeetTranslator.Models;

namespace LeetTranslator.Services.Interfaces
{
    public interface INotificationDataServices
    {
        Task DeleteNotificationAsync(int notificationId);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task<int> InsertNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
    }
}