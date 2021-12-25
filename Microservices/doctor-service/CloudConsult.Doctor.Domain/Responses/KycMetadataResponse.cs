namespace CloudConsult.Doctor.Domain.Responses
{
    public class KycMetadataResponse
    {
        public string ProfileId { get; set; }
        public List<KycMetadata> KycDocumentsMetadata { get; set; }
    }

    public class KycMetadata
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public DateTime UploadTimestamp { get; set; }
    }
}
