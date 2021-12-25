using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Data.Doctor;
using Microsoft.AspNetCore.Components.Forms;

namespace CloudConsult.UI.Interfaces.Doctor
{
    public interface IKycService
    {
        Task<ApiResponse> Upload(string profileId, List<IBrowserFile> kycDocuments);
        Task<ApiResponse<KycMetadataResponseData>> GetMetadata(string profileId, CancellationToken cancellationToken = default);
        Task<ApiResponse<KycDocumentResponseData>> DownloadAll(string profileId, CancellationToken cancellationToken = default);
        Task<ApiResponse<KycDocumentResponseData>> DownloadOne(string profileId, string fileName, CancellationToken cancellationToken = default);
    }
}
