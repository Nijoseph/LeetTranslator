using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LeetTranslator.Core.Models;

namespace LeetTranslator.Services.Implementations
{
    public class UserDataServices : IUserDataServices
    {
        private readonly string _connectionString;
        private readonly ILogger<UserDataServices> _logger;

        public UserDataServices(ILogger<UserDataServices> logger, string connectionString)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<UserAccount>> GetAllUsersAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<UserAccount>("GetAllUsers", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<UserAccount> GetUserByIdAsync(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserId = userId };
                return await db.QuerySingleOrDefaultAsync<UserAccount>("GetUserById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<UserAccount> GetUserByUserNameAsync(string UserName)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserName };
                return await db.QuerySingleOrDefaultAsync<UserAccount>("GetUserByUserName", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> InsertUserAsync(UserAccount user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash, user.UserName);
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@UserRole", user.UserRole);
                parameters.Add("@PasswordHash", user.PasswordHash);
                parameters.Add("@Email", user.Email);
                parameters.Add("@DateCreated", user.DateCreated);
                parameters.Add("@LastLogin", user.LastLogin);
                parameters.Add("@IsDeleted", user.IsDeleted);
                return await db.ExecuteScalarAsync<int>("InsertUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateUserAsync(UserAccount user)
        {
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = HashPassword(user.PasswordHash, user.UserName);
            }
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.UserId);
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@UserRole", user.UserRole);
                parameters.Add("@PasswordHash", user.PasswordHash);
                parameters.Add("@Email", user.Email);
                parameters.Add("@DateCreated", user.DateCreated);
                parameters.Add("@LastLogin", user.LastLogin);
                parameters.Add("@IsDeleted", user.IsDeleted);
                await db.ExecuteAsync("UpdateUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public string HashPassword(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserId = userId };
                await db.ExecuteAsync("DeleteUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
