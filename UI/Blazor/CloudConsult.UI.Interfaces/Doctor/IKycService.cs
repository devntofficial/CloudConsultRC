using CloudConsult.UI.Data.Common;
using Microsoft.AspNetCore.Components.Forms;

namespace CloudConsult.UI.Interfaces.Doctor
{
    public interface IKycService
    {
        Task<ApiResponse> Upload(string profileId, List<IBrowserFile> kycDocuments);
    }
}
