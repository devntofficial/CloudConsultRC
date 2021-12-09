namespace CloudConsult.Doctor.Domain.Responses
{
    public class KycDocumentResponse
    {
        public string FileType { get; set; } = string.Empty;
        public string ArchiveName { get; set; } = string.Empty;
        public byte[] ArchiveData { get; set; } = null;
    }
}
