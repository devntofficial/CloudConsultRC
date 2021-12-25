namespace CloudConsult.UI.Data.Doctor
{
    public class KycMetadataResponseData
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
