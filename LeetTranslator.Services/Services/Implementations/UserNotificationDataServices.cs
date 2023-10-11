using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LeetTranslator.Core.Models;

public class UserNotificationDataServices : IUserNotificationDataServices
{
    private readonly string _connectionString;

    public UserNotificationDataServices (string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<IEnumerable<UserNotification>> GetAllUserNotificationsAsync ()
    {
        using ( IDbConnection db = new SqlConnection(_connectionString) )
        {
            return await db.QueryAsync<UserNotification>("GetAllUserNotifications", commandType: CommandType.StoredProcedure);
        }
    }

    public async Task<UserNotification> GetUserNotificationByIdAsync (int userNotificationId)
    {
        using ( IDbConnection db = new SqlConnection(_connectionString) )
        {
            var parameters = new { UserNotificationId = userNotificationId };
            return await db.QuerySingleOrDefaultAsync<UserNotification>("GetUserNotificationById", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task<int> InsertUserNotificationAsync (UserNotification userNotification)
    {
        using ( IDbConnection db = new SqlConnection(_connectionString) )
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userNotification.UserId);
            parameters.Add("@NotificationId", userNotification.NotificationId);
            parameters.Add("@IsRead", userNotification.IsRead);
            parameters.Add("@DateRead", userNotification.DateRead);

            return await db.ExecuteScalarAsync<int>("InsertUserNotification", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task UpdateUserNotificationAsync (UserNotification userNotification)
    {
        using ( IDbConnection db = new SqlConnection(_connectionString) )
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserNotificationId", userNotification.UserNotificationId);
            parameters.Add("@UserId", userNotification.UserId);
            parameters.Add("@NotificationId", userNotification.NotificationId);
            parameters.Add("@IsRead", userNotification.IsRead);
            parameters.Add("@DateRead", userNotification.DateRead);

            await db.ExecuteAsync("UpdateUserNotification", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task DeleteUserNotificationAsync (int userNotificationId)
    {
        using ( IDbConnection db = new SqlConnection(_connectionString) )
        {
            var parameters = new { UserNotificationId = userNotificationId };
            await db.ExecuteAsync("DeleteUserNotification", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task<IEnumerable<UserNotification>> GetUserNotificationsByUserIdAsync (int userId)
    {
        using ( IDbConnection db = new SqlConnection(_connectionString) )
        {
            var parameters = new { UserId = userId };
            return await db.QueryAsync<UserNotification>("GetUserNotificationsByUserId", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task MarkNotificationAsReadAsync (int userNotificationId)
    {
        using ( IDbConnection db = new SqlConnection(_connectionString) )
        {
            var parameters = new { UserNotificationId = userNotificationId };
            await db.ExecuteAsync("MarkNotificationAsRead", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}


