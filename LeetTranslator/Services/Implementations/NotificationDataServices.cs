using Dapper;
using LeetTranslator.Models;
using LeetTranslator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LeetTranslator.Services.Implementations
{
    public class NotificationDataServices : INotificationDataServices
    {
        private readonly string _connectionString;

        public NotificationDataServices(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Notification>("GetAllNotifications", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { NotificationId = notificationId };
                return await db.QuerySingleOrDefaultAsync<Notification>("GetNotificationById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> InsertNotificationAsync(Notification notification)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Title", notification.Title);
                parameters.Add("@Description", notification.Description);
                parameters.Add("@Date", notification.Date);
                parameters.Add("@IsDeleted", notification.IsDeleted);

                return await db.ExecuteScalarAsync<int>("InsertNotification", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@NotificationId", notification.NotificationId);
                parameters.Add("@Title", notification.Title);
                parameters.Add("@Description", notification.Description);
                parameters.Add("@Date", notification.Date);
                parameters.Add("@IsDeleted", notification.IsDeleted);

                await db.ExecuteAsync("UpdateNotification", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { NotificationId = notificationId };
                await db.ExecuteAsync("DeleteNotification", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}