using Microsoft.AspNetCore.Http;
using SentinelDocs.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentinelDocs.Services.Interfaces
{
    public  interface IDocumentService
    {
        /// <summary>
        /// Reads an uploaded file, extracts metadata using Regex, and saves it to the database.
        /// </summary>
        /// <param name="file">The uploaded file from the MVC form.</param>
        Task ProcessDocumentAsync(IFormFile file);

        /// <summary>
        /// Gets all documents for the dashboard view.
        /// </summary>
        Task<IEnumerable<DocumentUploadDto>> GetAllDocuments();

        Task<DocumentUploadDto> GetDocument(int id);
    }
}
