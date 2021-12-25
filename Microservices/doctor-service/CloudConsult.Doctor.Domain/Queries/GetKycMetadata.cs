using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Responses;

namespace CloudConsult.Doctor.Domain.Commands
{
    public class GetKycMetadata : IQuery<KycMetadataResponse>
    {
        public string ProfileId { get; set; }
    }
}
