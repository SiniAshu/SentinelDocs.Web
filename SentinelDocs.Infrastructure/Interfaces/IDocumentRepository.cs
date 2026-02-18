using SentinelDocs.Core.DTO;

namespace SentinelDocs.Infrastructure.Interfaces
{
    /// <summary>
    /// Contract for document and metadata persistence operations.
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Atomically saves a document record and its associated metadata to the database.
        /// </summary>
        Task SaveDocument(DocumentUploadDto dto);

        /// <summary>
        /// Retrieves a summary of all processed documents.
        /// </summary>
        Task<IEnumerable<DocumentUploadDto>> GetAllDocuments();

        Task<DocumentUploadDto> GetDocument(int id);
    }
}
