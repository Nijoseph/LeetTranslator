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
    public class TranslationTypeDataServices : ITranslationTypeDataServices
    {
        private readonly string _connectionString;

        public TranslationTypeDataServices(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<TranslationType>> GetAllTranslationTypesAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<TranslationType>("GetAllTranslationTypes", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<TranslationType> GetTranslationTypeByIdAsync(int translationTypeId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { TranslationTypeId = translationTypeId };
                return await db.QuerySingleOrDefaultAsync<TranslationType>("GetTranslationTypeById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> InsertTranslationTypeAsync(TranslationType translationType)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TypeName", translationType.TypeName);
                parameters.Add("@ApiKey", translationType.ApiKey);
                parameters.Add("@ApiUrl", translationType.ApiUrl);
                parameters.Add("@IsDeleted", translationType.IsDeleted);

                return await db.ExecuteScalarAsync<int>("InsertTranslationType", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateTranslationTypeAsync(TranslationType translationType)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TranslationTypeId", translationType.TranslationTypeId);
                parameters.Add("@TypeName", translationType.TypeName);
                parameters.Add("@ApiKey", translationType.ApiKey);
                parameters.Add("@ApiUrl", translationType.ApiUrl);
                parameters.Add("@IsDeleted", translationType.IsDeleted);

                await db.ExecuteAsync("UpdateTranslationType", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteTranslationTypeAsync(int translationTypeId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { TranslationTypeId = translationTypeId };
                await db.ExecuteAsync("DeleteTranslationType", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}