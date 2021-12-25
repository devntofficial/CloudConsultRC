namespace CloudConsult.UI.Data.Doctor
{
    public class KycDocumentResponseData
    {
        public string FileType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = null;
    }
}
