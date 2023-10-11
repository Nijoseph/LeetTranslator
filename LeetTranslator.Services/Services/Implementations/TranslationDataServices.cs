using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LeetTranslator.Models;
using LeetTranslator.Services.Interfaces;

namespace LeetTranslator.Services.Implementations
{
    public class TranslationDataServices : ITranslationDataServices
    {
        private readonly string _connectionString;
        public IEnumerable<TranslationView> ListOfTranslations { get; set; }
        public int TotalCount { get; set; }
        public TranslationDataServices(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<Translation>> GetAllTranslationsAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Translation>("GetAllTranslations", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Translation> GetTranslationByIdAsync(int translationId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { TranslationId = translationId };
                return await db.QuerySingleOrDefaultAsync<Translation>("GetTranslationById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> InsertTranslationAsync(Translation translation)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", translation.UserId);
                parameters.Add("@TranslationTypeId", translation.TranslationTypeId);
                parameters.Add("@OriginalText", translation.OriginalText);
                parameters.Add("@TranslatedText", translation.TranslatedText);
                parameters.Add("@Date", translation.Date);
                parameters.Add("@IsDeleted", translation.IsDeleted);

                return await db.ExecuteScalarAsync<int>("InsertTranslation", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<TranslationResult> GetAllTranslationsWithDetailsAsync ()
        {
            using ( IDbConnection db = new SqlConnection(_connectionString) )
            {
                

                var translations = await db.QueryAsync<TranslationView>("GetAllTranslationsWithDetails", commandType: CommandType.StoredProcedure);
                var result = new TranslationResult
                {
                    ListOfTranslations = translations,
                    TotalCount = translations.Count() 
                };

                return result;
            }
        }

        public async Task UpdateTranslationAsync(Translation translation)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TranslationId", translation.TranslationId);
                parameters.Add("@UserId", translation.UserId);
                parameters.Add("@TranslationTypeId", translation.TranslationTypeId);
                parameters.Add("@OriginalText", translation.OriginalText);
                parameters.Add("@TranslatedText", translation.TranslatedText);
                parameters.Add("@Date", translation.Date);
                parameters.Add("@IsDeleted", translation.IsDeleted);

                await db.ExecuteAsync("UpdateTranslation", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteTranslationAsync(int translationId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { TranslationId = translationId };
                await db.ExecuteAsync("DeleteTranslation", parameters, commandType: CommandType.StoredProcedure);
            }
        }

 
    }
}