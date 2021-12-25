using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Responses;

namespace CloudConsult.Doctor.Domain.Queries
{
    public class DownloadOneKyc : IQuery<KycDocumentResponse>
    {
        public string ProfileId { get; set; }
        public string FileName { get; set; }
    }
}
