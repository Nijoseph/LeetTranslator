using LeetTranslator.Models;

namespace LeetTranslator.Services.Interfaces
{
    public interface IUserActivityDataServices
    {
        Task DeleteUserActivityAsync(int userActivityId);
        Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync();
        Task<IEnumerable<UserActivity>> GetUserActivitiesByUserIdAsync(int userId);
        Task<UserActivity> GetUserActivityByIdAsync(int userActivityId);
        Task<int> InsertUserActivityAsync(UserActivity userActivity);
        Task UpdateUserActivityAsync(UserActivity userActivity);
    }
}