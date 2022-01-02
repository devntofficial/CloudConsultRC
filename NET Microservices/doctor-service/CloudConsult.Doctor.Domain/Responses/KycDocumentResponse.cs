namespace CloudConsult.Doctor.Domain.Responses
{
    public class KycDocumentResponse
    {
        public string FileType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = null;
    }
}
