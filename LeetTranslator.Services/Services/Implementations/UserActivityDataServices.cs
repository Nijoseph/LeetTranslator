using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LeetTranslator.Services.Interfaces;
using LeetTranslator.Core.Models;

namespace LeetTranslator.Services.Implementations
{
    public class UserActivityDataServices : IUserActivityDataServices
    {
        private readonly string _connectionString;

        public UserActivityDataServices(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<UserActivity>("GetAllUserActivities", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<UserActivity> GetUserActivityByIdAsync(int userActivityId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserActivityId = userActivityId };
                return await db.QuerySingleOrDefaultAsync<UserActivity>("GetUserActivityById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> InsertUserActivityAsync(UserActivity userActivity)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userActivity.UserId);
                parameters.Add("@ActivityType", userActivity.ActivityType);
                parameters.Add("@ActivityDescription", userActivity.ActivityDescription);
                parameters.Add("@ActivityDate", userActivity.ActivityDate);
                parameters.Add("@IsDeleted", userActivity.IsDeleted);

                return await db.ExecuteScalarAsync<int>("InsertUserActivity", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateUserActivityAsync(UserActivity userActivity)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserActivityId", userActivity.UserActivityId);
                parameters.Add("@UserId", userActivity.UserId);
                parameters.Add("@ActivityType", userActivity.ActivityType);
                parameters.Add("@ActivityDescription", userActivity.ActivityDescription);
                parameters.Add("@ActivityDate", userActivity.ActivityDate);
                parameters.Add("@IsDeleted", userActivity.IsDeleted);

                await db.ExecuteAsync("UpdateUserActivity", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteUserActivityAsync(int userActivityId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserActivityId = userActivityId };
                await db.ExecuteAsync("DeleteUserActivity", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<UserActivity>> GetUserActivitiesByUserIdAsync(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserId = userId };
                return await db.QueryAsync<UserActivity>("GetUserActivitiesByUserId", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}