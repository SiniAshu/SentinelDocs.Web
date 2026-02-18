using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SentinelDocs.Core.DTO;
using SentinelDocs.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace SentinelDocs.Infrastructure.Repositories
{
    public class DocumentRepository :IDocumentRepository
    {
        private readonly string? connectionString;

        public DocumentRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task SaveDocument(DocumentUploadDto document)
        {
            using SqlConnection conn = new(connectionString);
            conn.Open();

            using SqlTransaction trans = conn.BeginTransaction();
            try
            {
                // 1. Save Document and get back the ID
                string docSql = "INSERT INTO Documents (FileName,UploadDate,TotalHits) " +
                    "OUTPUT INSERTED.DocID VALUES (@Name,@UploadDate,@TotalHits)";
                SqlCommand docCmd = new(docSql, conn, trans);

                docCmd.Parameters.AddWithValue("@Name", document.FileName);
                docCmd.Parameters.AddWithValue("@UploadDate", System.DateTime.Now);
                docCmd.Parameters.AddWithValue("@TotalHits", document.MetaData.Count);

                // Use ExecuteScalarAsync
                var result = await docCmd.ExecuteScalarAsync();
                int newDocId = Convert.ToInt32(result);

                await SaveMetaData(document, conn, trans, newDocId);

                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback(); 
                throw;
            }
        }

        private static async Task SaveMetaData(DocumentUploadDto dto, SqlConnection conn, SqlTransaction trans, int newDocId)
        {
            foreach (var item in dto.MetaData)
            {
                string metaSql = "INSERT INTO DocumentMetadata (DocID, DataCategory, ExtractedValue) " +
                               "VALUES (@DocID, @Key, @Val)";
                SqlCommand metaCmd = new(metaSql, conn, trans);

                metaCmd.Parameters.AddWithValue("@DocID", newDocId);
                metaCmd.Parameters.AddWithValue("@Key", item.Category);
                metaCmd.Parameters.AddWithValue("@Val", item.Value);
                await metaCmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<DocumentUploadDto>> GetAllDocuments()
        {
            var documents = new List<DocumentUploadDto>();
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new("GetAllDocuments", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    documents.Add(new DocumentUploadDto()
                    {
                        FileName = reader["FileName"].ToString(),
                        UploadDate = Convert.ToDateTime(reader["UploadDate"]),
                        TotalHits = Convert.ToInt32(reader["TotalHits"]),
                        ErrorCount = Convert.ToInt32(reader["ErrorCount"]),
                        WarningCount = Convert.ToInt32(reader["WarningCount"]),
                    });
                }
            }

            return documents;
        }
    }
}