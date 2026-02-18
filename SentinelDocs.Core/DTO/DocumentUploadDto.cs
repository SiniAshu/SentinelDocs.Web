namespace SentinelDocs.Core.DTO
{
    /// <summary>
    /// Data Transfer Object used to transport uploaded file information 
    /// and its extracted metadata from the Service layer to the Repository.
    /// </summary>
    public class DocumentUploadDto
    {
        /// <summary>
        /// Represents the unique Id for each upload.
        /// </summary>
        public int DocumentID { get; set; }

        /// <summary>
        /// Gets or sets the original name of the uploaded file (e.g., "server_log.txt").
        /// </summary>
        /// <value>The string name including the extension.</value>
        public required string FileName { get; set; }

        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of key-value pairs extracted from the document content.
        /// Keys represent categories (e.g., "Email", "IP"), and values represent the found data.
        /// </summary>
        /// <example>{"Email", "admin@corp.com"}, {"IP", "192.168.1.1"}</example>
        public List<MetadataDto> MetaData { get; set; }

        public int TotalHits { get; set; }

        public int ErrorCount { get; set; }

        public int WarningCount { get; set; }
    }
}
