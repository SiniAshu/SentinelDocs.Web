using Microsoft.AspNetCore.Http;
using SentinelDocs.Core.DTO;
using SentinelDocs.Infrastructure.Interfaces;
using SentinelDocs.Services.Interfaces;
using System.Text.RegularExpressions;

namespace SentinelDocs.Services.Services
{
    public class DocumentService(IDocumentRepository repo) : IDocumentService
    {
        private readonly IDocumentRepository DocRepository = repo;

        public async Task ProcessDocumentAsync(IFormFile file)
        {
            // Use a List instead of a Dictionary to allow multiple entries for "Email", "IP", etc.
            var metadataList = new List<MetadataDto>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string content = await reader.ReadToEndAsync();

                // 1. Extract All Emails
                var emailMatches = Regex.Matches(content, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
                foreach (Match m in emailMatches)
                    metadataList.Add(new MetadataDto { Category = "Email", Value = m.Value });

                // 2. Extract All IP Addresses
                var ipMatches = Regex.Matches(content, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                foreach (Match m in ipMatches)
                    metadataList.Add(new MetadataDto { Category = "IP_Address", Value = m.Value });

                // 3. Extract All Statuses
                var statusMatches = Regex.Matches(content, @"(CRITICAL|WARNING|ERROR|INFO)");
                foreach (Match m in statusMatches)
                    metadataList.Add(new MetadataDto { Category = "Status", Value = m.Value });
            }

            var uploadDto = new DocumentUploadDto
            {
                FileName = file.FileName,
                MetaData = metadataList
            };

            await DocRepository.SaveDocument(uploadDto);
        }

        public async Task<IEnumerable<DocumentUploadDto>> GetAllDocuments()
        {
           return await DocRepository.GetAllDocuments();
        }
    }
}
